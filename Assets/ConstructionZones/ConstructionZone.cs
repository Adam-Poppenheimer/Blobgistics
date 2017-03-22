using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

namespace Assets.ConstructionZones {

    public class ConstructionZone : ConstructionZoneBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler {

        #region instance fields and properties

        #region from ConstructionZoneBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override ConstructionProjectBase CurrentProject {
            get { return _currentProject; }

            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                }else {
                    _currentProject = value;

                    var blobSite = Location.BlobSite;
                    blobSite.ClearPermissionsAndCapacity();
                    blobSite.ClearContents();

                    int runningCapacityTotal = 0;
                    foreach(var resourceType in _currentProject.Cost) {
                        if(_currentProject.Cost[resourceType] > 0) {
                            blobSite.SetPlacementPermissionForResourceType(resourceType, true);
                            blobSite.SetExtractionPermissionForResourceType(resourceType, false);
                            blobSite.SetCapacityForResourceType(resourceType, _currentProject.Cost[resourceType]);
                            runningCapacityTotal += _currentProject.Cost[resourceType];
                        }
                    }
                    blobSite.TotalCapacity = runningCapacityTotal;

                    blobSite.BlobPlacedInto += BlobSite_BlobPlacedInto;
                }
            }
        }

        [SerializeField] private ConstructionProjectBase _currentProject;

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        [SerializeField, HideInInspector] private MapNodeBase _location;

        #endregion

        public ConstructionZoneFactoryBase ParentFactory {
            get {
                if(_parentFactory == null) {
                    throw new InvalidOperationException("ParentFactory is uninitialized");
                } else {
                    return _parentFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _parentFactory = value;
                }
            }
        }
        [SerializeField, HideInInspector] private ConstructionZoneFactoryBase _parentFactory;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField, HideInInspector] private UIControlBase _uiControl;

        #endregion

        #region instance methods

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ConstructionZoneUISummary(this), eventData);
        }

        #endregion

        #region from ConstructionZoneBase

        public override ResourceSummary GetResourcesNeededToFinish() {
            var countDict = new Dictionary<ResourceType, int>();
            var blobSite = Location.BlobSite;

            foreach(var resourceType in CurrentProject.Cost) {
                countDict[resourceType] = CurrentProject.Cost[resourceType] - blobSite.GetCountOfContentsOfType(resourceType);
            }

            return ResourceSummary.BuildResourceSummary(gameObject, countDict);
        }

        #endregion

        private void BlobSite_BlobPlacedInto(object sender, BlobEventArgs e) {
            if(CurrentProject.Cost.IsContainedWithinBlobSite(Location.BlobSite)) {
                Location.BlobSite.ClearContents();
                Location.BlobSite.ClearPermissionsAndCapacity();
                Location.BlobSite.TotalCapacity = 0;
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
                if(CurrentProject.BuildAction != null) {
                    CurrentProject.BuildAction(Location);
                }
                ParentFactory.DestroyConstructionZone(this);
            }
        }

        #endregion
                
    }

}
