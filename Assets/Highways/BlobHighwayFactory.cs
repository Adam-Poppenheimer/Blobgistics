using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.BlobSites;

namespace Assets.Highways {

    public class BlobHighwayFactory : BlobHighwayFactoryBase {

        #region instance fields and properties

        [SerializeField] private MapGraph MapGraph;
        [SerializeField] private BlobHighwayProfile StartingProfile;
        [SerializeField] private BlobHighwayPrivateData HighwayPrivateData;

        private HashSet<BlobHighway> AllConstructedHighways = new HashSet<BlobHighway>();

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool HasHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint) {
            return AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint.BlobSite  && highway.SecondEndpoint == secondEndpoint.BlobSite) ||
                    (highway.FirstEndpoint == secondEndpoint.BlobSite && highway.SecondEndpoint == firstEndpoint.BlobSite )
                );
            }).Count() > 0;
        }

        public override BlobHighwayBase GetHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint) {
            var highwayQuery = AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint.BlobSite  && highway.SecondEndpoint == secondEndpoint.BlobSite) ||
                    (highway.FirstEndpoint == secondEndpoint.BlobSite && highway.SecondEndpoint == firstEndpoint.BlobSite )
                );
            });
            if(highwayQuery.Count() > 0) {
                return highwayQuery.First();
            }else {
                return null;
            }
        }

        public override bool CanConstructHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint) {
            return !HasHighwayBetween(firstEndpoint, secondEndpoint) && MapGraph.HasEdge(firstEndpoint, secondEndpoint);
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint) {
            var hostingObject = new GameObject();
            hostingObject.transform.SetParent(MapGraph.transform);
            var newHighway = hostingObject.AddComponent<BlobHighway>();

            newHighway.Profile = StartingProfile;

            var newPrivateData = HighwayPrivateData.Clone(hostingObject);
            newPrivateData.ID = AllConstructedHighways.Count;
            newPrivateData.FirstEndpoint = firstEndpoint.BlobSite;
            newPrivateData.SecondEndpoint = secondEndpoint.BlobSite;

            newHighway.PrivateData = newPrivateData;

            AllConstructedHighways.Add(newHighway);

            return newHighway;
        }

        public override BlobHighwayBase GetHighwayOfID(int id) {
            var highwayQuery = AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return highway.ID == id;
            });
            if(highwayQuery.Count() > 0) {
                return highwayQuery.First();
            }else {
                return null;
            }
        }

        public override void RemoveHighway(BlobHighwayBase highway) {
            var highwayToRemove = AllConstructedHighways.Where(delegate(BlobHighway highwayToCheck) {
                return highway == highwayToCheck;
            }).FirstOrDefault();
            if(highwayToRemove != null) {
                AllConstructedHighways.Remove(highwayToRemove);
                Destroy(highwayToRemove);
            }
        }

        public override void TickHighways(float secondsPassed) {
            foreach(var highway in AllConstructedHighways) {
                highway.TickMovement(secondsPassed);
            }
        }

        #endregion

        #endregion
        
    }

}
