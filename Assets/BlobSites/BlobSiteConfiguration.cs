using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    public class BlobSiteConfiguration : BlobSiteConfigurationBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public override float ConnectionCircleRadius {
            get { return _connectionCircleRadius; }
        }
        public void SetConnectionCircleRadius(float value) {
            _connectionCircleRadius = value;
        }
        [SerializeField] private float _connectionCircleRadius;

        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get { return _alignmentStrategy; }
        }
        public void SetAlignmentStrategy(BlobAlignmentStrategyBase value) {
            _alignmentStrategy = value;
        }
        [SerializeField] private BlobAlignmentStrategyBase _alignmentStrategy;

        public override float BlobRealignmentSpeedPerSecond {
            get { return _blobRealignmentSpeedPerSecond; }
        }
        public void SetBlobRealignmentSpeedPerSecond(float value) {
            _blobRealignmentSpeedPerSecond = value;
        }
        [SerializeField] private float _blobRealignmentSpeedPerSecond;

        #endregion

        #endregion
        
    }

}
