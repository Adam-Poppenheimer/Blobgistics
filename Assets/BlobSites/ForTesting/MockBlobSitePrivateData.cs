using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using UnityEngine;

namespace Assets.BlobSites.ForTesting {

    public class MockBlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override float ConnectionCircleRadius {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get {
                if(_alignmentStrategy == null) {
                    var hostingObject = new GameObject();
                    _alignmentStrategy = hostingObject.AddComponent<BoxyBlobAlignmentStrategy>();
                }
                return _alignmentStrategy;
            }
        }
        public void SetAlignmentStrategy(BlobAlignmentStrategyBase value) {
            _alignmentStrategy = value;
        }
        private BlobAlignmentStrategyBase _alignmentStrategy = null;

        public override float BlobRealignmentSpeedPerSecond {
            get { return 0f; }
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
