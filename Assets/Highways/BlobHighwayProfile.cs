using System;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    /// <summary>
    /// A POD class containing configuration datat for BlobHighway.
    /// </summary>
    [Serializable]
    public class BlobHighwayProfile {

        #region instance fields and properties

        #region from BlobHighwayProfileBase

        /// <summary>
        /// The speed at which blobs will travel down the highway.
        /// </summary>
        public float BlobSpeedPerSecond {
            get { return _blobSpeedPerSecond; }
            set { _blobSpeedPerSecond = value; }
        }
        [SerializeField] private float _blobSpeedPerSecond;

        /// <summary>
        /// The maximum number of blobs that can be in transit per direction at any time.
        /// </summary>
        public int Capacity {
            get { return _capacity; }
            set { _capacity = value; }
        }
        [SerializeField] private int _capacity;

        /// <summary>
        /// The time between blob pull attempts, used to inform BlobDistributor.
        /// </summary>
        public float BlobPullCooldownInSeconds {
            get { return _blobPullCooldownInSeconds; }
            set { _blobPullCooldownInSeconds = value; }
        }
        [SerializeField] private float _blobPullCooldownInSeconds;

        #endregion

        #endregion

    }

}