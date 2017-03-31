using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Societies;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.ConstructionZones {

    public class FarmlandConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        #region from ConstructionProjectBase

        [SerializeField] private int NumberOfResourcesRequired;
        [SerializeField] private List<ResourceType> ResourceTypesAccepted = new List<ResourceType>();

        #endregion

        [SerializeField] private SocietyFactoryBase SocietyFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            if(SocietyFactory.CanConstructSocietyAt(location)) {
                SocietyFactory.ConstructSocietyAt(location, SocietyFactory.StandardComplexityLadder);
            }
        }

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

        #endregion

        #endregion

    }

}
