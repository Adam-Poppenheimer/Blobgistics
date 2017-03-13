using System;

using UnityEngine;

using Assets.Blobs;

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

        public ResourceSummary Cost {
            get { return _cost; }
        }
        [SerializeField] private ResourceSummary _cost;

        #endregion

        #region constructors

        public BlobHighwayProfile(float blobSpeedPerSecond, int capacity, ResourceSummary cost) {
            _blobSpeedPerSecond = blobSpeedPerSecond;
            _capacity = capacity;
            _cost = cost;
        }

        #endregion

    }

}