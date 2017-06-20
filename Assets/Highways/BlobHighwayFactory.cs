using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.Blobs;
using Assets.Core;

namespace Assets.Highways {

    [ExecuteInEditMode]
    public class BlobHighwayFactory : BlobHighwayFactoryBase {

        #region instance fields and properties

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set {
                if(_mapGraph != null) {
                    _mapGraph.MapNodeUnsubscribed -= MapGraph_MapNodeUnsubscribed;
                    _mapGraph.MapEdgeUnsubscribed -= MapGraph_MapEdgeUnsubscribed;
                }
                _mapGraph = value;
                if(_mapGraph != null) {
                    _mapGraph.MapNodeUnsubscribed += MapGraph_MapNodeUnsubscribed;
                    _mapGraph.MapEdgeUnsubscribed += MapGraph_MapEdgeUnsubscribed;
                }
            }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobTubeFactoryBase BlobTubeFactory {
            get { return _blobTubeFactory; }
            set { _blobTubeFactory = value; }
        }
        [SerializeField] private BlobTubeFactoryBase _blobTubeFactory;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public BlobHighwayProfile HighwayProfile {
            get { return _highwayProfile; }
            set { _highwayProfile = value; }
        }
        [SerializeField] private BlobHighwayProfile _highwayProfile;

        public override ReadOnlyCollection<BlobHighwayBase> Highways {
            get { return AllConstructedHighways.AsReadOnly(); }
        }
        [SerializeField] private List<BlobHighwayBase> AllConstructedHighways = new List<BlobHighwayBase>();

        [SerializeField] private GameObject HighwayPrefab;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnValidate() {
            if(MapGraph != null) {
                MapGraph.MapNodeUnsubscribed -= MapGraph_MapNodeUnsubscribed;
                MapGraph.MapEdgeUnsubscribed -= MapGraph_MapEdgeUnsubscribed;

                MapGraph.MapNodeUnsubscribed += MapGraph_MapNodeUnsubscribed;                
                MapGraph.MapEdgeUnsubscribed += MapGraph_MapEdgeUnsubscribed;
            }
        }

        private void Start() {
            if(MapGraph != null) {
                MapGraph.MapNodeUnsubscribed -= MapGraph_MapNodeUnsubscribed;
                MapGraph.MapEdgeUnsubscribed -= MapGraph_MapEdgeUnsubscribed;

                MapGraph.MapNodeUnsubscribed += MapGraph_MapNodeUnsubscribed;
                MapGraph.MapEdgeUnsubscribed += MapGraph_MapEdgeUnsubscribed;
            }
        }

        #endregion

        #region from BlobHighwayFactoryBase

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            if(firstEndpoint == null) {
                throw new ArgumentNullException("firstEndpoint");
            }else if(secondEndpoint == null){
                throw new ArgumentNullException("secondEndpoint");
            }

            return AllConstructedHighways.Where(delegate(BlobHighwayBase highway) {
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

            var highwayQuery = AllConstructedHighways.Where(delegate(BlobHighwayBase highway) {
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

            return !HasHighwayBetween(firstEndpoint, secondEndpoint) && MapGraph.GetEdge(firstEndpoint, secondEndpoint) != null;
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

            newHighway.Profile = HighwayProfile;

            newHighway.TubePullingFromFirstEndpoint = BlobTubeFactory.ConstructTube(
                firstEndpoint.transform.position, secondEndpoint.transform.position
            );
            newHighway.TubePullingFromSecondEndpoint = BlobTubeFactory.ConstructTube(
                secondEndpoint.transform.position, firstEndpoint.transform.position
            );

            newHighway.SetEndpoints(firstEndpoint, secondEndpoint);
            newHighway.gameObject.SetActive(true);

            SubscribeHighway(newHighway);            
            return newHighway;
        }

        public override void SubscribeHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }

            highway.ParentFactory = this;
            highway.UIControl = UIControl;
            highway.BlobFactory = BlobFactory;

            AllConstructedHighways.Add(highway);

            if(EventSystem.current != null) {
                EventSystem.current.SetSelectedGameObject(highway.gameObject);
            }
            RaiseHighwaySubscribed(highway);
        }

        public override BlobHighwayBase GetHighwayOfID(int id) {
            return AllConstructedHighways.Find(highway => highway.ID == id);
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            UnsubscribeHighway(highway);
            if(Application.isPlaying) {
                Destroy(highway.gameObject);
            }else {
                DestroyImmediate(highway.gameObject);
            }
        }

        public override void UnsubscribeHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            AllConstructedHighways.Remove(highway);
            RaiseHighwayUnsubscribed(highway);
        }

        #endregion

        private void MapGraph_MapNodeUnsubscribed(object sender, MapNodeEventArgs e) {
            var highwaysToDestroy = new List<BlobHighwayBase>(
                AllConstructedHighways.Where(delegate(BlobHighwayBase highway) {
                    return highway.FirstEndpoint == e.Node || highway.SecondEndpoint == e.Node;
                })
            );
            foreach(var highway in highwaysToDestroy) {
                DestroyHighway(highway);
            }
        }

        private void MapGraph_MapEdgeUnsubscribed(object sender, MapEdgeEventArgs e) {
            var highwaysToDestroy = new List<BlobHighwayBase>(
                AllConstructedHighways.Where(delegate(BlobHighwayBase highway) {
                    return MapGraph.GetEdge(highway.FirstEndpoint, highway.SecondEndpoint) == null;
                })
            );
            foreach(var highway in highwaysToDestroy) {
                DestroyHighway(highway);
            }
        }

        #endregion
        
    }

}
