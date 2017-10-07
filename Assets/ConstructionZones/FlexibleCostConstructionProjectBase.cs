using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    /// <summary>
    /// The abstract base class for construction projects who have flexible costs.
    /// </summary>
    /// <remarks>
    /// Flexibility is defined by a total number of resources required and a collection of resource types
    /// that can be used to satisfy that number. For instance, "10 of any combination of Food, Wood, and Steel"
    /// is a flexible cost, while "10 Food, 10 Wood, and 10 Steel" is not.
    /// </remarks>
    public abstract class FlexibleCostConstructionProjectBase : ConstructionProjectBase {

        #region instance fields and properties

        /// <summary>
        /// The total number of resources that must be acquired to complete the project.
        /// </summary>
        [SerializeField] protected int NumberOfResourcesRequired;

        /// <summary>
        /// The types of resources that are accepted by this project.
        /// </summary>
        [SerializeField] protected List<ResourceType> ResourceTypesAccepted = new List<ResourceType>();

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        /// <inheritdoc/>
        public override void SetSiteForProject(BlobSiteBase site) {
            site.ClearContents();
            site.ClearPermissionsAndCapacity();
            foreach(var resourceType in ResourceTypesAccepted) {
                site.SetPlacementPermissionForResourceType(resourceType, true);
                site.SetCapacityForResourceType(resourceType, NumberOfResourcesRequired);
            }
            site.TotalCapacity = NumberOfResourcesRequired;
        }

        /// <inheritdoc/>
        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return site.Contents.Count >= NumberOfResourcesRequired;
        }

        /// <inheritdoc/>
        public override ResourceDisplayInfo GetCostInfo() {
            return new ResourceDisplayInfo(ResourceTypesAccepted, NumberOfResourcesRequired);
        }

        #endregion

        #endregion

    }

}
