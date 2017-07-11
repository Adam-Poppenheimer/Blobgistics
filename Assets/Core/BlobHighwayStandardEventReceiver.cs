using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Highways;
using Assets.UI.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class BlobHighwayStandardEventReceiver : TargetedEventReceiverBase<BlobHighwayUISummary> {

        #region instance fields and properties

        public BlobHighwaySummaryDisplayBase HighwaySummaryDisplay {
            get { return _highwayDisplay; }
            set {
                if(_highwayDisplay != null) {
                    _highwayDisplay.FirstEndpointResourcePermissionChanged  -= HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                    _highwayDisplay.SecondEndpointResourcePermissionChanged -= HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                    _highwayDisplay.ResourceRequestedForUpkeep              -= HighwayDisplay_ResourceRequestedForUpkeep;
                        
                }
                _highwayDisplay = value;
                if(_highwayDisplay != null) {
                    _highwayDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                    _highwayDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                    _highwayDisplay.ResourceRequestedForUpkeep              += HighwayDisplay_ResourceRequestedForUpkeep;
                }
            }
        }
        [SerializeField] private BlobHighwaySummaryDisplayBase _highwayDisplay;

        public HighwayControlBase HighwayControl {
            get { return _highwayControl; }
            set { _highwayControl = value; }
        }
        [SerializeField] private HighwayControlBase _highwayControl;  

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(HighwaySummaryDisplay != null) {
                HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged  -= HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged -= HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.ResourceRequestedForUpkeep              -= HighwayDisplay_ResourceRequestedForUpkeep;

                HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.ResourceRequestedForUpkeep              += HighwayDisplay_ResourceRequestedForUpkeep;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<BlobHighwayUISummary>

        public override void PushBeginDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushEndDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushPointerClickEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushPointerExitEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        public override void PushSelectEvent(BlobHighwayUISummary source, BaseEventData eventData) {
            if(HighwaySummaryDisplay != null) {
                HighwaySummaryDisplay.CurrentSummary = source;
                HighwaySummaryDisplay.Activate();
            }
        }

        public override void PushUpdateSelectedEvent(BlobHighwayUISummary source, BaseEventData eventData) {
            if(source == HighwaySummaryDisplay.CurrentSummary) {
                HighwaySummaryDisplay.Deactivate();
            }
        }

        public override void PushDeselectEvent(BlobHighwayUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(BlobHighwayUISummary source) { }

        #endregion

        private void HighwaySummaryDisplay_PriorityChanged(object sender, IntEventArgs e) {
            HighwayControl.SetHighwayPriority(HighwaySummaryDisplay.CurrentSummary.ID, e.Value);
        }

        private void HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            HighwayControl.SetHighwayPullingPermissionOnFirstEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        private void HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            HighwayControl.SetHighwayPullingPermissionOnSecondEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        private void HighwayDisplay_ResourceRequestedForUpkeep(object sender, UpkeepRequestEventArgs e) {
            HighwayControl.SetHighwayUpkeepRequest(HighwaySummaryDisplay.CurrentSummary.ID, e.Type, e.IsBeingRequested);
        }

        #endregion

    }

}
