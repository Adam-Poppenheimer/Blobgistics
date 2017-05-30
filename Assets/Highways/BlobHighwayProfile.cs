using System;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    [Serializable]
    public class BlobHighwayProfile {

        #region instance fields and properties

        #region from BlobHighwayProfileBase

        public float BlobSpeedPerSecond {
            get { return _blobSpeedPerSecond; }
            set { _blobSpeedPerSecond = value; }
        }
        [SerializeField] private float _blobSpeedPerSecond;

        public int Capacity {
            get { return _capacity; }
            set { _capacity = value; }
        }
        [SerializeField] private int _capacity;

        public float BlobPullCooldownInSeconds {
            get { return _blobPullCooldownInSeconds; }
            set { _blobPullCooldownInSeconds = value; }
        }
        [SerializeField] private float _blobPullCooldownInSeconds;

        #endregion

        #endregion

    }

}