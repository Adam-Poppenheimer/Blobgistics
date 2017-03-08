using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BlobSites.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override Vector3 NorthConnectionOffset {
            get {
                return new Vector3(0f, 1f, 0f);
            }
        }

        public override Vector3 SouthConnectionOffset {
            get {
                return new Vector3(0f, -2f, 0f);
            }
        }

        public override Vector3 EastConnectionOffset {
            get {
                return new Vector3(3f, 0f, 0f);
            }
        }

        public override Vector3 WestConnectionOffset {
            get {
                return new Vector3(-4f, 0f, 0f);
            }
        }

        #endregion

        #endregion

    }

}
