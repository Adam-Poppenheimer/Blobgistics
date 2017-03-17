using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Map;
using UnityEngine;

namespace Assets.BlobDistributors.ForTesting {

    public class AccessingCountingMockMapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get {
                if(NodesAccessed != null) {
                    NodesAccessed(this, EventArgs.Empty);
                }
                return new List<MapNodeBase>().AsReadOnly();
            }
        }

        #endregion

        #endregion

        #region events

        public event EventHandler<EventArgs> NodesAccessed;

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override void AddUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
