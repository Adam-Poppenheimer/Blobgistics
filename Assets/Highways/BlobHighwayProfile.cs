using System;

using UnityEngine;

namespace Assets.Highways {

    [Serializable]
    public struct BlobHighwayProfile {

        #region instance fields and properties

        public float BlobSpeedPerSecond {
            get { return _blobSpeedPerSecond; }
        }
        [SerializeField] private float _blobSpeedPerSecond;

        public int Capacity {
            get { return _capacity; }
        }
        [SerializeField] private int _capacity;

        #endregion

        #region constructors

        public BlobHighwayProfile(float blobSpeedPerSecond, int capacity) {
            _blobSpeedPerSecond = blobSpeedPerSecond;
            _capacity = capacity;
        }

        #endregion

    }

}