using System;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    /// <summary>
    /// An abstract base class for tube configuration data.
    /// </summary>
    public abstract class BlobTubePrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The desired with of the tube.
        /// </summary>
        public abstract float TubeWidth { get; }

        /// <summary>
        /// The blob factory the tube should use to destroy blobs.
        /// </summary>
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        #endregion

    }

}