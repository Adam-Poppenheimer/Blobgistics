using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

using Assets.UI.Highways;

using Assets.ConstructionZones;
using Assets.UI.ConstructionZones;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class MapNodeStandardEventReceiver : TargetedEventReceiverBase<MapNodeUISummary> {

        #region instance fields and properties

        public BlobHighwayGhostBase HighwayGhost {
            get { return _highwayGhost; }
            set { _highwayGhost = value; }
        }
        [SerializeField] private BlobHighwayGhostBase _highwayGhost;

        public HighwayControlBase HighwayControl {
            get { return _highwayControl; }
            set { _highwayControl = value; }
        }
        [SerializeField] private HighwayControlBase _highwayControl;

        public ConstructionZoneControlBase ConstructionZoneControl {
            get { return _constructionZoneControl; }
            set { _constructionZoneControl = value; }
        }
        [SerializeField] private ConstructionZoneControlBase _constructionZoneControl;

        public ConstructionPanelBase ConstructionPanel {
            get { return _constructionPanel; }
            set {
                if(_constructionPanel != null) {
                    _constructionPanel.DeactivationRequested -= ConstructionPanel_DeactivationRequested;
                    _constructionPanel.ConstructionRequested -= ConstructionPanel_ConstructionRequested;
                }
                _constructionPanel = value;
                if(_constructionPanel != null) {
                    _constructionPanel.DeactivationRequested += ConstructionPanel_DeactivationRequested;
                    _constructionPanel.ConstructionRequested += ConstructionPanel_ConstructionRequested;
                }
            }
        }
        [SerializeField] private ConstructionPanelBase _constructionPanel;

        [SerializeField] private AudioSource HighwayDrawingAudio;
        [SerializeField] private float HighwayDrawingVolumeWhileMoving;
        [SerializeField] private float AudioDeltaPerSecond;

        private bool ReceivedDragEventLastFrame = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ConstructionPanel != null) {
                ConstructionPanel.DeactivationRequested -= ConstructionPanel_DeactivationRequested;
                ConstructionPanel.ConstructionRequested -= ConstructionPanel_ConstructionRequested;

                ConstructionPanel.DeactivationRequested += ConstructionPanel_DeactivationRequested;
                ConstructionPanel.ConstructionRequested += ConstructionPanel_ConstructionRequested;
            }
        }

        private void Update() {
            if(ReceivedDragEventLastFrame) {
                ReceivedDragEventLastFrame = false;
                HighwayDrawingAudio.volume = Math.Min(
                    HighwayDrawingVolumeWhileMoving,
                    HighwayDrawingAudio.volume + AudioDeltaPerSecond * Time.deltaTime
                );
            }else {
                HighwayDrawingAudio.volume = Math.Max(
                    0,
                    HighwayDrawingAudio.volume - AudioDeltaPerSecond * Time.deltaTime
                );
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<MapNodeUISummary>

        public override void PushBeginDragEvent(MapNodeUISummary source, PointerEventData eventData) {
            HighwayGhost.Clear();
            HighwayGhost.FirstEndpoint = source;
            HighwayGhost.Activate();
            HighwayDrawingAudio.volume = HighwayDrawingVolumeWhileMoving;
            HighwayDrawingAudio.Play();
        }

        public override void PushDragEvent(MapNodeUISummary source, PointerEventData eventData) {
            HighwayGhost.UpdateWithEventData(eventData);
            ReceivedDragEventLastFrame = true;
        }

        public override void PushEndDragEvent(MapNodeUISummary source, PointerEventData eventData) {
            var firstEndpoint = HighwayGhost.FirstEndpoint;
            var secondEndpoint = HighwayGhost.SecondEndpoint;

            if( firstEndpoint != null && secondEndpoint != null &&
                HighwayControl.CanConnectNodesWithHighway(firstEndpoint.ID, secondEndpoint.ID)
            ){
                HighwayControl.ConnectNodesWithHighway(HighwayGhost.FirstEndpoint.ID, HighwayGhost.SecondEndpoint.ID);
            }
            HighwayGhost.Clear();
            HighwayGhost.Deactivate();
            HighwayDrawingAudio.Stop();
        }

        public override void PushPointerClickEvent(MapNodeUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(MapNodeUISummary source, PointerEventData eventData) {
            if(HighwayGhost.IsActivated) {
                HighwayGhost.SecondEndpoint = source;
                HighwayGhost.GhostIsBuildable = HighwayControl.CanConnectNodesWithHighway(HighwayGhost.FirstEndpoint.ID,
                    HighwayGhost.SecondEndpoint.ID);
            }
        }

        public override void PushPointerExitEvent(MapNodeUISummary source, PointerEventData eventData) {
            if(HighwayGhost.IsActivated) {
                HighwayGhost.SecondEndpoint = null;
                HighwayGhost.GhostIsBuildable = false;
            }
        }

        public override void PushSelectEvent(MapNodeUISummary source, BaseEventData eventData) {
            ConstructionPanel.LocationToConstruct = source;
            ConstructionPanel.SetPermittedProjects(ConstructionZoneControl.GetAllPermittedConstructionZoneProjectsOnNode(source.ID));
            ConstructionPanel.Activate();
        }

        public override void PushUpdateSelectedEvent(MapNodeUISummary source, BaseEventData eventData) { }

        public override void PushDeselectEvent(MapNodeUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(MapNodeUISummary source) { }

        #endregion

        private void ConstructionPanel_ConstructionRequested(object sender, StringEventArgs e) {
            if(ConstructionZoneControl.CanCreateConstructionZoneOnNode(ConstructionPanel.LocationToConstruct.ID, e.Value)) {
                ConstructionZoneControl.CreateConstructionZoneOnNode(ConstructionPanel.LocationToConstruct.ID, e.Value);
            }
            ConstructionPanel.Deactivate();
        }

        private void ConstructionPanel_DeactivationRequested(object sender, EventArgs e) {
            ConstructionPanel.Deactivate();
        }

        #endregion
        
    }

}
