using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobSites;
using UnityEngine;

namespace Assets.Societies.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override Vector3 EastConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 NorthConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 SouthConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 WestConnectionOffset {
            get { return Vector3.zero; }
        }

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                throw new NotImplementedException();
            }
        }

        public override float BlobRealignmentSpeedPerSecond {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
