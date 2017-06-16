using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    public class TerrainTileHexGrid : HexGrid<TerrainHexTile> {

        #region instance fields and properties

        [SerializeField] private int MapRadius;

        [SerializeField] private TerrainMaterialRegistry TerrainMaterialRegistry;
        [SerializeField] private MapGraphBase MapGraph;

        [SerializeField] private float MaxTerrainAcquisitionDistance;
        [SerializeField] private GameObject HexTilePrefab;

        #endregion

        #region instance methods

        private void Start() {
            CoordsToHex.Clear();
            HexToCoords.Clear();
            foreach(var tile in tiles) {
                CoordsToHex[tile.Coordinates] = tile;
                HexToCoords[tile] = tile.Coordinates;
            }
        }

        public void ClearMap() {
            for(int i = tiles.Count - 1; i >= 0; --i) {
                DestroyHexTile(tiles[i]);
            }
            CoordsToHex.Clear();
            HexToCoords.Clear();
            tiles.Clear();
        }

        public void CreateMap() {
            for(int q = -MapRadius; q <= MapRadius; ++q) {
                int r1 = Math.Max(-MapRadius, -q - MapRadius);
                int r2 = Math.Min(MapRadius, -q + MapRadius);
                for(int r = r1; r <= r2; ++r) {
                    ConstructHexTile(q, r);
                }
            }
        }

        public void RefreshMapTerrains() {
            foreach(var tile in tiles) {
                var tilePosition = tile.transform.position;
                var nearestNode = MapGraph.Nodes.Aggregate(delegate(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
                    var distanceToOne = Vector3.Distance(tilePosition, nodeOne.transform.position);
                    var distanceToTwo = Vector3.Distance(tilePosition, nodeTwo.transform.position);
                    if(distanceToOne <= distanceToTwo) {
                        return nodeOne;
                    }else {
                        return nodeTwo;
                    }
                });
                if(Vector3.Distance(tilePosition, nearestNode.transform.position) <= MaxTerrainAcquisitionDistance) {
                    tile.gameObject.SetActive(true);
                    tile.Terrain = nearestNode.Terrain;
                }else {
                    tile.gameObject.SetActive(false);
                }
            }
        }

        private void ConstructHexTile(int q, int r) {
            var newHexCoords = new HexCoords(q, r, -q - r);
            Vector2 locationOfNewHex = HexGridLayout.HexCoordsToPixel(Layout, newHexCoords);

            var newHexTile = Instantiate(HexTilePrefab).GetComponent<TerrainHexTile>();

            newHexTile.transform.SetParent(this.transform, false);
            newHexTile.transform.localPosition = locationOfNewHex;
            newHexTile.transform.localScale = (Vector3)Layout.Size + new Vector3(0f, 0f, 1f);
            newHexTile.SetCoordinates(newHexCoords);
            newHexTile.TerrainMaterialRegistry = TerrainMaterialRegistry;

            tiles.Add(newHexTile);
            CoordsToHex.Add(newHexCoords, newHexTile);
            HexToCoords.Add(newHexTile, newHexCoords);
        }

        private void DestroyHexTile(TerrainHexTile tile) {
            tiles.Remove(tile);
            CoordsToHex.Remove(tile.Coordinates);
            HexToCoords.Remove(tile);

            if(Application.isPlaying) {
                Destroy(tile.gameObject);
            }else {
                DestroyImmediate(tile.gameObject);
            }
        }

        #endregion

    }

}
