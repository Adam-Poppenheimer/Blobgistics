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

namespace Assets.Depots {

    public class ResourceDepot : ResourceDepotBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler {

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
        [SerializeField, HideInInspector] private MapNodeBase _location;

        public override ResourceDepotProfile Profile {
            get { return _profile; }
            set {
                _profile = value;
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
        [SerializeField, HideInInspector] private ResourceDepotProfile _profile;

        #endregion

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        #endregion

        #region instance methods

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ResourceDepotUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ResourceDepotUISummary(this), eventData);
        }

        #endregion

        #region from ResourceDepotBase

        public override void Clear() {
            Location.BlobSite.ClearContents();
        }

        #endregion

        #endregion

    }

}
