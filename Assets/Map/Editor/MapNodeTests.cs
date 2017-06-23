using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Map.ForTesting;

using UnityCustomUtilities.Grids;

namespace Assets.Map.Editor {

    public class MapNodeTests {

        #region instance methods

        #region tests

        [Test]
        public void OnAddAssociatedTileCalled_TileAppearsInAssociatedTileCollection() {
            //Setup
            var nodeToTest = BuildMapNode();
            var newAssociate = BuildTerrainHexTile(Vector2.zero);

            //Execution
            nodeToTest.AddAssociatedTile(newAssociate);

            //Validation
            Assert.Contains(newAssociate, nodeToTest.AssociatedTiles);
        }

        [Test]
        public void OnAddAssociatedTileCalled_TileIsGivenTheAppropriateTerrain() {
            //Setup
            var nodeToTest = BuildMapNode();
            nodeToTest.Terrain = TerrainType.Forest;

            var newAssociate = BuildTerrainHexTile(Vector2.zero);
            newAssociate.Terrain = TerrainType.Water;

            //Execution
            nodeToTest.AddAssociatedTile(newAssociate);

            //Validation
            Assert.AreEqual(nodeToTest.Terrain, newAssociate.Terrain);
        }

        [Test]
        public void OnClearAssociatedTilesCalled_AssociatedTilesCollectionBecomesEmpty() {
            //Setup
            var nodeToTest = BuildMapNode();

            nodeToTest.AddAssociatedTile(BuildTerrainHexTile(Vector2.zero));
            nodeToTest.AddAssociatedTile(BuildTerrainHexTile(Vector2.zero));
            nodeToTest.AddAssociatedTile(BuildTerrainHexTile(Vector2.zero));
            nodeToTest.AddAssociatedTile(BuildTerrainHexTile(Vector2.zero));

            //Execution
            nodeToTest.ClearAssociatedTiles();

            //Validation
            Assert.AreEqual(0, nodeToTest.AssociatedTiles.Count);
        }

        [Test]
        public void OnRefreshOutlineCalled_LineRendererIsGivenALoopingSetOfPoints() {
            //Setup
            var terrainGrid = BuildTerrainTileHexGrid();
            terrainGrid.Layout = new HexGridLayout(HexGridOrientationType.Pointy, new Vector2(1f, 1f), Vector2.zero); 
            terrainGrid.Radius = 2;
            terrainGrid.MaxAcquisitionDistance = 1f;
            terrainGrid.MapGraph = BuildMockMapGraph();
            terrainGrid.ClearMap();
            terrainGrid.CreateMap();

            var nodeToTest = BuildMapNode();
            nodeToTest.TerrainGrid = terrainGrid;

            TerrainHexTile centerTile;
            terrainGrid.TryGetTileOfCoords(new HexCoords(0, 0, 0), out centerTile);

            foreach(var tile in terrainGrid.GetTilesInRadius(centerTile, 1)) {
                nodeToTest.AddAssociatedTile(tile);
            }

            //Execution
            nodeToTest.RefreshOutline();

            //Validation
            var lineRenderer = nodeToTest.GetComponent<LineRenderer>();
            Assert.That(lineRenderer.GetPosition(0) == lineRenderer.GetPosition(lineRenderer.positionCount - 1));
        }

        [Test]
        public void OnRefreshOutlineCalled_AllPositionsBesidesTheFirstAndLastAreUnique() {
            //Setup
            var terrainGrid = BuildTerrainTileHexGrid();
            terrainGrid.Layout = new HexGridLayout(HexGridOrientationType.Pointy, new Vector2(1f, 1f), Vector2.zero); 
            terrainGrid.Radius = 2;
            terrainGrid.MaxAcquisitionDistance = 1f;
            terrainGrid.MapGraph = BuildMockMapGraph();
            terrainGrid.ClearMap();
            terrainGrid.CreateMap();

            var nodeToTest = BuildMapNode();
            nodeToTest.TerrainGrid = terrainGrid;

            TerrainHexTile centerTile;
            terrainGrid.TryGetTileOfCoords(new HexCoords(0, 0, 0), out centerTile);

            foreach(var tile in terrainGrid.GetTilesInRadius(centerTile, 1)) {
                nodeToTest.AddAssociatedTile(tile);
            }

            //Execution
            nodeToTest.RefreshOutline();

            //Validation
            var lineRenderer = nodeToTest.GetComponent<LineRenderer>();
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(positions);

            var positionsList = new List<Vector3>(positions);

            for(int i = 1; i < positionsList.Count - 1; ++i) {
                var currentPosition = positionsList[i];
                Assert.AreEqual(1, positionsList.Where(position => position.Equals(currentPosition)).Count(),
                    string.Format("Position {0} at index {1} is not unique", currentPosition, i));
            }
        }

        [Test]
        public void OnRefreshOutlineCalled_AllPositionsAreWithinOneHexEdgeLengthOfTheirNeighbors() {
            //Setup
            var terrainGrid = BuildTerrainTileHexGrid();
            terrainGrid.Layout = new HexGridLayout(HexGridOrientationType.Pointy, new Vector2(1f, 1f), Vector2.zero); 
            terrainGrid.Radius = 2;
            terrainGrid.MaxAcquisitionDistance = 1f;
            terrainGrid.MapGraph = BuildMockMapGraph();
            terrainGrid.ClearMap();
            terrainGrid.CreateMap();

            var nodeToTest = BuildMapNode();
            nodeToTest.TerrainGrid = terrainGrid;

            TerrainHexTile centerTile;
            terrainGrid.TryGetTileOfCoords(new HexCoords(0, 0, 0), out centerTile);

            foreach(var tile in terrainGrid.GetTilesInRadius(centerTile, 1)) {
                nodeToTest.AddAssociatedTile(tile);
            }

            //Execution
            nodeToTest.RefreshOutline();

            //Validation
            var lineRenderer = nodeToTest.GetComponent<LineRenderer>();
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(positions);
            for(int i = 0; i < positions.Length - 1; ++i) {
                var currentPosition = positions[i];
                var positionAfter   = positions[i + 1];
                Assert.That(Mathf.Approximately(terrainGrid.Layout.Size.x, Vector3.Distance(currentPosition, positionAfter)),
                    string.Format(
                        "Position {0} at index {1} and position {2} at index {3} are more than {4} apart from each-other",
                        currentPosition, i, positionAfter, i + 1, terrainGrid.Layout.Size.x
                    )
                );
            }
        }

        #endregion

        #region utilities

        private MapNode BuildMapNode() {
            var newNode = (new GameObject()).AddComponent<MapNode>();
            newNode.TerrainOutlineRenderer = newNode.gameObject.AddComponent<LineRenderer>();
            return newNode;
        }

        private TerrainHexTile BuildTerrainHexTile(Vector2 position) {
            return (new GameObject()).AddComponent<TerrainHexTile>();
        }

        private TerrainGrid BuildTerrainTileHexGrid() {
            return (new GameObject()).AddComponent<TerrainGrid>();
        }

        private MockMapGraph BuildMockMapGraph() {
            return (new GameObject()).AddComponent<MockMapGraph>();
        }

        #endregion

        #endregion

    }

}
