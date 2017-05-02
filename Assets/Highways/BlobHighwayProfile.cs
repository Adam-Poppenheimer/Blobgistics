using System;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    [Serializable]
    public class BlobHighwayProfile : BlobHighwayProfileBase {

        #region instance fields and properties

        #region from BlobHighwayProfileBase

        public override float BlobSpeedPerSecond {
            get { return _blobSpeedPerSecond; }
        }
        public void SetBlobSpeedPerSecond(float value) {
            _blobSpeedPerSecond = value;
        }
        [SerializeField] private float _blobSpeedPerSecond;

        public override int Capacity {
            get { return _capacity; }
        }
        public void SetCapacity(int value) {
            _capacity = value;
        }
        [SerializeField] private int _capacity;

        public override float BlobPullCooldownInSeconds {
            get { return _blobPullCooldownInSeconds; }
        }
        public void SetBlobPullCooldownInSeconds(float value) {
            _blobPullCooldownInSeconds = value;
        }
        [SerializeField] private float _blobPullCooldownInSeconds;

        #endregion

        #endregion

    }

}