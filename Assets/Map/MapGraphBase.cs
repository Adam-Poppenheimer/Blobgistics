using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [ExecuteInEditMode]
    public abstract class MapGraphBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<MapNodeBase> Nodes { get; }
        public abstract ReadOnlyCollection<MapEdgeBase> Edges { get; }

        #endregion

        #region events

        public event EventHandler<MapNodeEventArgs> MapNodeUnsubscribed;
        public event EventHandler<MapEdgeEventArgs> MapEdgeUnsubscribed;

        protected void RaiseMapNodeUnsubscribed(MapNodeBase node) {
            if(MapNodeUnsubscribed != null) {
                MapNodeUnsubscribed(this, new MapNodeEventArgs(node));
            }
        }

        protected void RaiseMapEdgeUnsubscribed(MapEdgeBase edge) {
            if(MapEdgeUnsubscribed != null) {
                MapEdgeUnsubscribed(this, new MapEdgeEventArgs(edge));
            }
        }

        #endregion

        #region instance methods

        public abstract MapNodeBase BuildNode(Vector3 localPosition);
        public abstract MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain);

        public abstract void DestroyNode(MapNodeBase node);

        public abstract void SubscribeNode  (MapNodeBase node);
        public abstract void UnsubscribeNode(MapNodeBase node);

        public abstract MapEdgeBase BuildMapEdge(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract void DestroyMapEdge(MapNodeBase first, MapNodeBase second);
        public abstract void DestroyMapEdge(MapEdgeBase edge);

        public abstract void SubscribeMapEdge(MapEdgeBase edge);
        public abstract void UnsubscribeMapEdge(MapEdgeBase edge);

        public abstract MapNodeBase GetNodeOfID(int id);

        public abstract MapEdgeBase GetEdge(MapNodeBase endpointOne, MapNodeBase endpointTwo);

        public abstract IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node);
        public abstract IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node);

        public abstract int GetDistanceBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo);

        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo);

        public abstract NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue);

        #endregion

    }

}
