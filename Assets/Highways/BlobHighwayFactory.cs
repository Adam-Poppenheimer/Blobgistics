using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.BlobSites;
using Assets.Core;

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

        public BlobTubeFactoryBase BlobTubeFactory {
            get {
                if(_blobTubeFactory == null) {
                    throw new InvalidOperationException("BlobTubeFactory is uninitialized");
                } else {
                    return _blobTubeFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _blobTubeFactory = value;
                }
            }
        }
        [SerializeField] private BlobTubeFactoryBase _blobTubeFactory;

        public UIControl UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControl _uiControl;

        public BlobHighwayProfile StartingProfile {
            get { return _startingProfile; }
            set { _startingProfile = value; }
        }
        [SerializeField] private BlobHighwayProfile _startingProfile;

        [SerializeField] private GameObject HighwayPrefab;

        [SerializeField, HideInInspector] private List<BlobHighway> AllConstructedHighways = new List<BlobHighway>();

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            if(firstEndpoint == null) {
                throw new ArgumentNullException("firstEndpoint");
            }else if(secondEndpoint == null){
                throw new ArgumentNullException("secondEndpoint");
            }

            return AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint  && highway.SecondEndpoint == secondEndpoint) ||
                    (highway.FirstEndpoint == secondEndpoint && highway.SecondEndpoint == firstEndpoint )
                );
            }).Count() > 0;
        }

        public override BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            if(firstEndpoint == null) {
                throw new ArgumentNullException("firstEndpoint");
            }else if(secondEndpoint == null){
                throw new ArgumentNullException("secondEndpoint");
            }else if(!HasHighwayBetween(firstEndpoint, secondEndpoint)) {
                throw new BlobHighwayException("There exists no highway between these two points");
            }

            var highwayQuery = AllConstructedHighways.Where(delegate(BlobHighway highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint  && highway.SecondEndpoint == secondEndpoint) ||
                    (highway.FirstEndpoint == secondEndpoint && highway.SecondEndpoint == firstEndpoint )
                );
            });
            if(highwayQuery.Count() > 0) {
                return highwayQuery.First();
            }else {
                return null;
            }
        }

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            if(firstEndpoint == null) {
                throw new ArgumentNullException("firstEndpoint");
            }else if(secondEndpoint == null){
                throw new ArgumentNullException("secondEndpoint");
            }

            return !HasHighwayBetween(firstEndpoint, secondEndpoint) && MapGraph.HasEdge(firstEndpoint, secondEndpoint);
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            if(firstEndpoint == null) {
                throw new ArgumentNullException("firstEndpoint");
            }else if(secondEndpoint == null){
                throw new ArgumentNullException("secondEndpoint");
            }else if(!CanConstructHighwayBetween(firstEndpoint, secondEndpoint)) {
                throw new BlobHighwayException("Cannot construct a highway between these two endpoints");
            }

            BlobHighway newHighway;
            GameObject hostingObject;
            if(HighwayPrefab != null) {
                hostingObject = Instantiate<GameObject>(HighwayPrefab);
                newHighway = hostingObject.GetComponent<BlobHighway>();
                if(newHighway == null) {
                    throw new BlobHighwayException("BlobHighwayFactory's HighwayPrefab lacks a BlobHighway component");
                }
            }else {
                hostingObject = new GameObject();
                newHighway = hostingObject.AddComponent<BlobHighway>();
            }
            
            hostingObject.name = string.Format("Highway [{0} <--> {1}]", firstEndpoint.name, secondEndpoint.name);
            hostingObject.transform.SetParent(MapGraph.transform);

            var newPrivateData = hostingObject.AddComponent<BlobHighwayPrivateData>();
            newPrivateData.SetFirstEndpoint(firstEndpoint);
            newPrivateData.SetSecondEndpoint(secondEndpoint);
            newPrivateData.SetUIControl(UIControl);

            newPrivateData.SetTubePullingFromFirstEndpoint(BlobTubeFactory.ConstructTube(
                firstEndpoint.transform.position, secondEndpoint.transform.position));

            newPrivateData.SetTubePullingFromSecondEndpoint(BlobTubeFactory.ConstructTube(
                secondEndpoint.transform.position, firstEndpoint.transform.position));

            newHighway.PrivateData = newPrivateData;
            newHighway.Profile = StartingProfile;

            AllConstructedHighways.Add(newHighway);

            return newHighway;
        }

        public override BlobHighwayBase GetHighwayOfID(int id) {
            return AllConstructedHighways.Find(highway => highway.ID == id);
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }

            var highwayToRemove = AllConstructedHighways.Where(delegate(BlobHighway highwayToCheck) {
                return highway == highwayToCheck;
            }).FirstOrDefault();
            if(highwayToRemove != null) {
                AllConstructedHighways.Remove(highwayToRemove);
                DestroyImmediate(highwayToRemove.gameObject);
            }
        }

        #endregion

        #endregion
        
    }

}
