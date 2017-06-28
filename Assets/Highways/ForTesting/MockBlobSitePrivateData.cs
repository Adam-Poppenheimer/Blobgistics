using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.Highways.ForTesting {

    public class MockBlobSiteConfiguration : BlobSiteConfigurationBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override float ConnectionCircleRadius {
            get { return _connectionCircleRadius; }
        }
        public void SetConnectionCircleRadius(float value) {
            _connectionCircleRadius= value;
        }
        private float _connectionCircleRadius;

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

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
