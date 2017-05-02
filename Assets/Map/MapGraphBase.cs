using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public abstract class MapGraphBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<MapNodeBase> Nodes { get; }
        public abstract ReadOnlyCollection<MapEdgeBase> Edges { get; }

        #endregion

        #region instance methods

        public abstract MapNodeBase BuildNode(Vector3 localPosition);
        public abstract void SubscribeNode(MapNodeBase node);

        public abstract void AddUndirectedEdge(MapNodeBase first, MapNodeBase second);
        public abstract bool RemoveUndirectedEdge(MapNodeBase first, MapNodeBase second);
        public abstract bool RemoveUndirectedEdge(MapEdgeBase edge);

        public abstract bool RemoveNode(MapNodeBase nodeToRemove);

        public abstract MapNodeBase GetNodeOfID(int id);

        public abstract bool        HasEdge(MapNodeBase first, MapNodeBase second);
        public abstract MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second);

        public abstract IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node);
        public abstract IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node);

        public abstract List<NodeDistanceSearchResults> GetNodesWithinDistanceOfEdge(MapEdgeBase edge, uint distanceInEdges);

        public abstract int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2);

        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2);

        #endregion

    }

}
