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

    /// <summary>
    /// The standard event receiver for all events propagating from construction zones.
    /// </summary>
    /// <remarks>
    /// Currently, this class mostly connects the ConstructionZoneSummaryDisplay panel to the
    /// rest of the codebase.
    /// </remarks>
    public class ConstructionZoneStandardEventReceiver : TargetedEventReceiverBase<ConstructionZoneUISummary> {

        #region instance fields and properties

        /// <summary>
        /// The simulation facade this event receiver interacts with.
        /// </summary>
        public ConstructionZoneControlBase ConstructionZoneControl {
            get { return _constructionZoneControl; }
            set { _constructionZoneControl = value; }
        }
        [SerializeField] private ConstructionZoneControlBase _constructionZoneControl;  

        /// <summary>
        /// The display the event receiver serves.
        /// </summary>
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

        private void Start() {
            if(ConstructionZoneSummaryDisplay != null) {
                ConstructionZoneSummaryDisplay.DeactivationRequested                -= ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested -= ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;

                ConstructionZoneSummaryDisplay.DeactivationRequested                += ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested += ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<ConstructionZoneUISummary>

        /// <inheritdoc/>
        public override void PushBeginDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushEndDragEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerClickEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerEnterEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerExitEvent(ConstructionZoneUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushSelectEvent(ConstructionZoneUISummary source, BaseEventData eventData) {
            if(ConstructionZoneSummaryDisplay != null) {
                ConstructionZoneSummaryDisplay.CurrentSummary = source as ConstructionZoneUISummary;
                ConstructionZoneSummaryDisplay.Activate();
            }
        }

        /// <inheritdoc/>
        public override void PushUpdateSelectedEvent(ConstructionZoneUISummary source, BaseEventData eventData) { }

        /// <inheritdoc/>
        public override void PushDeselectEvent(ConstructionZoneUISummary source, BaseEventData eventData) { }

        /// <inheritdoc/>
        public override void PushObjectDestroyedEvent(ConstructionZoneUISummary source) {
            if(source == ConstructionZoneSummaryDisplay.CurrentSummary) {
                ConstructionZoneSummaryDisplay.Deactivate();
            }
        }

        /// <inheritdoc/>
        public override bool TryCloseAllOpenDisplays() {
            if(ConstructionZoneSummaryDisplay.gameObject.activeInHierarchy) {
                ConstructionZoneSummaryDisplay.Deactivate();
                return true;
            }else {
                return false;
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
