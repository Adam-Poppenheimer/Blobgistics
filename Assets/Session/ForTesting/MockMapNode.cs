using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Core;
using Assets.Map;

namespace Assets.Session.ForTesting {

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

        #endregion

        #endregion
        
    }

}
