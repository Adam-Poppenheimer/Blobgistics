using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    public abstract class FlexibleCostConstructionProjectBase : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] protected int NumberOfResourcesRequired;
        [SerializeField] protected List<ResourceType> ResourceTypesAccepted = new List<ResourceType>();

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void SetSiteForProject(BlobSiteBase site) {
            site.ClearContents();
            site.ClearPermissionsAndCapacity();
            foreach(var resourceType in ResourceTypesAccepted) {
                site.SetPlacementPermissionForResourceType(resourceType, true);
                site.SetCapacityForResourceType(resourceType, NumberOfResourcesRequired);
            }
            site.TotalCapacity = NumberOfResourcesRequired;
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return site.Contents.Count >= NumberOfResourcesRequired;
        }

        public override ResourceDisplayInfo GetCostInfo() {
            return new ResourceDisplayInfo(ResourceTypesAccepted, NumberOfResourcesRequired);
        }

        #endregion

        #endregion

    }

}
