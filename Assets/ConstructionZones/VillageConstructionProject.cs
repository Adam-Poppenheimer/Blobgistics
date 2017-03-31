using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.Societies;

namespace Assets.ConstructionZones {

    public class VillageConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private ResourceSummary Cost;

        [SerializeField] private ComplexityDefinitionBase VillageDefinition;

        [SerializeField] private SocietyFactoryBase SocietyFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            if(SocietyFactory.CanConstructSocietyAt(location)) {
                SocietyFactory.ConstructSocietyAt(location, SocietyFactory.StandardComplexityLadder, VillageDefinition);
            }
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        #endregion

        #endregion
        
    }

}
