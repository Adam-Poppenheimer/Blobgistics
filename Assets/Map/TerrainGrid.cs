using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class TerrainGrid : TerrainGridBase {

        #region instance fields and properties

        public override int Radius {
            get { return _radius; }
            set { _radius = value; }
        }
        [SerializeField] private int _radius;

        public override float MaxAcquisitionDistance {
            get { return _maxAcquisitionDistance; }
            set { _maxAcquisitionDistance = value; }
        }
        [SerializeField] private float _maxAcquisitionDistance;

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        [SerializeField] private TerrainMaterialRegistry TerrainMaterialRegistry;
        [SerializeField] private GameObject HexTilePrefab;

        #endregion

        #region instance methods

        public override void ClearMap() {
            for(int i = tiles.Count - 1; i >= 0; --i) {
                DestroyHexTile(tiles[i]);
            }
            CoordsToHex.Clear();
            HexToCoords.Clear();
            tiles.Clear();
            foreach(var mapNode in MapGraph.Nodes) {
                mapNode.ClearAssociatedTiles();
            }
        }

        public override void CreateMap() {
            for(int q = -Radius; q <= Radius; ++q) {
                int r1 = Math.Max(-Radius, -q - Radius);
                int r2 = Math.Min(Radius, -q + Radius);
                for(int r = r1; r <= r2; ++r) {
                    ConstructHexTile(q, r);
                }
            }
            foreach(var tile in tiles) {
                CoordsToHex[tile.Coordinates] = tile;
                HexToCoords[tile] = tile.Coordinates;
            }
        }

        public override void RefreshMapTerrains() {
            foreach(var node in MapGraph.Nodes) {
                node.ClearAssociatedTiles();
            }
            if(MapGraph.Nodes.Count == 0) {
                foreach(var tile in tiles) {
                    tile.Terrain = TerrainType.Water;
                }
            }else {
                foreach(var tile in tiles) {
                    var tilePosition = tile.transform.position;
                    var nearestNode = MapGraph.Nodes.Aggregate(delegate(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
                        var distanceToOne = Vector2.Distance(tilePosition, nodeOne.transform.position);
                        var distanceToTwo = Vector2.Distance(tilePosition, nodeTwo.transform.position);
                        if(distanceToOne <= distanceToTwo) {
                            return nodeOne;
                        }else {
                            return nodeTwo;
                        }
                    });
                    if(Vector3.Distance(tilePosition, nearestNode.transform.position) <= MaxAcquisitionDistance) {
                        nearestNode.AddAssociatedTile(tile);
                    }else {
                        tile.Terrain = TerrainType.Water;
                    }
                }
            }
            foreach(var node in MapGraph.Nodes) {
                node.RefreshOutline();
            }
        }

        private void ConstructHexTile(int q, int r) {
            var newHexCoords = new HexCoords(q, r, -q - r);
            Vector2 locationOfNewHex = HexGridLayout.HexCoordsToPixel(Layout, newHexCoords);

            TerrainHexTile newHexTile;
            if(HexTilePrefab == null) {
                newHexTile = (new GameObject()).AddComponent<TerrainHexTile>();
            }else {
                newHexTile = Instantiate(HexTilePrefab).GetComponent<TerrainHexTile>();
            }

            newHexTile.transform.SetParent(this.transform, false);
            newHexTile.transform.localPosition = locationOfNewHex;
            newHexTile.transform.localScale = (Vector3)Layout.Size + new Vector3(0f, 0f, 1f);
            newHexTile.SetCoordinates(newHexCoords);
            newHexTile.TerrainMaterialRegistry = TerrainMaterialRegistry;
            newHexTile.ParentGrid = this;
            newHexTile.gameObject.name = string.Format("Hex[{0}, {1}, {2}]", q, r, -q - r);

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
