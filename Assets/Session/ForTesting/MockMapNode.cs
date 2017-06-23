using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.BlobSites;
using Assets.Core;
using Assets.Map;

using UnityCustomUtilities.Grids;

namespace Assets.Session.ForTesting {

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
            get { return GetInstanceID(); }
        }

        public override IEnumerable<MapNodeBase> Neighbors {
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

        public override TerrainType Terrain { get; set; }

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

        public override TerrainGridBase TerrainGrid {
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
