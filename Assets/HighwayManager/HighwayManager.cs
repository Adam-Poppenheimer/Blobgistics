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
            var highwaysBeingManaged = new List<BlobHighwayBase>(PrivateData.ParentFactory.GetHighwaysServedByManager(this));
            highwaysBeingManaged.Sort((x, y) => x.Priority - y.Priority);

            RecalculateUpkeep(highwaysBeingManaged);

            PrepareBlobSiteForConsumption(highwaysBeingManaged);

            var blobSite = Location.BlobSite;

            foreach(var highway in highwaysBeingManaged) {
                highway.Efficiency = 1f;

                foreach(var resourceType in lastCalculatedUpkeep.Keys) {
                    if(highway.GetUpkeepRequestedForResource(resourceType) && blobSite.CanExtractBlobOfType(resourceType)) {
                        var blobToDestroy = blobSite.ExtractBlobOfType(resourceType);
                        PrivateData.BlobFactory.DestroyBlob(blobToDestroy);
                        highway.Efficiency += PrivateData.EfficiencyGainFromResource[resourceType];
                    }
                }
            }

            RevertBlobSiteToNormal(highwaysBeingManaged);
        }

        private void RecalculateUpkeep(List<BlobHighwayBase> highwaysBeingManaged) {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                lastCalculatedUpkeep[resourceType] = 0;
                foreach(var highway in highwaysBeingManaged) {
                    if(highway.GetUpkeepRequestedForResource(resourceType)) {
                        ++lastCalculatedUpkeep[resourceType];
                    }
                }
            }
        }

        private void PrepareBlobSiteForConsumption(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;
            blobSite.ClearPermissionsAndCapacity();

            foreach(var resourceType in lastCalculatedUpkeep.Keys) {
                int upkeepForResource = lastCalculatedUpkeep[resourceType];
                blobSite.SetExtractionPermissionForResourceType(resourceType, true);
                blobSite.SetCapacityForResourceType(resourceType, upkeepForResource);
                blobSite.TotalCapacity += upkeepForResource;
            }
        }

        private void RevertBlobSiteToNormal(IEnumerable<BlobHighwayBase> highwaysBeingManaged) {
            var blobSite = Location.BlobSite;
            blobSite.ClearPermissionsAndCapacity();

            foreach(var resourceType in lastCalculatedUpkeep.Keys) {
                int upkeepForResource = lastCalculatedUpkeep[resourceType];
                blobSite.SetPlacementPermissionForResourceType(resourceType, true);
                blobSite.SetCapacityForResourceType(resourceType, upkeepForResource);
                blobSite.TotalCapacity += upkeepForResource;
            }
        }
        
        #endregion

    }

}
