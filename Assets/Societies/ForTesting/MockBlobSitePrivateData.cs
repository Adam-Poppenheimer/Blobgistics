using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.Societies.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override Vector3 EastConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 NorthConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 SouthConnectionOffset {
            get { return Vector3.zero; }
        }

        public override Vector3 WestConnectionOffset {
            get { return Vector3.zero; }
        }

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                if(_alignmentStrategy == null) {
                    _alignmentStrategy = gameObject.AddComponent<BoxyBlobAlignmentStrategy>();
                }
                return _alignmentStrategy;
            }
        }
        private BlobAlignmentStrategyBase _alignmentStrategy = null;

        public override float BlobRealignmentSpeedPerSecond {
            get { return 1f; }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        private ResourceBlobFactoryBase _blobFactory;

        #endregion

        #endregion

    }

}
