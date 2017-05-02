using System;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    public abstract class BlobTubePrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract float MeshNonLengthDimensions { get; }
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        #endregion

    }

}