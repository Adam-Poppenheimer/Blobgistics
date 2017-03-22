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

        public int TotalCapacity { get; set; }
        public int TotalSpaceLeft { get; set; }

        public List<ResourceBlob> Contents { get; set; }

        public Vector3 NorthConnectionPoint { get; set; }
        public Vector3 SouthConnectionPoint { get; set; }
        public Vector3 EastConnectionPoint  { get; set; }
        public Vector3 WestConnectionPoint  { get; set; }

        public Transform Transform { get; set; }
        
        #endregion

        #region constructors

        public BlobSiteUISummary(BlobSiteBase siteToSummarize) {
            TotalCapacity = siteToSummarize.TotalCapacity;
            TotalSpaceLeft = siteToSummarize.TotalSpaceLeft;

            Contents = new List<ResourceBlob>(siteToSummarize.Contents);

            NorthConnectionPoint = siteToSummarize.NorthConnectionPoint;
            SouthConnectionPoint = siteToSummarize.SouthConnectionPoint;
            EastConnectionPoint = siteToSummarize.EastConnectionPoint;
            WestConnectionPoint = siteToSummarize.WestConnectionPoint;

            Transform = siteToSummarize.transform;
        }

        #endregion

        #region instance methods

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthConnectionPoint;
                case ManhattanDirection.South: return SouthConnectionPoint;
                case ManhattanDirection.East:  return EastConnectionPoint;
                case ManhattanDirection.West:  return WestConnectionPoint;
                default: return NorthConnectionPoint;
            }
        }

        #endregion

    }

}
