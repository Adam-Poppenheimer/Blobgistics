using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.BlobSites;
using Assets.Map;

using UnityCustomUtilities.Grids;

namespace Assets.Core.ForTesting {

    public class MockMapNode : MapNodeBase {

        #region instance fields and properties

        #region from MapNodeBase

        public override BlobSiteBase BlobSite {
            get {
                throw new NotImplementedException();
            }
        }

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapGraphBase ParentGraph {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override IEnumerable<MapNodeBase> Neighbors {
            get {
                throw new NotImplementedException();
            }
        }

        public override TerrainType Terrain {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override TerrainMaterialRegistry TerrainMaterialRegistry {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override UIControlBase UIControl {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override HexGridBase<TerrainHexTile> TerrainTileGrid {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<TerrainHexTile> AssociatedTiles {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from MapNodeBase

        public override void AddAssociatedTile(TerrainHexTile tile) {
            throw new NotImplementedException();
        }

        public override void ClearAssociatedTiles() {
            throw new NotImplementedException();
        }

        public override void RefreshOutline() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
