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

        #region static methods

        public static bool operator ==(BlobHighwayProfile profile1, BlobHighwayProfile profile2) {
            return (
                profile1.BlobSpeedPerSecond == profile2.BlobSpeedPerSecond &&
                profile1.Capacity == profile2.Capacity &&
                profile1.Cost.Equals(profile2.Cost)
            );
        }

        public static bool operator !=(BlobHighwayProfile profile1, BlobHighwayProfile profile2) {
            return (
                profile1.BlobSpeedPerSecond != profile2.BlobSpeedPerSecond ||
                profile1.Capacity != profile2.Capacity ||
                !profile1.Cost.Equals(profile2.Cost)
            );
        }

        #endregion

        #region instance methods

        #region from Object

        public override bool Equals(object obj) {
            return obj is BlobHighwayProfile && this == (BlobHighwayProfile)obj;
        }

        public override int GetHashCode() {
            return _blobSpeedPerSecond.GetHashCode() ^ _capacity.GetHashCode() ^ Cost.GetHashCode();
        }

        #endregion

        #endregion

    }

}