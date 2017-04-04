using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.HighwayUpgraders.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

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

        public override Vector3 EastConnectionOffset {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 NorthConnectionOffset {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 SouthConnectionOffset {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 WestConnectionOffset {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion
        
    }

}
