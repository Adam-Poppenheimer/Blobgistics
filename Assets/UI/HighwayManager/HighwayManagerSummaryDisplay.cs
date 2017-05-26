using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.HighwayManager;
using Assets.UI.Blobs;
using Assets.Core;

namespace Assets.UI.HighwayManager {

    public class HighwayManagerSummaryDisplay : HighwayManagerSummaryDisplayBase {

        #region instance fields and properties

        #region from HighwayManagerSummaryDisplayBase

        public override HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion
        
        [SerializeField] private HighwayHighlighterControlBase HighwayHighlighter;
        [SerializeField] private HighwayManagerControlBase HighwayManagerControl;

        [SerializeField] private ResourceDisplayBase UpkeepDisplay;
        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            MovePanelWithCamera = true;
        }

        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                ClearDisplay();
                UpdateDisplay();
            }
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            DestroyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });
            DesiredWorldPosition = CurrentSummary.Transform.position;
        }

        protected override void DoOnDeactivate() {
            DestroyButton.onClick.RemoveAllListeners();
        }

        public override void ClearDisplay() {
            HighwayHighlighter.UnhighlightAllHighways();
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                UpkeepDisplay.PushAndDisplaySummary(CurrentSummary.LastUpkeep);
                foreach(var highway in HighwayManagerControl.GetHighwaysManagedByManagerOfID(CurrentSummary.ID)) {
                    HighwayHighlighter.HighlightHighway(highway.ID);
                }
            }
        }

        #endregion

        #endregion

    }

}
