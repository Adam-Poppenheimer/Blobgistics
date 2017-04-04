using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

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

        private BlobSitePrivateDataBase BlobSitePrivateData {
            get {
                if(_blobSitePrivateData == null) {
                    var hostingObject = new GameObject();
                    _blobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();
                    _blobSitePrivateData.SetNorthConnectionOffset(Vector3.zero);
                    _blobSitePrivateData.SetSouthConnectionOffset(Vector3.zero);
                    _blobSitePrivateData.SetEastConnectionOffset(Vector3.zero);
                    _blobSitePrivateData.SetWestConnectionOffset(Vector3.zero);
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

        #endregion

        #endregion

    }

}
