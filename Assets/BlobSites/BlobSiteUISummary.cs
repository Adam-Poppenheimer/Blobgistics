using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    public class BlobSiteUISummary {

        #region instance fields and properties

        public float ConnectionCircleRadius { get; set; }

        public int TotalCapacity { get; set; }
        public int TotalSpaceLeft { get; set; }

        public List<ResourceBlobBase> Contents { get; set; }

        public Transform Transform { get; set; }
        
        #endregion

        #region constructors

        public BlobSiteUISummary(BlobSiteBase siteToSummarize) {
            TotalCapacity = siteToSummarize.TotalCapacity;
            TotalSpaceLeft = siteToSummarize.TotalSpaceLeft;

            Contents = new List<ResourceBlobBase>(siteToSummarize.Contents);

            Transform = siteToSummarize.transform;

            ConnectionCircleRadius = siteToSummarize.ConnectionCircleRadius;
        }

        #endregion

        #region instance methods

        public Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            var normalizedCenterToPoint = (point - Transform.position).normalized;
            return (normalizedCenterToPoint * ConnectionCircleRadius) + Transform.position;
        }

        #endregion

    }

}
