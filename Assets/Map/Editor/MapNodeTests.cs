using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

namespace Assets.Map.Editor {

    public class MapNodeTests {

        #region instance methods

        #region tests

        [Test]
        public void OnAddAssociatedTileCalled_TileAppearsInAssociatedTileCollection() {
            //Setup
            var nodeToTest = BuildMapNode();
            var newAssociate = BuildTerrainHexTile();

            //Execution
            nodeToTest.AddAssociatedTile(newAssociate);

            //validation
            Assert.Contains(newAssociate, nodeToTest.AssociatedTiles);
        }

        [Test]
        public void OnAddAssociatedTileCalled_TileIsGivenTheAppropriateTerrain() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnClearAssociatedTilesCalled_AssociatedTilesCollectionBecomesEmpty() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnRefreshOutlineCalled_LineRendererIsGivenALineThatEncirclesAllAssociatedTiles() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private MapNode BuildMapNode() {
            var newNode = (new GameObject()).AddComponent<MapNode>();
            return newNode;
        }

        private TerrainHexTile BuildTerrainHexTile() {
            return (new GameObject()).AddComponent<TerrainHexTile>();
        }

        private MockTerrainTileHexGrid BuildMockTerrainTile() {
            return (new GameObject()).AddComponent<MockTerrainTileHexGrid>();
        }

        #endregion

        #endregion

    }

}
