using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.ConstructionZones;


namespace Assets.UI.ConstructionZones {

    public class ConstructionZoneSummaryDisplay : ConstructionZoneSummaryDisplayBase {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Text ProjectNameField;
        [SerializeField] private Text CostField;
        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(DestroyButton != null) {
                DestroyButton.onClick.AddListener(delegate() {
                    RaiseDestructionRequested();
                });
            }
            MovePanelWithCamera = true;
        }

        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
            }
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            if(CurrentSummary != null) {
               DesiredWorldPosition = CurrentSummary.Transform.position;
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                ProjectNameField.text = CurrentSummary.Project.Name;
                CostField.text = CurrentSummary.Project.CostSummaryString;
            }
        }

        #endregion

        #endregion

    }

}
