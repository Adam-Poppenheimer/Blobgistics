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

    [ExecuteInEditMode]
    public class ResourceDepot : ResourceDepotBase, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from ResourceDepotBase

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

        public override ResourceDepotProfile Profile {
            get { return _profile; }
            set { _profile = value; }
        }
        [SerializeField] private ResourceDepotProfile _profile;

        #endregion

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

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

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ResourceDepotUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new ResourceDepotUISummary(this), eventData);
        }

        #endregion

        #region from ResourceDepotBase

        public override void Clear() {
            Location.BlobSite.ClearContents();
        }

        #endregion

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
