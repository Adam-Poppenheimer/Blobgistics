using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    /// <summary>
    /// A class containing information that BlobSite should pass into UIControl whenever it catches user input.
    /// </summary>
    public class BlobSiteUISummary {

        #region instance fields and properties

        /// <summary>
        /// Equivalent to BlobSite.Configuration.ConnectionCircleRadius.
        /// </summary>
        public float ConnectionCircleRadius { get; set; }

        /// <summary>
        /// Equivalent to <see cref="BlobSite.TotalCapacity"/>.
        /// </summary>
        public int TotalCapacity { get; set; }

        /// <summary>
        /// Equivalent to <see cref="BlobSite.TotalSpaceLeft"/>.
        /// </summary>
        public int TotalSpaceLeft { get; set; }

        /// <summary>
        /// A copied equivalent of <see cref="BlobSite.Contents"/>.
        /// </summary>
        public List<ResourceBlobBase> Contents { get; set; }

        /// <summary>
        /// The Transform object associated with the original BlobSite.
        /// </summary>
        public Transform Transform { get; set; }
        
        #endregion

        #region constructors

        /// <summary>
        /// Creates a BlobSiteUISummary that summarizes the given BlobSiteBase.
        /// </summary>
        /// <param name="siteToSummarize">The site that should be summarized</param>
        public BlobSiteUISummary(BlobSiteBase siteToSummarize) {
            TotalCapacity = siteToSummarize.TotalCapacity;
            TotalSpaceLeft = siteToSummarize.TotalSpaceLeft;

            Contents = new List<ResourceBlobBase>(siteToSummarize.Contents);

            Transform = siteToSummarize.transform;

            ConnectionCircleRadius = siteToSummarize.Configuration.ConnectionCircleRadius;
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Analogous to <see cref="BlobSite.GetPointOfConnectionFacingPoint(Vector3)"/>.
        /// </summary>
        /// <remarks>
        /// This method is perhaps an instance of the copy/paste antipattern and should
        /// be consolidated with <see cref="BlobSite.GetPointOfConnectionFacingPoint(Vector3)"/>
        /// into a single utility method.
        /// </remarks>
        /// <param name="point">The point from which hypothetical resources are coming</param>
        /// <returns></returns>
        public Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            var normalizedCenterToPoint = (point - Transform.position).normalized;
            return (normalizedCenterToPoint * ConnectionCircleRadius) + Transform.position;
        }

        #endregion

    }

}
