using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.ResourceDepots;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.ConstructionZones {

    public class ResourceDepotConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        public ResourceSummary Cost {
            get {
                if(_cost == null) {
                    throw new InvalidOperationException("Cost is uninitialized");
                } else {
                    return _cost;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _cost = value;
                }
            }
        }
        [SerializeField]private ResourceSummary _cost;

        [SerializeField] private ResourceDepotFactoryBase DepotFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            DepotFactory.ConstructDepotAt(location);
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        public override string GetCostSummaryString() {
            return Cost.GetSummaryString();
        }

        #endregion

        #endregion

    }

}