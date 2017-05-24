using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;

namespace Assets.Map.ForTesting {

    public class MockBlobSiteConfiguration : BlobSiteConfigurationBase {

        #region instance fields and properties

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        public override float BlobRealignmentSpeedPerSecond {
            get {
                throw new NotImplementedException();
            }
        }

        public override float ConnectionCircleRadius {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion
        
    }

}
