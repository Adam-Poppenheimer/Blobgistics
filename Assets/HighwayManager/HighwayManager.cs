using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Highways;
using Assets.Map;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    public class HighwayManager : HighwayManagerBase, IPointerClickHandler, ISelectHandler, IDeselectHandler {

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

        public HighwayManagerPrivateDataBase PrivateData {
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField] private HighwayManagerPrivateDataBase _privateData;

        public override ReadOnlyDictionary<ResourceType, int> LastCalculatedUpkeep {
            get { return new ReadOnlyDictionary<ResourceType, int>(lastCalculatedUpkeep); }
        }
        private Dictionary<ResourceType, int> lastCalculatedUpkeep =
            new Dictionary<ResourceType, int>();

        #endregion

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

        #region Unity EventSystem interfaces

        public void OnPointerClick(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerClickEvent(new HighwayManagerUISummary(this), eventData);
            if(EventSystem.current != null) {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }

        public void OnSelect(BaseEventData eventData) {
            PrivateData.UIControl.PushSelectEvent(new HighwayManagerUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            PrivateData.UIControl.PushDeselectEvent(new HighwayManagerUISummary(this), eventData);
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
            while(ConsumptionTimer >= PrivateData.SecondsToPerformConsumption) {
                PerformConsumptionOnce();
                ConsumptionTimer -= PrivateData.SecondsToPerformConsumption;
            }
        }

        #endregion

        private void PerformConsumptionOnce() {
            int totalNeedCount = 0;
            int runningConsumptionTotal = 0;
            lastCalculatedUpkeep.Clear();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                lastCalculatedUpkeep[resourceType] = 0;
            }

            var highwaysBeingManaged = new List<BlobHighwayBase>(PrivateData.ParentFactory.GetHighwaysServedByManager(this));
            highwaysBeingManaged.Sort((x, y) => x.Priority - y.Priority);

            PrepareBlobSiteForConsumption(highwaysBeingManaged);

            var blobSite = Location.BlobSite;

            foreach(var highway in highwaysBeingManaged) {
                highway.Efficiency = 1f;

                if(highway.IsRequestingFood && blobSite.CanExtractBlobOfType(ResourceType.Food)) {
                    PrivateData.BlobFactory.DestroyBlob(blobSite.ExtractBlobOfType(ResourceType.Food));
                    ++lastCalculatedUpkeep[ResourceType.Food];
                    highway.Efficiency += PrivateData.EfficiencyGainFromFood;
                }

                if(highway.IsRequestingYellow && blobSite.CanExtractBlobOfType(ResourceType.Yellow)) {
                    PrivateData.BlobFactory.DestroyBlob(blobSite.ExtractBlobOfType(ResourceType.Yellow));
                    ++lastCalculatedUpkeep[ResourceType.Yellow];
                    highway.Efficiency += PrivateData.EfficiencyGainFromYellow;
                }

                if(highway.IsRequestingWhite && blobSite.CanExtractBlobOfType(ResourceType.White)) {
                    PrivateData.BlobFactory.DestroyBlob(blobSite.ExtractBlobOfType(ResourceType.White));
                    ++lastCalculatedUpkeep[ResourceType.White];
                    highway.Efficiency += PrivateData.EfficiencyGainFromWhite;
                }

                if(highway.IsRequestingBlue && blobSite.CanExtractBlobOfType(ResourceType.Blue)) {
                    PrivateData.BlobFactory.DestroyBlob(blobSite.ExtractBlobOfType(ResourceType.Blue));
                    ++lastCalculatedUpkeep[ResourceType.Blue];
                    highway.Efficiency += PrivateData.EfficiencyGainFromBlue;
                }
            }

            RevertBlobSiteToNormal(highwaysBeingManaged);
        }

        private void PrepareBlobSiteForConsumption(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;

            foreach(var highway in highwaysBeingManaged) {
                if(highway.IsRequestingFood)   { blobSite.SetExtractionPermissionForResourceType(ResourceType.Food,   true); }
                if(highway.IsRequestingYellow) { blobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true); }
                if(highway.IsRequestingWhite)  { blobSite.SetExtractionPermissionForResourceType(ResourceType.White,  true); }
                if(highway.IsRequestingBlue)   { blobSite.SetExtractionPermissionForResourceType(ResourceType.Blue,   true); }
            }
        }

        private void RevertBlobSiteToNormal(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;
            var newCapacityDict = new Dictionary<ResourceType, int>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                newCapacityDict[resourceType] = 0;
            }

            foreach(var highway in highwaysBeingManaged) {
                if(highway.IsRequestingFood  ) { ++newCapacityDict[ResourceType.Food  ]; }
                if(highway.IsRequestingYellow) { ++newCapacityDict[ResourceType.Yellow]; }
                if(highway.IsRequestingWhite ) { ++newCapacityDict[ResourceType.White ]; }
                if(highway.IsRequestingBlue  ) { ++newCapacityDict[ResourceType.Blue  ]; }
            }

            int totalCapacity = 0;
            foreach(var capacityPair in newCapacityDict) {
                int valueStockpiled = capacityPair.Value * PrivateData.NeedStockpileCoefficient;
                blobSite.SetCapacityForResourceType(capacityPair.Key, valueStockpiled);
                totalCapacity += valueStockpiled;
            }
            blobSite.TotalCapacity = totalCapacity;
        }
        
        #endregion

    }

}
