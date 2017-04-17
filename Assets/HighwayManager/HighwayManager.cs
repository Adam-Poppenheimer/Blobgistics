using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    public class HighwayManager : HighwayManagerBase {

        #region instance fields and properties

        #region from HighwayManagerBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        [SerializeField] private MapNodeBase _location;

        public override uint ManagementRadius {
            get { return _managementRadius; }
        }
        public void SetManagementRadius(uint value) {
            _managementRadius = value;
        }
        [SerializeField] private uint _managementRadius;

        public override int NeedStockpileCoefficient {
            get { return _needStockpileCoefficient; }
        }
        public void SetNeedStockpileCoefficient(int value) {
            _needStockpileCoefficient = value;
        }
        [SerializeField] private int _needStockpileCoefficient;

        public override float SecondsToPerformConsumption {
            get { return _secondsToPerformConsumption; }
        }
        public void SetSecondsToPerformConsumption(float value) {
            _secondsToPerformConsumption = value;
        }
        [SerializeField] private float _secondsToPerformConsumption;

        public override float LastCalculatedEfficiency {
            get { return lastCalculatedEfficiency; }
        }
        private float lastCalculatedEfficiency = 1f;

        #endregion

        public HighwayManagerFactoryBase ParentFactory { get; set; }

        private float ConsumptionTimer = 0f;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            var blobSite = Location.BlobSite;
            blobSite.ClearPermissionsAndCapacity();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                blobSite.SetPlacementPermissionForResourceType(resourceType, true);
            }
        }

        #endregion

        #region from Object

        public override string ToString() {
            return name;
        }

        #endregion

        #region from HighwayManagerBase

        public override void TickConsumption(float secondsPassed) {
            ConsumptionTimer += secondsPassed;
            while(ConsumptionTimer >= SecondsToPerformConsumption) {
                PerformConsumptionOnce();
                ConsumptionTimer -= SecondsToPerformConsumption;
            }
        }

        #endregion

        private void PerformConsumptionOnce() {
            int totalNeedCount = 0;
            int runningConsumptionTotal = 0;
            var resourcesNeededByType = new Dictionary<ResourceType, int>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                resourcesNeededByType[resourceType] = 0;
            }

            var highwaysBeingManaged = ParentFactory.GetHighwaysServedByManager(this);
            PrepareBlobSiteForConsumption(highwaysBeingManaged);

            foreach(var highway in highwaysBeingManaged) {
                foreach(var resourceType in highway.Profile.Upkeep) {
                    int upkeepOfResource = highway.Profile.Upkeep[resourceType];
                    resourcesNeededByType[resourceType] += upkeepOfResource;
                    totalNeedCount += upkeepOfResource;
                }
            }

            var blobSite = Location.BlobSite;
            foreach(var resourceType in resourcesNeededByType.Keys) {
                for(int i = 0; i < resourcesNeededByType[resourceType]; ++i) {
                    if(blobSite.CanExtractBlobOfType(resourceType)) {
                        blobSite.ExtractBlobOfType(resourceType);
                        ++runningConsumptionTotal;
                    }else {
                        break;
                    }
                }
            }

            lastCalculatedEfficiency = ((float)runningConsumptionTotal) / ((float)totalNeedCount);

            foreach(var highway in highwaysBeingManaged) {
                highway.Efficiency = lastCalculatedEfficiency;
            }

            RevertBlobSiteToNormal(highwaysBeingManaged);
        }

        private void PrepareBlobSiteForConsumption(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;
            foreach(var highway in highwaysBeingManaged) {
                foreach(var resourceType in highway.Profile.Upkeep) {
                    blobSite.SetExtractionPermissionForResourceType(resourceType, true);
                }
            }
        }

        private void RevertBlobSiteToNormal(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;
            var newCapacityDict = new Dictionary<ResourceType, int>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                newCapacityDict[resourceType] = 0;
            }

            foreach(var highway in highwaysBeingManaged) {
                var highwayUpkeep = highway.Profile.Upkeep;
                foreach(var resourceType in highwayUpkeep) {
                    newCapacityDict[resourceType] += highwayUpkeep[resourceType];
                }
            }

            int totalCapacity = 0;
            foreach(var capacityPair in newCapacityDict) {
                int valueStockpiled = capacityPair.Value * NeedStockpileCoefficient;
                blobSite.SetCapacityForResourceType(capacityPair.Key, valueStockpiled);
                totalCapacity += valueStockpiled;
            }
            blobSite.TotalCapacity = totalCapacity;
        }

        #endregion

    }

}
