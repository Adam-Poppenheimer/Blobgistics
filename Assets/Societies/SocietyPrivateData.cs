using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

namespace Assets.Societies {

    public class SocietyPrivateData : SocietyPrivateDataBase {

        #region instance fields and properties

        #region from SocietyPrivateDataBase

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        public override ComplexityDefinitionBase StartingComplexity {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteBase BlobSite {
            get {
                if(_blobSite == null) {
                    throw new InvalidOperationException("BlobSite is uninitialized");
                } else {
                    return _blobSite;
                }
            }
        }
        [SerializeField] private BlobSiteBase _blobSite;

        #endregion

        #endregion

    }

}
