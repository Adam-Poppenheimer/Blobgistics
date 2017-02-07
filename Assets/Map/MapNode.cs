using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobEngine;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class MapNode : MonoBehaviour {

        #region instance fields and properties

        public IEnumerable<MapNode> Neighbors {
            get { return ManagingGraph.GetNeighborsOfNode(this); }
        }

        public MapGraph ManagingGraph {
            get { return _managingGraph; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _managingGraph = value;
                }
            }
        }
        [SerializeField, HideInInspector] private MapGraph _managingGraph;

        public FactoryPileBase FactoryPile {
            get {
                if(_factoryPile == null) {
                    throw new InvalidOperationException("FactoryPile is uninitialized");
                } else {
                    return _factoryPile;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _factoryPile = value;
                }
            }
        }
        [SerializeField, HideInInspector] private FactoryPileBase _factoryPile;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ManagingGraph != null && !ManagingGraph.Nodes.Contains(this)) {
                ManagingGraph.SubscribeNode(this);
            }
        }

        private void OnDestroy() {
            if(ManagingGraph != null) {
                ManagingGraph.RemoveNode(this);
            }
        }

        #endregion

        [ContextMenu("Construct Resource Gyser")]
        private void ConstructResourceGyser() {
            FactoryPile.BuildingPlotFactory.ConstructResourceGyser(this, ResourceType.Red);
        }

        [ContextMenu("Construct Building Plot")]
        private void ConstructBuildingPlot() {
            FactoryPile.BuildingPlotFactory.ConstructBuildingPlot(this);
        }
        
        [ContextMenu("Construct Resource Pool")]
        private void ConstructResourcePool() {
            FactoryPile.ResourcePoolFactory.ConstructResourcePool(this);
        }

        [ContextMenu("Construct Blob Generator")]
        private void ConstructBlobGenerator() {
            FactoryPile.BlobGeneratorFactory.ConstructGenerator(this, ResourceType.Red);
        }

        #endregion

    }

}
