using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.HighwayManager;
using Assets.UI.HighwayManager;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class HighwayManagerStandardEventReceiver : TargetedEventReceiverBase<HighwayManagerUISummary> {

        #region instance fields and properties

        public HighwayManagerControlBase HighwayManagerControl {
            get { return _highwayManagerControl; }
            set { _highwayManagerControl = value; }
        }
        [SerializeField] private HighwayManagerControlBase _highwayManagerControl;

        public HighwayManagerSummaryDisplayBase HighwayManagerDisplay {
            get { return _highwayManagerDisplay; }
            set {
                if(_highwayManagerDisplay != null) {
                    _highwayManagerDisplay.DestructionRequested -= HighwayManagerDisplay_DestructionRequested;
                }
                _highwayManagerDisplay = value;
                if(_highwayManagerDisplay != null) {
                    _highwayManagerDisplay.DestructionRequested += HighwayManagerDisplay_DestructionRequested;
                }
            }
        }
        [SerializeField] private HighwayManagerSummaryDisplayBase _highwayManagerDisplay;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(HighwayManagerDisplay != null) {
                HighwayManagerDisplay.DestructionRequested -= HighwayManagerDisplay_DestructionRequested;

                HighwayManagerDisplay.DestructionRequested += HighwayManagerDisplay_DestructionRequested;
            }
        }

        #endregion

        #region from TargetedEventReceiverBase<HighwayManagerUISummary>

        public override void PushBeginDragEvent(HighwayManagerUISummary source, PointerEventData eventData) { }

        public override void PushDragEvent(HighwayManagerUISummary source, PointerEventData eventData) { }

        public override void PushEndDragEvent(HighwayManagerUISummary source, PointerEventData eventData) { }  

        public override void PushPointerClickEvent(HighwayManagerUISummary source, PointerEventData eventData) { }

        public override void PushPointerEnterEvent(HighwayManagerUISummary source, PointerEventData eventData) { }

        public override void PushPointerExitEvent(HighwayManagerUISummary source, PointerEventData eventData) { }

        public override void PushSelectEvent(HighwayManagerUISummary source, BaseEventData eventData) {
            if(HighwayManagerDisplay != null) {
                HighwayManagerDisplay.CurrentSummary = source;
                HighwayManagerDisplay.Activate();
            }
        }

        public override void PushUpdateSelectedEvent(HighwayManagerUISummary source, BaseEventData eventData) { }

        public override void PushDeselectEvent(HighwayManagerUISummary source, BaseEventData eventData) { }

        public override void PushObjectDestroyedEvent(HighwayManagerUISummary source) {
            if(source == HighwayManagerDisplay.CurrentSummary) {
                HighwayManagerDisplay.Deactivate();
            }
        }

        #endregion

        private void HighwayManagerDisplay_DestructionRequested(object sender, EventArgs e) {
            HighwayManagerControl.DestroyHighwayManagerOfID(HighwayManagerDisplay.CurrentSummary.ID);
            HighwayManagerDisplay.Deactivate();
        }

        #endregion

    }

}
