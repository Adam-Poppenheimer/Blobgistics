using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Highways;

namespace Assets.Highways.ForTesting {

    public class MockBlobTubePrivateData : BlobTubePrivateDataBase {

        #region instance fields and properties

        #region from BlobTubePrivateDataBase

        public override float MeshNonLengthDimensions {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        private ResourceBlobFactoryBase _blobFactory;

        #endregion

        #endregion

    }

}
