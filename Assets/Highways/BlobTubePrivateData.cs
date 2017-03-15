using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using UnityEngine;

namespace Assets.Highways {

    public class BlobTubePrivateData : BlobTubePrivateDataBase {

        #region instance fields and properties

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        #endregion

    }

}
