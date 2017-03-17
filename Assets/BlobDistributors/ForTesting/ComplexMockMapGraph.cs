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
            get {
                throw new NotImplementedException();
            }
        }

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
                }
                return _blobSitePrivateData;
            }
        }
        private BlobSitePrivateData _blobSitePrivateData = null;

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override void AddUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            
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

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            throw new NotImplementedException();
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
