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

        private BlobSitePrivateData BlobSitePrivateData {
            get {
                if(_blobSitePrivateData == null) {
                    var hostingObject = new GameObject();
                    _blobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();
                    _blobSitePrivateData.SetConnectionCircleRadius(2f);
                    _blobSitePrivateData.SetAlignmentStrategy(gameObject.AddComponent<BoxyBlobAlignmentStrategy>());
                }
                return _blobSitePrivateData;
            }
        }
        private BlobSitePrivateData _blobSitePrivateData = null;

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override void AddUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            var hostingObject = new GameObject();
            var newEdge = hostingObject.AddComponent<MapEdge>();
            newEdge.SetFirstNode(first);
            newEdge.SetSecondNode(second);

            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.PrivateData = hostingObject.AddComponent<MockBlobSitePrivateData>();
            newEdge.SetBlobSite(newBlobSite);
            edges.Add(newEdge);
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            var hostingObject = new GameObject();
            var newNode = hostingObject.AddComponent<MockMapNode>();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.PrivateData = BlobSitePrivateData;

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

        public override bool HasEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override bool RemoveNode(MapNodeBase nodeToRemove) {
            var retval = nodes.Remove(nodeToRemove);
            DestroyImmediate(nodeToRemove.gameObject);
            return retval;
        }

        public override bool RemoveUndirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override bool RemoveUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override bool UnsubscribeDirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override List<NodeDistanceSearchResults> GetNodesWithinDistanceOfEdge(MapEdgeBase edge, uint distanceInEdges) {
            throw new NotImplementedException();
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
