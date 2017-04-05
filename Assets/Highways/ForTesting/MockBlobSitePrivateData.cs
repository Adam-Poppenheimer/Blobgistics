using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobSites;
using UnityEngine;

namespace Assets.Highways.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override Vector3 NorthConnectionOffset {
            get { return _northConnectionOffset; }
        }
        public void SetNorthConnectionOffset(Vector3 value) {
            _northConnectionOffset = value;
        }
        private Vector3 _northConnectionOffset = Vector3.zero;

        public override Vector3 SouthConnectionOffset {
            get { return _southConnectionOffset; }
        }
        public void SetSouthConnectionOffset(Vector3 value) {
            _southConnectionOffset = value;
        }
        private Vector3 _southConnectionOffset = Vector3.zero;

        public override Vector3 EastConnectionOffset {
            get { return _eastConnectionOffset; }
        }
        public void SetEastConnectionOffset(Vector3 value) {
            _eastConnectionOffset = value;
        }
        private Vector3 _eastConnectionOffset = Vector3.zero;

        public override Vector3 WestConnectionOffset {
            get { return _westConnectionOffset; }
        }
        public void SetWestConnectionOffset(Vector3 value) {
            _westConnectionOffset = value;
        }
        private Vector3 _westConnectionOffset = Vector3.zero;

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                if(_alignmentStrategy == null) {
                    _alignmentStrategy = gameObject.AddComponent<BoxyBlobAlignmentStrategy>();
                }
                return _alignmentStrategy;
            }
        }
        private BlobAlignmentStrategyBase _alignmentStrategy;

        public override float BlobRealignmentSpeedPerSecond {
            get { return 1f; }
        }

        #endregion

        #endregion

    }

}
