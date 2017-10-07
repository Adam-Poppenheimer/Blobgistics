using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    /// <summary>
    /// A hexagonal tile used by TerrainGrid to visualize the map defined in MapGraphBase.
    /// </summary>
    /// <remarks>
    /// See http://www.redblobgames.com/grids/hexagons/ for a description of
    /// the math the hex grids are based on.
    /// </remarks>
    public class TerrainHexTile : HexTileBase {

        #region instance fields and properties

        #region from HexTileBase

        /// <summary>
        /// The cubic coordinates of the hex tile in its ParentGrid.
        /// </summary>
        public override HexCoords Coordinates {
            get { return _coordinates; }
        }
        /// <summary>
        /// The setter method for the Coordinates property.
        /// </summary>
        /// <remarks>
        /// Normally, the set method for a property would be defined within the property itself.
        /// However, Coordinates should be a read-only value at all times except for initialization.
        /// Since properties do not let you declare the new keyword on their accessors
        /// or add additional accessors when inheriting, it was necessary to separate the set accessor.
        /// </remarks>
        /// <param name="value"></param>
        public void SetCoordinates(HexCoords value) {
            _coordinates = value;
        }
        [SerializeField] private HexCoords _coordinates;

        #endregion

        /// <summary>
        /// The TerrainMaterialRegsitry that the TerrainHexTile will use to inform its appearance.
        /// </summary>
        public TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set { _terrainMaterialRegistry = value; }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        /// <summary>
        /// The terrain of the tile.
        /// </summary>
        public TerrainType Terrain {
            get { return _terrain; }
            set {
                _terrain = value;
                RefreshMeshRenderer();
            }
        }
        [SerializeField] private TerrainType _terrain;

        /// <summary>
        /// The grid in which this tile lies.
        /// </summary>
        public TerrainGrid ParentGrid {
            get { return _parentGrid; }
            set { _parentGrid = value; }
        }
        [SerializeField] private TerrainGrid _parentGrid;

        [SerializeField] private MeshRenderer MeshRenderer;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnValidate() {
            RefreshMeshRenderer();
        }

        #endregion
        /// <summary>
        /// Updates the material in the attached MeshRenderer component to reflect the tile's
        /// current terrain type.
        /// </summary>
        public void RefreshMeshRenderer() {
            if(TerrainMaterialRegistry != null) {
                MeshRenderer.sharedMaterial = TerrainMaterialRegistry.GetMaterialForTerrain(Terrain);
            }
        }

        #endregion

    }

}
