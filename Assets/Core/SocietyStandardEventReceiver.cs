using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Societies;
using Assets.UI.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    /// <summary>
    /// The standard event receiver for all events propagating from societies.
    /// </summary>
    /// <remarks>
    /// Currently, the main purpose of this class is to connect the SocietyUISummaryDisplay
    /// panel to the rest of the codebase.
    /// </remarks>
    public class SocietyStandardEventReceiver : TargetedEventReceiverBase<SocietyUISummary> {

        #region instance fields and properties

        /// <summary>
        /// The simulation facade this event receiver interacts with.
        /// </summary>
        public SocietyControlBase SocietyControl {
            get { return _societyControl; }
            set { _societyControl = value; }
        }
        [SerializeField] private SocietyControlBase _societyControl;  

        /// <summary>
        /// The display the event receiver serves.
        /// </summary>
        public SocietyUISummaryDisplayBase SocietySummaryDisplay {
            get { return _societySummaryDisplay; }
            set {
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested                      -= SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested        -= SocietySummaryDisplay_AscensionPermissionChangeRequested;
                    _societySummaryDisplay.ComplexityAscentPermissionChangeRequested -= SocietySummaryDisplay_ComplexityAscentPermissionChangeRequested;
                }
                _societySummaryDisplay = value;
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested                      += SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested        += SocietySummaryDisplay_AscensionPermissionChangeRequested;
                    _societySummaryDisplay.ComplexityAscentPermissionChangeRequested += SocietySummaryDisplay_ComplexityAscentPermissionChangeRequested;
                }
            }
        }
        [SerializeField] private SocietyUISummaryDisplayBase _societySummaryDisplay;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(SocietySummaryDisplay != null) {
                SocietySummaryDisplay.DestructionRequested                      -= SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested        -= SocietySummaryDisplay_AscensionPermissionChangeRequested;
                SocietySummaryDisplay.ComplexityAscentPermissionChangeRequested -= SocietySummaryDisplay_ComplexityAscentPermissionChangeRequested;

                SocietySummaryDisplay.DestructionRequested                      += SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested        += SocietySummaryDisplay_AscensionPermissionChangeRequested;
                SocietySummaryDisplay.ComplexityAscentPermissionChangeRequested += SocietySummaryDisplay_ComplexityAscentPermissionChangeRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<SocietyUISummary>

        /// <inheritdoc/>
        public override void PushBeginDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushEndDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerClickEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerEnterEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushPointerExitEvent(SocietyUISummary source, PointerEventData eventData) { }

        /// <inheritdoc/>
        public override void PushSelectEvent(SocietyUISummary source, BaseEventData eventData) {
            if(SocietySummaryDisplay != null) {
                SocietySummaryDisplay.CurrentSummary = source as SocietyUISummary;
                SocietySummaryDisplay.Activate();
            }
        }

        /// <inheritdoc/>
        public override void PushUpdateSelectedEvent(SocietyUISummary source, BaseEventData eventData) { }

        /// <inheritdoc/>
        public override void PushDeselectEvent(SocietyUISummary source, BaseEventData eventData) { }

        /// <inheritdoc/>
        public override void PushObjectDestroyedEvent(SocietyUISummary source) {
            if(source == SocietySummaryDisplay.CurrentSummary) {
                SocietySummaryDisplay.Deactivate();
            }
        }

        /// <inheritdoc/>
        public override bool TryCloseAllOpenDisplays() {
            if(SocietySummaryDisplay.gameObject.activeInHierarchy) {
                SocietySummaryDisplay.Deactivate();
                return true;
            }else {
                return false;
            }            
        }

        #endregion

        private void SocietySummaryDisplay_DestructionRequested(object sender, EventArgs e) {
            SocietyControl.DestroySociety(SocietySummaryDisplay.CurrentSummary.ID);
            SocietySummaryDisplay.Deactivate();
        }

        private void SocietySummaryDisplay_AscensionPermissionChangeRequested(object sender, BoolEventArgs e) {
            SocietyControl.SetGeneralAscensionPermissionForSociety(SocietySummaryDisplay.CurrentSummary.ID, e.Value);
        }

        private void SocietySummaryDisplay_ComplexityAscentPermissionChangeRequested(object sender, ComplexityAscentPermissionEventArgs e) {
            if(SocietySummaryDisplay.CurrentSummary != null) {
                SocietyControl.SetSpecificAscensionPermissionForSociety(
                    SocietySummaryDisplay.CurrentSummary.ID, e.Complexity, e.AscensionIsPermitted
                );
            }
            
        }

        #endregion

    }

}
