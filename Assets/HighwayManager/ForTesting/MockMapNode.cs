using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Core;
using Assets.Map;

namespace Assets.HighwayManager.ForTesting {

    public class MockMapNode : MapNodeBase {

        #region instance fields and properties

        #region from MapNodeBase

        public override BlobSiteBase BlobSite {
            get {
                if(_blobSite == null) {
                    _blobSite = gameObject.AddComponent<MockBlobSite>();
                }
                return _blobSite;
            }
        }
        private BlobSiteBase _blobSite;

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapGraphBase ParentGraph {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public MapGraphBase managingGraph;

        public override IEnumerable<MapNodeBase> Neighbors {
            get { return managingGraph.GetNeighborsOfNode(this); }
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

        public override TerrainTileHexGrid TerrainTileGrid {
            get {
                throw new NotImplementedException();
            }

            set {
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
