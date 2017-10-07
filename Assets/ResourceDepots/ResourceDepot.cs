using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.ResourceDepots {

    /// <summary>
    /// The standard implementation of ResourceDepotBase. Implements a gameplay element
    /// that facilitates the transfer and storage of resources.
    /// </summary>
    [ExecuteInEditMode]
    public class ResourceDepot : ResourceDepotBase, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from ResourceDepotBase

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
        public override ResourceDepotProfile Profile {
            get { return _profile; }
            set {
                _profile = value;
                RefreshBlobSite();
            }
        }
        [SerializeField] private ResourceDepotProfile _profile;

        #endregion

        /// <summary>
        /// The UIControl this depot should send user input events to.
        /// </summary>
        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <summary>
        /// The parent factory of this depot.
        /// </summary>
        public ResourceDepotFactoryBase ParentFactory {
            get { return _parentFactory; }
            set { _parentFactory = value; }
        }
        [SerializeField] private ResourceDepotFactoryBase _parentFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            RefreshBlobSite();
        }

        private void OnDestroy() {
            if(ParentFactory != null) {
                ParentFactory.UnsubscribeDepot(this);
            }
            if(UIControl != null) {
                UIControl.PushObjectDestroyedEvent(new ResourceDepotUISummary(this));
            }
        }

        #endregion

        #region EventSystem interface implementations

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ResourceDepotUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ResourceDepotUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ResourceDepotUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new ResourceDepotUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new ResourceDepotUISummary(this), eventData);
        }

        #endregion

        #region from ResourceDepotBase

        /// <inheritdoc/>
        public override void Clear() {
            Location.BlobSite.ClearContents();
        }

        #endregion

        /// <summary>
        /// Sets the permissions and capacities of Location's
        /// BlobSite to enact the properties of this ResourceDepot.
        /// </summary>
        public void RefreshBlobSite() {
            if(Location != null) {
                var blobSite = Location.BlobSite;
                int intendedTotalCapacity = 0;

                blobSite.ClearPermissionsAndCapacity();

                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    blobSite.SetPlacementPermissionForResourceType(resourceType, true);
                    blobSite.SetExtractionPermissionForResourceType(resourceType, true);
                    blobSite.SetCapacityForResourceType(resourceType, _profile.PerResourceCapacity);
                    intendedTotalCapacity += _profile.PerResourceCapacity;
                }

                blobSite.TotalCapacity = intendedTotalCapacity;
            }
        }

        #endregion

    }

}
