using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BlobSites {

    public class BlobSiteFactory : BlobSiteFactoryBase {

        #region instance fields and properties

        public BlobSitePrivateDataBase BlobSitePrivateData {
            get {
                if(_blobSitePrivateData == null) {
                    throw new InvalidOperationException("BlobSitePrivateData is uninitialized");
                } else {
                    return _blobSitePrivateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _blobSitePrivateData = value;
                }
            }
        }
        [SerializeField] private BlobSitePrivateDataBase _blobSitePrivateData;

        #endregion

        #region instance methods

        #region from BlobSiteFactoryBase

        public override BlobSiteBase ConstructBlobSite(GameObject hostingObject) {
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.PrivateData = BlobSitePrivateData;

            return newBlobSite;
        }

        #endregion

        #endregion
        
    }

}
