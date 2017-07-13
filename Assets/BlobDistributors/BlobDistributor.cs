using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Profiling;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobDistributors {

    public class BlobDistributor : BlobDistributorBase {

        #region instance fields and properties

        #region from BlobDistributorBase

        public override float EdgePullCooldownInSeconds {
            get { return _edgePullCooldownInSeconds; }
            set { _edgePullCooldownInSeconds = value; }
        }
        [SerializeField] private float _edgePullCooldownInSeconds;

        #endregion

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        private Dictionary<BlobSiteBase, BlobHighwayBase> LastServedHighwayOnBlobSite = 
            new Dictionary<BlobSiteBase, BlobHighwayBase>();

        private Dictionary<BlobSiteBase, MapEdgeBase> LastServedEdgeOnBlobSite = 
            new Dictionary<BlobSiteBase, MapEdgeBase>();

        private Dictionary<BlobSiteBase, Dictionary<BlobHighwayBase, float>> PullTimerForBlobHighwayOnSite =
            new Dictionary<BlobSiteBase, Dictionary<BlobHighwayBase, float>>();

        private Dictionary<BlobSiteBase, Dictionary<MapEdgeBase, float>> PullTimerForMapEdgeOnSite = 
            new Dictionary<BlobSiteBase, Dictionary<MapEdgeBase, float>>();

        #endregion

        #region instance methods

        #region from BlobDistributorBase

        public override void Tick(float secondsPassed) {
            foreach(var activeNode in MapGraph.Nodes) {
                var adjacentHighways = HighwayFactory.GetHighwaysAttachedToNode(activeNode);

                Profiler.BeginSample("Highway Distribution");
                if(activeNode.BlobSite.Contents.Count > 0 && adjacentHighways.Count() > 0) {
                    DistributeFromSiteToHighways(activeNode.BlobSite, adjacentHighways, secondsPassed);
                }
                Profiler.EndSample();
            }
        }

        #endregion

        private void DistributeFromSiteToEdges(BlobSiteBase site, List<MapEdgeBase> adjacentEdges, float secondsPassed) {
            if(!PullTimerForMapEdgeOnSite.ContainsKey(site)) {
                PullTimerForMapEdgeOnSite[site] = new Dictionary<MapEdgeBase, float>();
            }

            foreach(var edge in adjacentEdges) {
                if(!PullTimerForMapEdgeOnSite[site].ContainsKey(edge)) {
                    PullTimerForMapEdgeOnSite[site][edge] = secondsPassed;
                }else {
                    PullTimerForMapEdgeOnSite[site][edge] += secondsPassed;
                }
            }

            

            MapEdgeBase lastEdgeServed;
            LastServedEdgeOnBlobSite.TryGetValue(site, out lastEdgeServed);

            bool continueCycling = true;
            while(continueCycling) {
                continueCycling = false;

                int indexOfLast = adjacentEdges.IndexOf(lastEdgeServed);
                if(indexOfLast < 0) {
                    foreach(var candidateEdge in adjacentEdges) {
                        if(AttemptTransferIntoEdge(candidateEdge, site)) {
                            lastEdgeServed = candidateEdge;
                            continueCycling = true;
                        }
                    }
                }else {
                    for(int i = (indexOfLast + 1) % adjacentEdges.Count;
                        i != indexOfLast;
                        i = ++i % adjacentEdges.Count
                    ){
                        var candidateEdge = adjacentEdges[i];
                        if(AttemptTransferIntoEdge(candidateEdge, site)) {
                            lastEdgeServed = candidateEdge;
                            continueCycling = true;
                        }
                    }

                    if(AttemptTransferIntoEdge(lastEdgeServed, site)) {
                        continueCycling = true;
                    }
                }
            }

            foreach(var edge in adjacentEdges) {
                PullTimerForMapEdgeOnSite[site][edge] = Mathf.Clamp(PullTimerForMapEdgeOnSite[site][edge], 0f, EdgePullCooldownInSeconds);
            }
        }

        private void DistributeFromSiteToHighways(BlobSiteBase site, IEnumerable<BlobHighwayBase> adjacentHighways, float secondsPassed) {

            var dictionaryOfPriorities = new SortedDictionary<int, List<BlobHighwayBase>>();
            foreach(var highway in adjacentHighways) {
                if(!dictionaryOfPriorities.ContainsKey(highway.Priority)) {
                    dictionaryOfPriorities.Add(highway.Priority, new List<BlobHighwayBase>());
                }
                dictionaryOfPriorities[highway.Priority].Add(highway);
            }

            BlobHighwayBase lastHighwayServedOnSite;
            LastServedHighwayOnBlobSite.TryGetValue(site, out lastHighwayServedOnSite);

            foreach(var priorityList in dictionaryOfPriorities.Values) {
                PerformRoundRobinDistributionOnHighways(site, priorityList, ref lastHighwayServedOnSite, secondsPassed);
            }
            LastServedHighwayOnBlobSite[site] = lastHighwayServedOnSite;

        }

        private void PerformRoundRobinDistributionOnHighways(BlobSiteBase site, List<BlobHighwayBase> highways,
            ref BlobHighwayBase lastHighwayServed, float secondsPassed) {
            Profiler.BeginSample("Round-Robin Distribution");

            if(!PullTimerForBlobHighwayOnSite.ContainsKey(site)) {
                PullTimerForBlobHighwayOnSite[site] = new Dictionary<BlobHighwayBase, float>();
            }

            foreach(var highway in highways) {
                if(!PullTimerForBlobHighwayOnSite[site].ContainsKey(highway)) {
                    PullTimerForBlobHighwayOnSite[site][highway] = secondsPassed;
                }else {
                    PullTimerForBlobHighwayOnSite[site][highway] += secondsPassed;
                }
            }

            bool continueCycling = true;
            while(continueCycling) {
                continueCycling = false;

                int indexOfLast = highways.IndexOf(lastHighwayServed);
                if(indexOfLast < 0) {
                    foreach(var candidateHighway in highways) {
                        if(AttemptPull(candidateHighway, site)) {
                            lastHighwayServed = candidateHighway;
                            continueCycling = true;
                        }
                    }
                }else {
                    for(int i = (indexOfLast + 1) % highways.Count;
                        i != indexOfLast;
                        i = ++i % highways.Count
                    ){
                        var candidateHighway = highways[i];
                        if(AttemptPull(candidateHighway, site)) {
                            lastHighwayServed = candidateHighway;
                            continueCycling = true;
                        }
                    }

                    if(AttemptPull(lastHighwayServed, site)) {
                        continueCycling = true;
                    }
                }
            }

            foreach(var highway in highways) {
                PullTimerForBlobHighwayOnSite[site][highway] = 
                    Mathf.Clamp(PullTimerForBlobHighwayOnSite[site][highway], 0f, highway.BlobPullCooldownInSeconds);
            }

            Profiler.EndSample();
        }

        private bool AttemptPull(BlobHighwayBase highwayToPull, BlobSiteBase site) {
            bool retval = false;

            var highwayPullTimer = PullTimerForBlobHighwayOnSite[site][highwayToPull];
            float effectiveHighwayCooldown = highwayToPull.BlobPullCooldownInSeconds;

            if(highwayPullTimer >= effectiveHighwayCooldown) {
                if(highwayToPull.FirstEndpoint.BlobSite == site && highwayToPull.CanPullFromFirstEndpoint()) {
                    highwayToPull.PullFromFirstEndpoint();
                    highwayPullTimer -= effectiveHighwayCooldown;
                    retval = true;
                }else if(highwayToPull.SecondEndpoint.BlobSite == site && highwayToPull.CanPullFromSecondEndpoint()) {
                    highwayToPull.PullFromSecondEndpoint();
                    highwayPullTimer -= effectiveHighwayCooldown;
                    retval = true;
                }
            }

            PullTimerForBlobHighwayOnSite[site][highwayToPull] = highwayPullTimer;
            return retval;
        }

        private bool AttemptTransferIntoEdge(MapEdgeBase edge, BlobSiteBase siteToTransferFrom) {
            if(!PullTimerForMapEdgeOnSite[siteToTransferFrom].ContainsKey(edge)) {
                PullTimerForMapEdgeOnSite[siteToTransferFrom][edge] = 0f;
            }

            bool retval = false;
            var edgePullTimer = PullTimerForMapEdgeOnSite[siteToTransferFrom][edge];

            var edgeSite = edge.BlobSite;
            if(edgePullTimer >= EdgePullCooldownInSeconds) {
                foreach(var transferCandidate in siteToTransferFrom.Contents) {
                    if(siteToTransferFrom.CanExtractBlob(transferCandidate) && edgeSite.CanPlaceBlobInto(transferCandidate)) {
                        siteToTransferFrom.ExtractBlob(transferCandidate);
                        edgeSite.PlaceBlobInto(transferCandidate);
                        edgePullTimer -= EdgePullCooldownInSeconds;
                        retval = true;
                        break;
                    }
                }
            }

            PullTimerForMapEdgeOnSite[siteToTransferFrom][edge] = edgePullTimer;
            return retval;
        }

        private int PriorityCompare(BlobHighwayBase highway1, BlobHighwayBase highway2) {
            return highway1.Priority.CompareTo(highway2.Priority);
        }

        #endregion
        
    }

}
