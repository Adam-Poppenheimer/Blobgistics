using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    /// <summary>
    /// The standard implementation of TerrainGridBase.
    /// This is used in tandem with MapGraphBase to define and display a cohesive map.
    /// </summary>
    [ExecuteInEditMode]
    public class TerrainGrid : TerrainGridBase {

        #region instance fields and properties

        #region from TerrainGridBase

        /// <inheritdoc/>
        public override int Radius {
            get { return _radius; }
            set { _radius = value; }
        }
        [SerializeField] private int _radius;

        /// <inheritdoc/>
        public override float MaxAcquisitionDistance {
            get { return _maxAcquisitionDistance; }
            set { _maxAcquisitionDistance = value; }
        }
        [SerializeField] private float _maxAcquisitionDistance;

        /// <inheritdoc/>
        public override Rect Bounds {
            get {
                return bounds;
            }
        }
        private Rect bounds = new Rect();

        #endregion

        /// <summary>
        /// The MapGraphBase whose nodes will receive associativity data from this TerrainGrid.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        [SerializeField] private TerrainMaterialRegistry TerrainMaterialRegistry;
        [SerializeField] private GameObject HexTilePrefab;

        #endregion

        #region instance methods

        /// <inheritdoc/>
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
            RefreshLocalBounds();
        }

        /// <inheritdoc/>
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

            RefreshLocalBounds();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This method assigns every TerrainHexTile to its nearest MapNodeBase. It also tells every MapNodeBase 
        /// whichTerrainHexTiles it should associate with, and then to refresh its outlines.
        /// </remarks>
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

        //Hex tiles are defined in Cubic coordinates. HexCoords and HexGridLayout are utility functions in
        //UnityCustomUtilities.Grids that facilitate the proper orientation of hexagonal grids.
        private void ConstructHexTile(int q, int r) {
            var newHexCoords = new HexCoords(q, r, -q - r);
            Vector2 locationOfNewHex = HexGridLayout.HexCoordsToWorldSpace(Layout, newHexCoords);

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

        //The bounds of the TerrainGrid are just its minimum and maximum points. More complicated
        //bounding calculation might be necessary to intelligently orient the camera in relation
        //to the map.
        private void RefreshLocalBounds() {
            if(tiles.Count < 0) {
                bounds = new Rect();
                return;
            }
            float xMin = float.PositiveInfinity;
            float xMax = float.NegativeInfinity;
            float yMin = float.PositiveInfinity;
            float yMax = float.NegativeInfinity;

            foreach(var tile in tiles) {
                var meshRenderer = tile.GetComponent<MeshRenderer>();
                if(meshRenderer != null) {
                    xMin = Mathf.Min(xMin, meshRenderer.bounds.min.x);
                    xMax = Mathf.Max(xMax, meshRenderer.bounds.max.x);

                    yMin = Mathf.Min(yMin, meshRenderer.bounds.min.y);
                    yMax = Mathf.Max(yMax, meshRenderer.bounds.max.y);
                }
            }

            bounds.xMin = xMin;
            bounds.xMax = xMax;
            bounds.yMin = yMin;
            bounds.yMax = yMax;
        }

        #endregion

    }

}
