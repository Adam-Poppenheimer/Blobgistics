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

    /// <summary>
    /// The standard implementation for HighwayManagerBase. This class supports a gameplay element that can
    /// provide increased speed and efficiency to highways at the cost of resources.
    /// </summary>
    public class HighwayManager : HighwayManagerBase, IPointerClickHandler, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from HighwayManagerBase

        /// <inheritdoc/>
        public override int ID {
            get { return GetInstanceID(); }
        }

        /// <inheritdoc/>
        public override MapNodeBase Location {
            get { return _location; }
        }
        /// <summary>
        /// The externalized Set method for Location.
        /// </summary>
        /// <param name="value">The new value of Location</param>
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        [SerializeField] private MapNodeBase _location;

        /// <inheritdoc/>
        public override ReadOnlyDictionary<ResourceType, int> LastCalculatedUpkeep {
            get { return new ReadOnlyDictionary<ResourceType, int>(lastCalculatedUpkeep); }
        }
        private Dictionary<ResourceType, int> lastCalculatedUpkeep =
            new Dictionary<ResourceType, int>();

        #endregion

        /// <summary>
        /// A POD object containing a number of configuration and dependency variables.
        /// </summary>
        public HighwayManagerPrivateDataBase PrivateData {
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField] private HighwayManagerPrivateDataBase _privateData;

        private float ConsumptionTimer = 0f;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            var blobSite = Location.BlobSite;
            blobSite.ClearPermissionsAndCapacity();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                blobSite.SetPlacementPermissionForResourceType(resourceType, true);
            }
        }

        private void OnDestroy() {
            if(PrivateData != null && PrivateData.ParentFactory != null) {
                PrivateData.ParentFactory.UnsubscribeHighwayManager(this);
            }
        }

        #endregion

        #region Unity EventSystem interfaces

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerClickEvent(new HighwayManagerUISummary(this), eventData);
            if(EventSystem.current != null) {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            PrivateData.UIControl.PushSelectEvent(new HighwayManagerUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDeselect(BaseEventData eventData) {
            PrivateData.UIControl.PushDeselectEvent(new HighwayManagerUISummary(this), eventData);
        }

        #endregion

        #region from Object

        /// <inheritdoc/>
        public override string ToString() {
            return name;
        }

        #endregion

        #region from HighwayManagerBase

        /// <inheritdoc/>
        public override void Tick(float secondsPassed) {
            ConsumptionTimer += secondsPassed;
            while(ConsumptionTimer >= PrivateData.SecondsToPerformConsumption) {
                PerformConsumptionOnce();
                ConsumptionTimer -= PrivateData.SecondsToPerformConsumption;
            }
        }

        #endregion

        /*
         * This method goes through all highways being managed by this manager and attempts to satisfy their upkeep requests.
         * If it manages to satisfy any of a given highway's requests, then it modifies the highway's efficiency accordingly.
         * Otherwise, efficiency is reset to its default value of 1.
         */
        private void PerformConsumptionOnce() {
            var highwaysBeingManaged = new List<BlobHighwayBase>(PrivateData.ParentFactory.GetHighwaysServedByManager(this));

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

        //BlobSite has some limitations to it. Setting extraction permissions for certain
        //resources to false makes it impossible to extract such resources for any reason,
        //including consumption. This means that, in order to enable consumption of the
        //resources highway manager is requesting, the manager needs to temporarily change the
        //extraction permissions of Location's BlobSite. It might be wise to refactor
        //BlobSite, adding methods for consumption-based extraction that bypass normal
        //permissions.

        //This is the state the BlobSite must occupy when it's actively consuming resources.
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

        //This is the state the BlobSite must occupy at all other times.
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
