using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    /// <summary>
    /// A simple class that manages the materials for each terrain type.
    /// </summary>
    [ExecuteInEditMode]
    public class TerrainMaterialRegistry : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The material that MeshRenderers on grassland tiles should use.
        /// </summary>
        public Material GrasslandMaterial {
            get { return _grasslandMaterial; }
        }
        [SerializeField] private Material _grasslandMaterial;

        /// <summary>
        /// The material that MeshRenderers on forest tiles should use.
        /// </summary>
        public Material ForestMaterial {
            get { return _forestMaterial; }
        }
        [SerializeField] private Material _forestMaterial;

        /// <summary>
        /// The material that MeshRenderers on mountain tiles should use.
        /// </summary>
        public Material MountainsMaterial {
            get { return _mountainsMaterial; }
        }
        [SerializeField] private Material _mountainsMaterial;

        /// <summary>
        /// The material that MeshRenderers on desert tiles should use.
        /// </summary>
        public Material DesertMaterial {
            get { return _desertMaterial; }
        }
        [SerializeField] private Material _desertMaterial;

        /// <summary>
        /// The material that MeshRenderers on water tiles should use.
        /// </summary>
        public Material WaterMaterial {
            get { return _waterMaterial; }
        }
        [SerializeField] private Material _waterMaterial;

        #endregion

        #region events

        /// <summary>
        /// Fires whenever one of the terrain materials has changed.
        /// </summary>
        /// <remarks>
        /// This event exists to make it easier for designers to modify the
        /// material associated with a given terrain type. It allows terrain
        /// tiles to automatically update their appearance when something has
        /// changed.
        /// </remarks>
        public event EventHandler<EventArgs> TerrainMaterialChanged;

        /// <summary>
        /// Fires the TerrainMaterialChanged event.
        /// </summary>
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

        /// <summary>
        /// Retrieves the material that should be used to render a particular terrain.
        /// </summary>
        /// <param name="terrain">The terrain type whose material should be retrieved</param>
        /// <returns>The material to be used</returns>
        public Material GetMaterialForTerrain(TerrainType terrain) {
            switch(terrain) {
                case TerrainType.Grassland: return GrasslandMaterial;
                case TerrainType.Forest:    return ForestMaterial;
                case TerrainType.Mountains: return MountainsMaterial;
                case TerrainType.Desert:    return DesertMaterial;
                case TerrainType.Water:     return WaterMaterial;
                default: return null;
            }
        }

        #endregion

    }

}
