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

    /// <summary>
    /// The standard implementation for BlobDistributorBase. Handles the distribution of blobs from
    /// blob sites into blob highways.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    /// This distributor attempts to distribute blobs in a round-robin fashion, giving each highway
    /// at each site approximately the same number of chances to receive each blob. This notion is
    /// complicated considerably by placement and extraction permissions on both the blob sites and
    /// the highways themselves.
    /// </para>
    /// <para>
    /// This class asserts that blob distribution isn't a responsibility of individual highways, but rather
    /// belongs entirely to the BlobDistributor. It thus stores state about individual highways that in an
    /// alternate design might belong to the highways themselves. I considered it necessary to record this
    /// state within BlobDistributor because the desired round-robin distribution implies the need to consider
    /// highways in groups rather than individually.
    /// </para> 
    /// </remarks>
    public class BlobDistributor : BlobDistributorBase {

        #region instance fields and properties

        /// <summary>
        /// The MapGraph this distributor will pull map nodes (and thus blob sites) from.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        /// <summary>
        /// The Highway factory this distributor will pull highways from.
        /// </summary>
        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        //These dictionaries store information about which highways are associated with which blob sites,
        //and how long it's been since a given highway has pulled from a given blob site. This information
        //is necessary to maintain a round robin distribution even while highways are being added and
        //removed.
        private Dictionary<BlobSiteBase, BlobHighwayBase> LastServedHighwayOnBlobSite = 
            new Dictionary<BlobSiteBase, BlobHighwayBase>();

        private Dictionary<BlobSiteBase, Dictionary<BlobHighwayBase, float>> PullTimerForBlobHighwayOnSite =
            new Dictionary<BlobSiteBase, Dictionary<BlobHighwayBase, float>>();

        #endregion

        #region instance methods

        #region from BlobDistributorBase

        /// <inheritdoc/>
        public override void Tick(float secondsPassed) {
            foreach(var activeNode in MapGraph.Nodes) {
                var adjacentHighways = HighwayFactory.GetHighwaysAttachedToNode(activeNode);
                if(activeNode.BlobSite.Contents.Count > 0 && adjacentHighways.Count() > 0) {
                    DistributeFromSiteToHighways(activeNode.BlobSite, adjacentHighways, secondsPassed);
                }
            }
        }

        #endregion

        // This method contains a holdover from a previous implementation. Highway priorities are no longer
        //an element of the game, and so much of this method could be refactored to remove the redundant
        //prioritization, though it wasn't considered a priority during production.
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

        /* This method does several things. For starters, it increments the pull timer for every highway on the
         * site. Once that's happened, it goes through the highways, starting at the index after that of the
         * last highway served, and gives it an opportunity to pull resources from the site. There are some
         * complications when there was no last highway served, and also when the last highway served is the
         * only valid highway that can pull blobs.
         * 
         */
        private void PerformRoundRobinDistributionOnHighways(BlobSiteBase site, List<BlobHighwayBase> highways,
            ref BlobHighwayBase lastHighwayServed, float secondsPassed) {
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

            //ContinueCycling is set to true (and thus permits the loop to keep spinning) if and only if
            //some highway manages to pull some blob. Normally you'd expect this loop to cycle once (if no
            //highway can pull any blob) or twice (if some highway can pull some blob) because secondsPassed
            //is usually Time.deltaTime and thus very small. This loop would only cycle more than twice
            //if secondsPassed was so large that a given highway would've pulled blobs more than once since
            //the last time tick was called. This most often happens in unit testing, but could occur if
            //framerates or highway pull cooldowns are very low.
            //A given cycle might serve multiple highways if there are enough appropriate blobs in the
            //blob site, or could serve none.
            bool continueCycling = true;
            while(continueCycling) {
                continueCycling = false;

                int indexOfLast = highways.IndexOf(lastHighwayServed);
                //When there was no last highway served, or that highway is no longer
                //attached to the site.
                if(indexOfLast < 0) {
                    foreach(var candidateHighway in highways) {
                        if(AttemptPull(candidateHighway, site)) {
                            lastHighwayServed = candidateHighway;
                            continueCycling = true;
                        }
                    }
                }else {
                    //Try to pull from the highways following the last served highway.
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

                    //Check the lastHighwayServed again, since the for loop skipped it.
                    if(AttemptPull(lastHighwayServed, site)) {
                        continueCycling = true;
                    }
                }
            }

            //This loop only comes into play when no highways were served. That only happens when
            //there are no valid blobs to distribute into them. This line makes sure that, when there
            //are blobs for the highways to pull, that they don't pull many at once. Without this line,
            //long periods of drought followed by sudden abundance can cause highways to pull in many
            //blobs at once, which compromises the notion of BlobPullCooldownInSeconds. This happens at the
            //end in case the framerate is low and might actually require us to pull multiple blobs to
            //keep up, though this is rare in the current implementation.
            foreach(var highway in highways) {
                PullTimerForBlobHighwayOnSite[site][highway] = 
                    Mathf.Clamp(PullTimerForBlobHighwayOnSite[site][highway], 0f, highway.BlobPullCooldownInSeconds);
            }
        }

        /*Three conditions must be met in order for a pull to be successful. 
            1. The pull timer for the highway on the blob site must be greater than its pull cooldown.
            2. One of the highway's endpoints is the blob site in question.
            3. The highway can pull from that endpoint.
          if these conditions are met, the highway pulls from its corresponding endpoint and reduces
          its pull timer on the current site by its cooldown.
        */
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

        private int PriorityCompare(BlobHighwayBase highway1, BlobHighwayBase highway2) {
            return highway1.Priority.CompareTo(highway2.Priority);
        }

        #endregion
        
    }

}
