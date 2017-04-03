using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;
using Assets.HighwayUpgraders;
using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobDistributors {

    public class BlobDistributor : BlobDistributorBase {

        #region instance fields and properties

        #region from BlobDistributorBase

        public override float SecondsToPerformDistributionTick {
            get { return _secondsToPerformDistributionTick; }
            set { _secondsToPerformDistributionTick = value; }
        }
        [SerializeField] private float _secondsToPerformDistributionTick = 1f;

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

        public HighwayUpgraderFactoryBase HighwayUpgraderFactory {
            get { return _highwayUpgraderFactory; }
            set { _highwayUpgraderFactory = value; }
        }
        [SerializeField] private HighwayUpgraderFactoryBase _highwayUpgraderFactory;

        private Dictionary<BlobSiteBase, BlobHighwayBase> LastServedHighwayOnBlobSite = 
            new Dictionary<BlobSiteBase, BlobHighwayBase>();

        private Dictionary<BlobSiteBase, MapEdgeBase> LastServedEdgeOnBlobSite = 
            new Dictionary<BlobSiteBase, MapEdgeBase>();

        private float DistributionTimer = 0f;

        #endregion

        #region instance methods

        #region from BlobDistributorBase

        public override void Tick(float secondsPassed) {
            DistributionTimer += secondsPassed;
            while(DistributionTimer >= SecondsToPerformDistributionTick) {
                DistributionTimer -= SecondsToPerformDistributionTick;
                PerformDistribution();
            }
        }

        protected override void PerformDistribution() {
            foreach(var activeNode in MapGraph.Nodes) {

                var adjacentEdges = new List<MapEdgeBase>(MapGraph.GetEdgesAttachedToNode(activeNode));
                DistributeOnceFromSiteToEdges(activeNode.BlobSite, adjacentEdges);

                var adjacentHighways = new List<BlobHighwayBase>();
                foreach(var neighboringNode in MapGraph.GetNeighborsOfNode(activeNode)) {
                    if(HighwayFactory.HasHighwayBetween(activeNode, neighboringNode)) {
                        adjacentHighways.Add(HighwayFactory.GetHighwayBetween(activeNode, neighboringNode));
                    }
                }
                DistributeOnceFromSiteToHighways(activeNode.BlobSite, adjacentHighways);
            }
        }

        #endregion

        private void DistributeOnceFromSiteToEdges(BlobSiteBase site, List<MapEdgeBase> adjacentEdges) {
            MapEdgeBase lastEdgeServed;
            LastServedEdgeOnBlobSite.TryGetValue(site, out lastEdgeServed);

            if(lastEdgeServed == null) {

                //When there was no last edge
                foreach(var candidateEdge in adjacentEdges) {
                    if(AttemptTransferIntoEdge(candidateEdge, site)) {
                        LastServedEdgeOnBlobSite[site] = candidateEdge;
                        return;
                    }
                }
            }else {
                //Address candidates in round-robin fashion
                int indexOfLast = adjacentEdges.IndexOf(lastEdgeServed);
                for(
                    int i = (indexOfLast + 1) % adjacentEdges.Count;
                    i != indexOfLast;
                    i = ++i % adjacentEdges.Count
                ){
                    var candidateEdge = adjacentEdges[i];
                    if(AttemptTransferIntoEdge(candidateEdge, site)) {
                        LastServedEdgeOnBlobSite[site] = candidateEdge;
                        return;
                    }
                }
                if(AttemptTransferIntoEdge(lastEdgeServed, site)) {
                    LastServedEdgeOnBlobSite[site] = lastEdgeServed;
                    return;
                }
            }
        }

        private void DistributeOnceFromSiteToHighways(BlobSiteBase site, List<BlobHighwayBase> adjacentHighways) {
            BlobHighwayBase lastHighwayServed;
            LastServedHighwayOnBlobSite.TryGetValue(site, out lastHighwayServed);

            if(lastHighwayServed == null) {

                //When there was no last highway
                adjacentHighways.Sort(PriorityCompare);
                foreach(var candidateHighway in adjacentHighways) {
                    if(AttemptPull(candidateHighway, site)) {
                        LastServedHighwayOnBlobSite[site] = candidateHighway;
                        return;
                    }
                }

            }else {

                //When there was a last highway
                var candidatesBeforeLast  = new List<BlobHighwayBase>(adjacentHighways.Where(highway => highway.Priority <  lastHighwayServed.Priority));
                var candidatesEqualToLast = new List<BlobHighwayBase>(adjacentHighways.Where(highway => highway.Priority == lastHighwayServed.Priority));
                var candidatesAfterLast   = new List<BlobHighwayBase>(adjacentHighways.Where(highway => highway.Priority >  lastHighwayServed.Priority));

                candidatesBeforeLast.Sort(PriorityCompare);
                candidatesEqualToLast.Sort(PriorityCompare);
                candidatesAfterLast.Sort(PriorityCompare);

                //Address highways with a higher priority
                foreach(var candidateHighway in candidatesBeforeLast) {
                    if(AttemptPull(candidateHighway, site)) {
                        LastServedHighwayOnBlobSite[site] = candidateHighway;
                        return;
                    }
                }

                //Address candidates with the same priority in round-robin fashion
                int indexOfLast = candidatesEqualToLast.IndexOf(lastHighwayServed);
                for(
                    int i = (indexOfLast + 1) % candidatesEqualToLast.Count;
                    i != indexOfLast;
                    i = ++i % candidatesEqualToLast.Count
                ){
                    var candidateHighway = candidatesEqualToLast[i];
                    if(AttemptPull(candidateHighway, site)) {
                        LastServedHighwayOnBlobSite[site] = candidateHighway;
                        return;
                    }
                }
                if(AttemptPull(lastHighwayServed, site)) {
                    LastServedHighwayOnBlobSite[site] = lastHighwayServed;
                    return;
                }
                
                //Address candidates with a lower priority
                foreach(var candidateHighway in candidatesAfterLast) {
                    if(AttemptPull(candidateHighway, site)) {
                        LastServedHighwayOnBlobSite[site] = candidateHighway;
                        return;
                    }
                }

                LastServedHighwayOnBlobSite[site] = null;
            }
        }

        private bool AttemptPull(BlobHighwayBase highwayToPull, BlobSiteBase site) {
            if(highwayToPull.FirstEndpoint.BlobSite == site && highwayToPull.CanPullFromFirstEndpoint()) {
                highwayToPull.PullFromFirstEndpoint();
                return true;
            }else if(highwayToPull.SecondEndpoint.BlobSite == site && highwayToPull.CanPullFromSecondEndpoint()) {
                highwayToPull.PullFromSecondEndpoint();
                return true;
            }else {
                return false;
            }
        }

        private bool AttemptTransferIntoEdge(MapEdgeBase edge, BlobSiteBase siteToTransferFrom) {
            var edgeSite = edge.BlobSite;
            foreach(var transferCandidate in siteToTransferFrom.Contents) {
                if(siteToTransferFrom.CanExtractBlob(transferCandidate) && edgeSite.CanPlaceBlobInto(transferCandidate)) {
                    siteToTransferFrom.ExtractBlob(transferCandidate);
                    edgeSite.PlaceBlobInto(transferCandidate);
                    return true;
                }
            }
            return false;
        }

        private int PriorityCompare(BlobHighwayBase highway1, BlobHighwayBase highway2) {
            return highway1.Priority.CompareTo(highway2.Priority);
        }

        #endregion
        
    }

}
