using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Highways;
using Assets.UI.Highways;
using Assets.Util;

namespace Assets.Core {

    /// <summary>
    /// The standard event receiver for all events propagating from highways and all commands made to them.
    /// </summary>
    /// <remarks>
    /// The main purpose of this class is to connect HighwaySummaryDisplay to the UI and to the simulation.
    /// It could ostensibly be used for other purposes as well. It's not clear if its relatively simple
    /// activity justifies its existence. 
    /// </remarks>
    public class BlobHighwayStandardEventReceiver : TargetedEventReceiverBase<BlobHighwayUISummary> {

        #region instance fields and properties

        /// <summary>
        /// The main display that this class is transferring information for.
        /// </summary>
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

        /// <summary>
        /// The facade into the simulation that this class needs to access in order to function.
        /// </summary>
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

        /// <inheritdoc/>
        public override void PushBeginDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushEndDragEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerClickEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerEnterEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerExitEvent(BlobHighwayUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushSelectEvent(BlobHighwayUISummary source, BaseEventData eventData) {
            if(HighwaySummaryDisplay != null) {
                HighwaySummaryDisplay.CurrentSummary = source;
                HighwaySummaryDisplay.Activate();
            }
        }

        /// <inheritdoc/>
        public override void PushUpdateSelectedEvent(BlobHighwayUISummary source, BaseEventData eventData) {
            if(source == HighwaySummaryDisplay.CurrentSummary) {
                HighwaySummaryDisplay.Deactivate();
            }
        }

        /// <inheritdoc/>
        public override void PushDeselectEvent(BlobHighwayUISummary source, BaseEventData eventData) { }

        /// <inheritdoc/>
        public override void PushObjectDestroyedEvent(BlobHighwayUISummary source) { }

        /// <inheritdoc/>
        public override bool TryCloseAllOpenDisplays() {
            if(HighwaySummaryDisplay.gameObject.activeInHierarchy) {
                HighwaySummaryDisplay.Deactivate();
                return true;
            }
            return false;
        }

        #endregion

        private void HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            HighwayControl.SetHighwayPullingPermissionOnFirstEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        private void HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            HighwayControl.SetHighwayPullingPermissionOnSecondEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        private void HighwayDisplay_ResourceRequestedForUpkeep(object sender, UpkeepRequestEventArgs e) {
            HighwayControl.SetHighwayUpkeepRequest(HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsBeingRequested);
        }

        #endregion

    }

}
