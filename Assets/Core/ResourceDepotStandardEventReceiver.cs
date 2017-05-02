using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.ResourceDepots;
using Assets.UI.ResourceDepots;

namespace Assets.Core {

    public class ResourceDepotStandardEventReceiver : TargetedEventReceiverBase<ResourceDepotUISummary> {

        #region instance fields and properties

        public ResourceDepotControlBase ResourceDepotControl {
            get { return _resourceDepotControl; }
            set { _resourceDepotControl = value; }
        }
        [SerializeField] private ResourceDepotControlBase _resourceDepotControl; 

        public ResourceDepotSummaryDisplayBase DepotSummaryDisplay {
            get { return _depotSummaryDisplay; }
            set {
                if(_depotSummaryDisplay != null) {
                    _depotSummaryDisplay.DestructionRequested -= DepotSummaryDisplay_DestructionRequested;
                }
                _depotSummaryDisplay = value;
                if(_depotSummaryDisplay != null) {
                    _depotSummaryDisplay.DestructionRequested += DepotSummaryDisplay_DestructionRequested;
                }
            }
        }
        [SerializeField] private ResourceDepotSummaryDisplayBase _depotSummaryDisplay;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(DepotSummaryDisplay != null) {
                DepotSummaryDisplay.DestructionRequested -= DepotSummaryDisplay_DestructionRequested;

                DepotSummaryDisplay.DestructionRequested += DepotSummaryDisplay_DestructionRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<ResourceDepotUISummary>

        public override void PushBeginDragEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushDragEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushEndDragEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushPointerClickEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushPointerExitEvent(ResourceDepotUISummary source, PointerEventData eventData) { }

        public override void PushSelectEvent(ResourceDepotUISummary source, BaseEventData eventData) {
            if(DepotSummaryDisplay != null) {
                DepotSummaryDisplay.CurrentSummary = source as ResourceDepotUISummary;
                DepotSummaryDisplay.Activate();
            }
        }

        public override void PushUpdateSelectedEvent(ResourceDepotUISummary source, BaseEventData eventData) { }

        public override void PushDeselectEvent(ResourceDepotUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(ResourceDepotUISummary source) {
            if(source == DepotSummaryDisplay.CurrentSummary) {
                DepotSummaryDisplay.Deactivate();
            }
        }

        #endregion

        private void DepotSummaryDisplay_DestructionRequested(object sender, EventArgs e) {
            ResourceDepotControl.DestroyResourceDepotOfID(DepotSummaryDisplay.CurrentSummary.ID);
            DepotSummaryDisplay.Deactivate();
        }

        #endregion
        

    }

}
