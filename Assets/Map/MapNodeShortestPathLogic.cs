using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    public class MapNodeShortestPathLogic : MapNodeShortestPathLogicBase {

        #region static fields and properties

        public static MapNodeShortestPathLogic Instance {
            get {
                if(_instance == null) {
                    _instance = new MapNodeShortestPathLogic();
                }
                return _instance;
            }
        }
        private static MapNodeShortestPathLogic _instance;

        #endregion

        #region constructors



        #endregion

        #region instance methods

        #region from MapNodeShortestPathLogicBase

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes) {
            if(node1 == null) {
                throw new ArgumentNullException("node1");
            }else if(node2 == null) {
                throw new ArgumentNullException("node2");
            }
            var shortestPath = GetShortestPathBetweenNodes(node1, node2, allNodes);
            return shortestPath != null ? shortestPath.Count : int.MaxValue;
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end, IEnumerable<MapNodeBase> allNodes) {
            if(start == null) {
                throw new ArgumentNullException("start");
            }else if(end == null) {
                throw new ArgumentNullException("end");
            }

            var previous = new Dictionary<MapNodeBase, MapNodeBase>();
            var distances = new Dictionary<MapNodeBase, int>();
            var nodesLeftToCheck = new List<MapNodeBase>();

            List<MapNodeBase> path = null;

            foreach(var node in allNodes) {
                if(node == start) {
                    distances[node] = 0;
                }else {
                    distances[node] = int.MaxValue;
                }
                nodesLeftToCheck.Add(node);
            }

            while(nodesLeftToCheck.Count != 0) {
                nodesLeftToCheck.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodesLeftToCheck[0];
                nodesLeftToCheck.Remove(smallest);

                if(smallest == end) {
                    path = new List<MapNodeBase>();

                    while(previous.ContainsKey(smallest)) {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if(distances[smallest] == int.MaxValue) {
                    break;
                }

                foreach(var neighbor in smallest.Neighbors) {
                    var alt = distances[smallest] + 1;
                    if(alt < distances[neighbor]) {
                        distances[neighbor] = alt;
                        previous[neighbor] = smallest;
                    }
                }
            }

            return path;
        }

        public override NodeDistanceSummary GetNearestNodeWhere(MapEdgeBase edgeOfOrigin, Predicate<MapNodeBase> condition, int maxDistance) {
            
            var closestFromFirstEndpoint = GetNearestNodeWhere(edgeOfOrigin.FirstNode, condition, maxDistance);
            var closestFromSecondEndpoint = GetNearestNodeWhere(edgeOfOrigin.SecondNode, condition, maxDistance);

            if(closestFromFirstEndpoint == null && closestFromSecondEndpoint == null) {
                return null;
            }else if(closestFromFirstEndpoint == null) {
                return closestFromSecondEndpoint;
            }else if(closestFromSecondEndpoint == null || closestFromFirstEndpoint.Distance <= closestFromSecondEndpoint.Distance) {
                return closestFromFirstEndpoint;
            }else {
                return closestFromSecondEndpoint;
            }
        }

        #endregion

        private NodeDistanceSummary GetNearestNodeWhere(MapNodeBase rootNode, Predicate<MapNodeBase> condition, int maxDistance) {
            var distanceSummariesToConsider = new Queue<NodeDistanceSummary>();
            var nodesAlreadyConsidered = new HashSet<MapNodeBase>();

            nodesAlreadyConsidered.Add(rootNode);
            distanceSummariesToConsider.Enqueue(new NodeDistanceSummary(rootNode, 0));

            while(distanceSummariesToConsider.Count > 0) {
                var currentSummary = distanceSummariesToConsider.Dequeue();
                if(condition(currentSummary.Node) && currentSummary.Distance <= maxDistance) {
                    return currentSummary;
                }
                foreach(var neighbor in currentSummary.Node.Neighbors) {
                    if(!nodesAlreadyConsidered.Contains(neighbor)) {
                        nodesAlreadyConsidered.Add(neighbor);
                        distanceSummariesToConsider.Enqueue(new NodeDistanceSummary(neighbor, currentSummary.Distance + 1));
                    }
                }
            }

            return null;
        }

        #endregion

    }

}
