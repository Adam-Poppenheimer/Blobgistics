using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

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

        public override string GetCostSummaryString() {
            var resourceCandidateString = "";
            foreach(var resourceType in ResourceTypesAccepted) {
                if(ResourceTypesAccepted.Last() == resourceType) {
                    resourceCandidateString += ", or " + resourceType;
                }else if(ResourceTypesAccepted.First() == resourceType) {
                    resourceCandidateString += resourceType;
                }else {
                    resourceCandidateString += ", " + resourceType;
                }
            }

            return string.Format("{0} of some combination of {1}", NumberOfResourcesRequired, resourceCandidateString);
        }

        #endregion

        #endregion

    }

}
