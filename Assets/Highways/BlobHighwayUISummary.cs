using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways {

    /// <summary>
    /// A class containing information that BlobHighway should pass into UIControl whenever it catches user input.
    /// </summary>
    public class BlobHighwayUISummary {

        #region instance fields and properties

        /// <summary>
        /// Equivalent to <see cref="BlobHighway.ID"/>.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Equivalent to <see cref="BlobHighway.Priority"/>.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Equivalent to <see cref="BlobHighway.Efficiency"/>.
        /// </summary>
        public float Efficiency { get; set; }

        /// <summary>
        /// The transform attached to the BlobHighway.
        /// </summary>
        public Transform Transform { get; set; }

        /// <summary>
        /// The permissions for pulling from FirstEndpoint.
        /// </summary>
        public Dictionary<ResourceType, bool> ResourcePermissionsForFirstEndpoint  { get; set; }

        /// <summary>
        /// The permissions for pulling from SecondEndpoint.
        /// </summary>
        public Dictionary<ResourceType, bool> ResourcePermissionsForSecondEndpoint { get; set; }

        /// <summary>
        /// The upkeep requests by resource.
        /// </summary>
        public Dictionary<ResourceType, bool> IsRequestingUpkeepForResource { get; set; }


        /// <summary>
        /// The profile the highway is using.
        /// </summary>
        public BlobHighwayProfile Profile { get; set; }


        /// <summary>
        /// The location of the first endpoint.
        /// </summary>
        public Vector3 FirstEndpoint  { get; set; }

        /// <summary>
        /// The location of the second endpoint.
        /// </summary>
        public Vector3 SecondEndpoint { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an empty summary.
        /// </summary>
        public BlobHighwayUISummary() {}

        /// <summary>
        /// Creates a summary from the information contained in a given highway.
        /// </summary>
        /// <param name="highwayToSummarize">The highway the summary will summarize</param>
        public BlobHighwayUISummary(BlobHighwayBase highwayToSummarize) {
            ID = highwayToSummarize.ID;
            Priority = highwayToSummarize.Priority;
            Efficiency = highwayToSummarize.Efficiency;
            Transform = highwayToSummarize.transform;

            ResourcePermissionsForFirstEndpoint = new Dictionary<ResourceType, bool>();
            ResourcePermissionsForSecondEndpoint = new Dictionary<ResourceType, bool>();
            IsRequestingUpkeepForResource = new Dictionary<ResourceType, bool>();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                ResourcePermissionsForFirstEndpoint[resourceType] = highwayToSummarize.GetPullingPermissionForFirstEndpoint (resourceType);
                ResourcePermissionsForSecondEndpoint[resourceType] = highwayToSummarize.GetPullingPermissionForSecondEndpoint(resourceType);
                IsRequestingUpkeepForResource  [resourceType] = highwayToSummarize.GetUpkeepRequestedForResource        (resourceType);
            }

            Profile = highwayToSummarize.Profile;

            FirstEndpoint = highwayToSummarize.FirstEndpoint.BlobSite.GetPointOfConnectionFacingPoint(
                highwayToSummarize.SecondEndpoint.transform.position);
            SecondEndpoint = highwayToSummarize.SecondEndpoint.BlobSite.GetPointOfConnectionFacingPoint(
                highwayToSummarize.FirstEndpoint.transform.position);;
        }

        #endregion

    }

}
