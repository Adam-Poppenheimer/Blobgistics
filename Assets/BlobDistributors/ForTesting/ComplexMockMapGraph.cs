using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.BlobDistributors.ForTesting {

    public class ComplexMockMapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get { return nodes.AsReadOnly(); }
        }
        private List<MapNodeBase> nodes = new List<MapNodeBase>();

        #endregion

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set {
                _blobFactory = value;
                BlobSitePrivateData.SetBlobFactory(_blobFactory);
            }
        }
        private ResourceBlobFactoryBase _blobFactory;

        private BlobSiteConfiguration BlobSitePrivateData {
            get {
                if(_blobSitePrivateData == null) {
                    var hostingObject = new GameObject();
                    _blobSitePrivateData = hostingObject.AddComponent<BlobSiteConfiguration>();
                    _blobSitePrivateData.SetConnectionCircleRadius(2f);
                    _blobSitePrivateData.SetAlignmentStrategy(gameObject.AddComponent<BoxyBlobAlignmentStrategy>());
                }
                return _blobSitePrivateData;
            }
        }
        private BlobSiteConfiguration _blobSitePrivateData = null;

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override MapEdgeBase BuildMapEdge(MapNodeBase first, MapNodeBase second) {
            var hostingObject = new GameObject();
            var newEdge = hostingObject.AddComponent<MapEdge>();

            newEdge.DisplayComponent = new GameObject().transform;
            newEdge.SetNodes(first, second);

            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.Configuration = hostingObject.AddComponent<MockBlobSitePrivateData>();
            edges.Add(newEdge);

            return newEdge;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            var hostingObject = new GameObject();
            var newNode = hostingObject.AddComponent<MockMapNode>();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.Configuration = BlobSitePrivateData;

            newNode.SetBlobSite(newBlobSite);

            nodes.Add(newNode);
            return newNode;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            throw new NotImplementedException();
        }

        public override void SubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            return edges.Find(delegate(MapEdgeBase edge) {
                return (edge.FirstNode == first && edge.SecondNode == second) || (edge.SecondNode == first && edge.FirstNode == second);
            });
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            return edges.Where(delegate(MapEdgeBase edge) {
                return edge.FirstNode == node || edge.SecondNode == node;
            });
        }

        public override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            var retval = new List<MapNodeBase>();

            foreach(var edgeConnected in edges.Where(edge => edge.FirstNode == node || edge.SecondNode == node)) {
                if(edgeConnected.FirstNode == node) {
                    retval.Add(edgeConnected.SecondNode);
                }else {
                    retval.Add(edgeConnected.FirstNode);
                }
            }

            return retval;
        }

        public override MapNodeBase GetNodeOfID(int id) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeNode(MapNodeBase nodeToRemove) {
            nodes.Remove(nodeToRemove);
            DestroyImmediate(nodeToRemove.gameObject);
        }

        public override void DestroyMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void DestroyMapEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {
            throw new NotImplementedException();
        }

        public override void DestroyNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override void SubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
