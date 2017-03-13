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

        public MapGraphBase MapGraph {
            get {
                if(_mapGraph == null) {
                    throw new InvalidOperationException("MapGraph is uninitialized");
                } else {
                    return _mapGraph;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _mapGraph = value;
                }
            }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayProfile StartingProfile {
            get { return _startingProfile; }
            set { _startingProfile = value; }
        }
        [SerializeField] private BlobHighwayProfile _startingProfile;

        public BlobHighwayPrivateData HighwayPrivateData {
            get {
                if(_highwayPrivateData == null) {
                    throw new InvalidOperationException("HighwayPrivateData is uninitialized");
                } else {
                    return _highwayPrivateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _highwayPrivateData = value;
                }
            }
        }
        [SerializeField] private BlobHighwayPrivateData _highwayPrivateData;

        [SerializeField] private List<BlobHighway> AllConstructedHighways = new List<BlobHighway>();

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint.BlobSite  && highway.SecondEndpoint == secondEndpoint.BlobSite) ||
                    (highway.FirstEndpoint == secondEndpoint.BlobSite && highway.SecondEndpoint == firstEndpoint.BlobSite )
                );
            }).Count() > 0;
        }

        public override BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
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

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return !HasHighwayBetween(firstEndpoint, secondEndpoint) && MapGraph.HasEdge(firstEndpoint, secondEndpoint);
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var hostingObject = new GameObject();
            hostingObject.transform.SetParent(MapGraph.transform);
            var newHighway = hostingObject.AddComponent<BlobHighway>();

            newHighway.Profile = StartingProfile;

            var newPrivateData = HighwayPrivateData.Clone(hostingObject);
            newPrivateData.SetID(AllConstructedHighways.Count);
            newPrivateData.SetFirstEndpoint(firstEndpoint);
            newPrivateData.SetSecondEndpoint(secondEndpoint);

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
