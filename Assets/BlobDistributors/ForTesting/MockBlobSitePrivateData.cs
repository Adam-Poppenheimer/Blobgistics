using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.BlobDistributors.ForTesting {

    public class MockBlobSitePrivateData : BlobSiteConfigurationBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                if(_alignmentStrategy == null) {
                    _alignmentStrategy = gameObject.AddComponent<BoxyBlobAlignmentStrategy>();
                }
                return _alignmentStrategy;
            }
        }
        private BlobAlignmentStrategyBase _alignmentStrategy = null;

        public override float BlobRealignmentSpeedPerSecond {
            get { return 1f; }
        }

        public override float ConnectionCircleRadius {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
