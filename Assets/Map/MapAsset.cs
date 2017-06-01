using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [CreateAssetMenu(fileName = "New Map Asset", menuName = "Strategy Blobs/Create New Map Asset")]
    public class MapAsset : ScriptableObject {

        #region instance fields and properties

        public ReadOnlyCollection<SaveableNodeSummary> NodeSummaries {
            get { return nodeSummaries.AsReadOnly(); }
        }
        [SerializeField] private List<SaveableNodeSummary> nodeSummaries = new List<SaveableNodeSummary>();

        public ReadOnlyCollection<SaveableEdgeSummary> EdgeSummaries {
            get { return edgeSummaries.AsReadOnly(); }
        }
        [SerializeField] private List<SaveableEdgeSummary> edgeSummaries = new List<SaveableEdgeSummary>();

        public ReadOnlyCollection<SaveableNeighborhoodSummary> NeighborhoodSummaries {
            get { return neighborhoodSummaries.AsReadOnly(); }
        }
        [SerializeField] private List<SaveableNeighborhoodSummary> neighborhoodSummaries = new List<SaveableNeighborhoodSummary>();

        #endregion

        #region instance methods

        public void LoadMapGraphInto(MapGraphBase mapGraph) {
            foreach(var node in mapGraph.Nodes) {
                nodeSummaries.Add(new SaveableNodeSummary(node.ID, node.transform.localPosition, node.Terrain));
            }
            foreach(var edge in mapGraph.Edges) {
                edgeSummaries.Add(new SaveableEdgeSummary(edge.ID, edge.FirstNode.ID, edge.SecondNode.ID));
            }
            foreach(var neighborhood in mapGraph.GetComponentsInChildren<Neighborhood>()) {
                var listOfNodeIDs = neighborhood.GetComponentsInChildren<MapNodeBase>().Select(node => node.ID).ToList();
                var listOfEdgeIDs = neighborhood.GetComponentsInChildren<MapEdgeBase>().Select(edge => edge.ID).ToList();

                neighborhoodSummaries.Add(new SaveableNeighborhoodSummary(
                    neighborhood.GetInstanceID(),
                    neighborhood.name,
                    neighborhood.transform.localPosition,
                    neighborhood.transform.localRotation,
                    listOfNodeIDs,
                    listOfEdgeIDs
                ));
            }
        }

        #endregion

    }

}
