using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.ConstructionZones;
using Assets.UI.ConstructionZones;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class ConstructionZoneEventReceiver : TargetedEventReceiverBase<ConstructionZoneUISummary> {

        #region instance fields and properties

        public ConstructionZoneControlBase ConstructionZoneControl {
            get { return _constructionZoneControl; }
            set { _constructionZoneControl = value; }
        }
        [SerializeField] private ConstructionZoneControlBase _constructionZoneControl;  

        public ConstructionZoneSummaryDisplayBase ConstructionZoneSummaryDisplay {
            get { return _constructionZoneSummaryDisplay; }
            set {
                if(_constructionZoneSummaryDisplay != null) {
                    _constructionZoneSummaryDisplay.ConstructionZoneDestructionRequested -= ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
                    _constructionZoneSummaryDisplay.DeactivationRequested                -= ConstructionZoneSummaryDisplay_CloseRequested;
                }
                _constructionZoneSummaryDisplay = value;
                if(_constructionZoneSummaryDisplay != null) {
                    _constructionZoneSummaryDisplay.ConstructionZoneDestructionRequested += ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
                    _constructionZoneSummaryDisplay.DeactivationRequested                += ConstructionZoneSummaryDisplay_CloseRequested;
                }
            }
        }
        [SerializeField] private ConstructionZoneSummaryDisplayBase _constructionZoneSummaryDisplay;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ConstructionZoneSummaryDisplay != null) {
                ConstructionZoneSummaryDisplay.DeactivationRequested                -= ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested -= ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;

                ConstructionZoneSummaryDisplay.DeactivationRequested                += ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested += ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<ConstructionZoneUISummary>

        public override void PushBeginDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushEndDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushPointerClickEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushPointerExitEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        public override void PushSelectEvent(ConstructionZoneUISummary source, BaseEventData eventData) {
            if(ConstructionZoneSummaryDisplay != null) {
                ConstructionZoneSummaryDisplay.CurrentSummary = source as ConstructionZoneUISummary;
                ConstructionZoneSummaryDisplay.Activate();
            }
        }

        public override void PushUpdateSelectedEvent(ConstructionZoneUISummary source, BaseEventData eventData) { }

        public override void PushDeselectEvent(ConstructionZoneUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(ConstructionZoneUISummary source) {
            if(source == ConstructionZoneSummaryDisplay.CurrentSummary) {
                ConstructionZoneSummaryDisplay.Deactivate();
            }
        }

        #endregion

        private void ConstructionZoneSummaryDisplay_CloseRequested(object sender, EventArgs e) {
            ConstructionZoneSummaryDisplay.Deactivate();
        }

        private void ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested(object sender, EventArgs e) {
            ConstructionZoneControl.DestroyConstructionZone(ConstructionZoneSummaryDisplay.CurrentSummary.ID);
            ConstructionZoneSummaryDisplay.Deactivate();
        }

        #endregion
        

    }

}
