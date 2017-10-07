using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    /// <summary>
    /// The standard implementation for BlobSiteConfigurationBase.
    /// </summary>
    public class BlobSiteConfiguration : BlobSiteConfigurationBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        /// <inheritdoc/>
        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        /// <summary>
        /// The externalized Set method for BlobFactory.
        /// </summary>
        /// <param name="value">The new value of BlobFactory</param>
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        /// <inheritdoc/>
        public override float ConnectionCircleRadius {
            get { return _connectionCircleRadius; }
        }
        /// <summary>
        /// The externalized Set method for ConnectionCircleRadius.
        /// </summary>
        /// <param name="value">The new value of ConnectionCircleRadius</param>
        public void SetConnectionCircleRadius(float value) {
            _connectionCircleRadius = value;
        }
        [SerializeField] private float _connectionCircleRadius;

        /// <inheritdoc/>
        public override BlobAlignmentStrategyBase AlignmentStrategy {
            get { return _alignmentStrategy; }
        }
        /// <summary>
        /// The externalized Set method for AlignmentStrategy.
        /// </summary>
        /// <param name="value">The new value of AlignmentStrategy</param>
        public void SetAlignmentStrategy(BlobAlignmentStrategyBase value) {
            _alignmentStrategy = value;
        }
        [SerializeField] private BlobAlignmentStrategyBase _alignmentStrategy;

        /// <inheritdoc/>
        public override float BlobRealignmentSpeedPerSecond {
            get { return _blobRealignmentSpeedPerSecond; }
        }
        /// <summary>
        /// The externalized Set method for BlobRealignmentSpeedPerSecond.
        /// </summary>
        /// <param name="value">The new value of BlobRealignmentSpeedPerSecond</param>
        public void SetBlobRealignmentSpeedPerSecond(float value) {
            _blobRealignmentSpeedPerSecond = value;
        }
        [SerializeField] private float _blobRealignmentSpeedPerSecond;

        #endregion

        #endregion
        
    }

}
