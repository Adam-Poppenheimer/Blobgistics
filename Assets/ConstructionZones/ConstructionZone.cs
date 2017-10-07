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

    /// <summary>
    /// The standard implementation for ConstructionZoneBase. It enable the creation of societies,
    /// resource depots, highway managers, and the addition and removal of forests by the player.
    /// </summary>
    public class ConstructionZone : ConstructionZoneBase, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler {

        #region instance fields and properties

        #region from ConstructionZoneBase

        /// <inheritdoc/>
        public override int ID {
            get { return GetInstanceID(); }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override MapNodeBase Location {
            get { return _location; }
        }
        /// <summary>
        /// Externalized Set method for Location.
        /// </summary>
        /// <param name="value">The new value of Location</param>
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

        /// <inheritdoc/>
        public override bool ProjectHasBeenCompleted {
            get { return projectHasBeenCompleted; }
        }
        private bool projectHasBeenCompleted = false;

        #endregion

        /// <summary>
        /// The factory that is supposed to manage this ConstructionZone.
        /// </summary>
        public ConstructionZoneFactoryBase ParentFactory {
            get { return _parentFactory; }
            set { _parentFactory = value; }
        }
        [SerializeField] private ConstructionZoneFactoryBase _parentFactory;

        /// <summary>
        /// The UIControlBase that this ConstructionZone should send player input events to.
        /// </summary>
        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <summary>
        /// The audio to play when the project is completed
        /// </summary>
        public AudioSource ProjectCompletionAudio {
            get { return _projectCompletionAudio; }
            set { _projectCompletionAudio = value; }
        }
        [SerializeField] private AudioSource _projectCompletionAudio;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnDestroy() {
            if(Location != null) {
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
            }
            if(ParentFactory != null) {
                ParentFactory.UnsubsribeConstructionZone(this);
            }
            if(UIControl != null) {
                UIControl.PushObjectDestroyedEvent(new ConstructionZoneUISummary(this));
            }
        }

        #endregion

        #region EventSystem interface implementations

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new ConstructionZoneUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new ConstructionZoneUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new ConstructionZoneUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new ConstructionZoneUISummary(this), eventData);
        }

        #endregion

        //Every time a blob is placed in Location's BlobSite, the zone must check to see if the construction zone
        //has collected the resources necessary to complete its project. If it has, it strips everything out
        //of Location's BlobSite and calls the construction project itself to do the real work of completing
        //the project.
        private void BlobSite_BlobPlacedInto(object sender, BlobEventArgs e) {
            if(CurrentProject.BlobSiteContainsNecessaryResources(Location.BlobSite)) {
                Location.BlobSite.ClearContents();
                Location.BlobSite.ClearPermissionsAndCapacity();
                Location.BlobSite.TotalCapacity = 0;
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
                CurrentProject.ExecuteBuild(Location);
                projectHasBeenCompleted = true;

                if(ProjectCompletionAudio != null && !ProjectCompletionAudio.isPlaying) {
                    ProjectCompletionAudio.Play();
                }
                
                ParentFactory.DestroyConstructionZone(this);
            }
        }

        #endregion

    }

}
