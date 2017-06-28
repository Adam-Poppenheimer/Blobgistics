using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Generator {

    [ExecuteInEditMode]
    public class ResourceGenerator : MonoBehaviour {

        #region instance fields and properties

        public MapNodeBase Location {
            get { return _location; }
            set { _location = value; }
        }
        [SerializeField, HideInInspector] private MapNodeBase _location;

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set {_blobFactory = value; }
        }
        [SerializeField, HideInInspector] private ResourceBlobFactoryBase _blobFactory;

        public ResourceGeneratorFactory ParentFactory {
            get { return _parentFactory; }
            set {_parentFactory = value; }
        }
        [SerializeField, HideInInspector] private ResourceGeneratorFactory _parentFactory;

        [SerializeField] private IntPerResourceDictionary Production;
        [SerializeField] private int             StockpileCoefficient;
        [SerializeField] private float           IntervalOfProductionInSeconds;

        private BlobSitePermissionProfile ProductionProfile;
        private BlobSitePermissionProfile ExtractionProfile;

        private float ProductionTimer = 0f;

        #endregion

        #region Unity event methods

        private void Start() {
            if(Production != null) {
                RefreshProfiles();
            }

            if(Location != null) {
                ExtractionProfile.InsertProfileIntoBlobSite(Location.BlobSite);
            }
        }

        private void OnDestroy() {
            ParentFactory.UnsubscribeGenerator(this);
        }

        private void OnValidate() {
            if(Production != null) {
                RefreshProfiles();
            }
        }

        #endregion

        #region instance methods

        public void TickProduction(float secondsPassed) {
            ProductionTimer += secondsPassed;
            var blobSite = Location.BlobSite;

            ProductionProfile.InsertProfileIntoBlobSite(blobSite);

            while(ProductionTimer >= IntervalOfProductionInSeconds) {
                ProductionTimer -= IntervalOfProductionInSeconds;
                foreach(var resourceType in Production) {
                    for(int i = 0; i < Production[resourceType]; ++i) {
                        if(blobSite.CanPlaceBlobOfTypeInto(resourceType)) {
                            blobSite.PlaceBlobInto(BlobFactory.BuildBlob(resourceType, blobSite.transform.position));
                        }else {
                            break;
                        }
                    }
                }
            }

            ExtractionProfile.InsertProfileIntoBlobSite(blobSite);
        }

        private void RefreshProfiles() {
            ProductionProfile = new BlobSitePermissionProfile();
            ExtractionProfile = new BlobSitePermissionProfile();

            int totalCapacity = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                ExtractionProfile.SetExtractionPermission(resourceType, true);
                ExtractionProfile.SetPlacementPermission(resourceType, false);

                ProductionProfile.SetPlacementPermission(resourceType, true);
                ProductionProfile.SetExtractionPermission(resourceType, false);
                ProductionProfile.SetCapacity(resourceType, Production[resourceType] * StockpileCoefficient);
                totalCapacity += Production[resourceType] * StockpileCoefficient;
            }

            ProductionProfile.SetTotalCapacity(totalCapacity);
        }

        #endregion

    }

}
