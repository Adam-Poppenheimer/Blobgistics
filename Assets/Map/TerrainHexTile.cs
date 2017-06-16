using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    public class TerrainHexTile : HexTileBase {

        #region instance fields and properties

        #region from HexTileBase

        public override HexCoords Coordinates {
            get { return _coordinates; }
        }
        public void SetCoordinates(HexCoords value) {
            _coordinates = value;
        }
        [SerializeField] private HexCoords _coordinates;

        #endregion

        public TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set { _terrainMaterialRegistry = value; }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        public TerrainType Terrain {
            get { return _terrain; }
            set {
                _terrain = value;
                RefreshMeshRenderer();
            }
        }
        [SerializeField] private TerrainType _terrain;

        [SerializeField] private MeshRenderer MeshRenderer;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnValidate() {
            RefreshMeshRenderer();
        }

        #endregion

        public void RefreshMeshRenderer() {
            if(TerrainMaterialRegistry != null) {
                MeshRenderer.sharedMaterial = TerrainMaterialRegistry.GetMaterialForTerrain(Terrain);
            }
        }

        #endregion

    }

}
