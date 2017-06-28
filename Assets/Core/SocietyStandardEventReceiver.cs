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

    public class SocietyStandardEventReceiver : TargetedEventReceiverBase<SocietyUISummary> {

        #region instance fields and properties

        public SocietyControlBase SocietyControl {
            get { return _societyControl; }
            set { _societyControl = value; }
        }
        [SerializeField] private SocietyControlBase _societyControl;  

        public SocietyUISummaryDisplayBase SocietySummaryDisplay {
            get { return _societySummaryDisplay; }
            set {
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested -= SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested -= SocietySummaryDisplay_AscensionPermissionChangeRequested;
                }
                _societySummaryDisplay = value;
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested += SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested += SocietySummaryDisplay_AscensionPermissionChangeRequested;
                }
            }
        }
        [SerializeField] private SocietyUISummaryDisplayBase _societySummaryDisplay;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(SocietySummaryDisplay != null) {
                SocietySummaryDisplay.DestructionRequested               -= SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested -= SocietySummaryDisplay_AscensionPermissionChangeRequested;

                SocietySummaryDisplay.DestructionRequested               += SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested += SocietySummaryDisplay_AscensionPermissionChangeRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<SocietyUISummary>

        public override void PushBeginDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushEndDragEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushPointerClickEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushPointerExitEvent(SocietyUISummary source, PointerEventData eventData) { }

        public override void PushSelectEvent(SocietyUISummary source, BaseEventData eventData) {
            if(SocietySummaryDisplay != null) {
                SocietySummaryDisplay.CurrentSummary = source as SocietyUISummary;
                SocietySummaryDisplay.Activate();
            }
        }

        public override void PushUpdateSelectedEvent(SocietyUISummary source, BaseEventData eventData) { }

        public override void PushDeselectEvent(SocietyUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(SocietyUISummary source) {
            if(source == SocietySummaryDisplay.CurrentSummary) {
                SocietySummaryDisplay.Deactivate();
            }
        }

        #endregion

        private void SocietySummaryDisplay_DestructionRequested(object sender, EventArgs e) {
            SocietyControl.DestroySociety(SocietySummaryDisplay.CurrentSummary.ID);
            SocietySummaryDisplay.Deactivate();
        }

        private void SocietySummaryDisplay_AscensionPermissionChangeRequested(object sender, BoolEventArgs e) {
            SocietyControl.SetAscensionPermissionForSociety(SocietySummaryDisplay.CurrentSummary.ID, e.Value);
        }

        #endregion

    }

}
