using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class TerrainMaterialRegistry : MonoBehaviour {

        #region instance fields and properties

        public Material GrasslandMaterial {
            get { return _grasslandMaterial; }
        }
        [SerializeField] private Material _grasslandMaterial;

        public Material ForestMaterial {
            get { return _forestMaterial; }
        }
        [SerializeField] private Material _forestMaterial;

        public Material MountainsMaterial {
            get { return _mountainsMaterial; }
        }
        [SerializeField] private Material _mountainsMaterial;

        public Material DesertMaterial {
            get { return _desertMaterial; }
        }
        [SerializeField] private Material _desertMaterial;

        #endregion

        #region events

        public event EventHandler<EventArgs> TerrainMaterialChanged;

        protected void RaiseTerrainMaterialChanged() {
            if(TerrainMaterialChanged != null) {
                TerrainMaterialChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            RaiseTerrainMaterialChanged();
        }

        #endregion

        public Material GetMaterialForTerrain(TerrainType terrain) {
            switch(terrain) {
                case TerrainType.Grassland: return GrasslandMaterial;
                case TerrainType.Forest:    return ForestMaterial;
                case TerrainType.Mountains: return MountainsMaterial;
                case TerrainType.Desert:    return DesertMaterial;
                default: return null;
            }
        }

        #endregion

    }

}
