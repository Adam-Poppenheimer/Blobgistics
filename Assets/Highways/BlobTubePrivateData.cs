using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using UnityEngine;

namespace Assets.Highways {

    /// <summary>
    /// The standard implementation of BlobTubePrivateDataBase.
    /// </summary>
    public class BlobTubePrivateData : BlobTubePrivateDataBase {

        #region instance fields and properties

        /// <inheritdoc/>
        public override float TubeWidth {
            get { return _tubeWidth; }
        }
        [SerializeField] private float _tubeWidth;

        /// <inheritdoc/>
        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        /// <summary>
        /// The externalized Set method for BlobFactory.
        /// </summary>
        /// <param name="value">The new value of BlobFactory</param>
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        #endregion

    }

}
