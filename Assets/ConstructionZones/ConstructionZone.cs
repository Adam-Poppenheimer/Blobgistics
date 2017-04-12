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

    public class ConstructionZone : ConstructionZoneBase, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler {

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
                    _currentProject.SetSiteForProject(Location.BlobSite);
                }
            }
        }

        [SerializeField] private ConstructionProjectBase _currentProject;

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            if(_location != null) {
                _location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
            }
            _location = value;
            if(_location != null) {
                _location.BlobSite.BlobPlacedInto += BlobSite_BlobPlacedInto;
            }
        }
        [SerializeField] private MapNodeBase _location;

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
        [SerializeField] private ConstructionZoneFactoryBase _parentFactory;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnDestroy() {
            if(Location != null) {
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
            }
            ParentFactory.UnsubsribeConstructionZone(this);
        }

        #endregion

        #region EventSystem interface implementations

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ConstructionZoneUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ConstructionZoneUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new ConstructionZoneUISummary(this), eventData);
        }

        #endregion

        private void BlobSite_BlobPlacedInto(object sender, BlobEventArgs e) {
            if(CurrentProject.BlobSiteContainsNecessaryResources(Location.BlobSite)) {
                Location.BlobSite.ClearContents();
                Location.BlobSite.ClearPermissionsAndCapacity();
                Location.BlobSite.TotalCapacity = 0;
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
                CurrentProject.ExecuteBuild(Location);
                ParentFactory.DestroyConstructionZone(this);
            }
        }

        #endregion

    }

}
