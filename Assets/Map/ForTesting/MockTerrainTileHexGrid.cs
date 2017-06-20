using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Grids;

namespace Assets.Map.ForTesting {

    public class MockTerrainTileHexGrid : HexGridBase<TerrainHexTile> {

        #region instance fields and properties

        #region from HexGridBase<TerrainHexTile>

        public override HexGridLayout Layout { get; set; }

        public override HexTilePathingLogicBase<TerrainHexTile> TilePathingLogic {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<TerrainHexTile> Tiles {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from HexGridBase<TerrainHexTile>

        public override int GetDistance(TerrainHexTile tileA, TerrainHexTile tileB) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetNeighbors(TerrainHexTile center) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetShorestPathBetween(TerrainHexTile start, TerrainHexTile end) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInLine(TerrainHexTile start, TerrainHexTile end) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInLine(TerrainHexTile start, TerrainHexTile end, Predicate<TerrainHexTile> condition) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInRadius(TerrainHexTile center, int radius) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInRadius(TerrainHexTile center, int radius, Predicate<TerrainHexTile> condition) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInRing(TerrainHexTile center, int radius) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<TerrainHexTile> GetTilesInRing(TerrainHexTile center, int radius, Predicate<TerrainHexTile> condition) {
            throw new NotImplementedException();
        }

        public override bool TryGetNeighborInDirection(TerrainHexTile centeredTile, int neighborDirection, out TerrainHexTile value) {
            throw new NotImplementedException();
        }

        public override bool TryGetTileOfCoords(HexCoords coords, out TerrainHexTile value) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
