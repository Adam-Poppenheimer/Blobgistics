using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Assets.Societies;

namespace Assets.UI.Societies {

    public class SocietyUISummaryDisplay : SocietyUISummaryDisplayBase {

        #region instance fields and properties

        #region from SocietyUISummaryDisplayBase

        public override SocietyUISummary CurrentSummary { get; set; }

        public override bool CanBeDestroyed {
            get { return _canBeDestroyed; }
            set {
                _canBeDestroyed = value;
                DestroySocietyButton.interactable = _canBeDestroyed;
            }
        }
        private bool _canBeDestroyed = false;

        #endregion

        [SerializeField] private Text   LocationIDField;
        [SerializeField] private Toggle NeedsAreSatisfiedToggle;
        [SerializeField] private Text   SecondsOfUnsatisfiedNeedsField;
        [SerializeField] private Text   SecondsUntilComplexityDescentField;
        [SerializeField] private Toggle PermitAscensionToggle;

        [SerializeField] private Button DestroySocietyButton;

        [SerializeField] private Text CurrentComplexityNameField;
        [SerializeField] private Text CurrentComplexityProductionField;
        [SerializeField] private Text CurrentComplexityNeedsField;
        [SerializeField] private Text CurrentComplexityWantsField;
        [SerializeField] private Text CurrentComplexityCostOfAscentField;

        [SerializeField] private Text DescentComplexityNameField;
        [SerializeField] private Text AscentComplexityNameField;

        

        #endregion

        #region instance methods

        #region Unity event methods

        protected override void DoOnUpdate() {
            UpdateDisplay();
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            PermitAscensionToggle.onValueChanged.AddListener(delegate(bool newValue) {
                RaiseAscensionPermissionChangeRequested(newValue);
            });

            DestroySocietyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });
        }

        protected override void DoOnDeactivate() {
            DestroySocietyButton.onClick.RemoveAllListeners();
            PermitAscensionToggle.onValueChanged.RemoveAllListeners();
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                LocationIDField.text = CurrentSummary.Location != null ? CurrentSummary.Location.ID.ToString() : "";
                
                CurrentComplexityNameField.text = CurrentSummary.CurrentComplexity;
                DescentComplexityNameField.text = CurrentSummary.DescentComplexity;
                AscentComplexityNameField.text = CurrentSummary.AscentComplexity;

                NeedsAreSatisfiedToggle.isOn = CurrentSummary.NeedsAreSatisfied;
                SecondsOfUnsatisfiedNeedsField.text = CurrentSummary.SecondsOfUnsatisfiedNeeds.ToString("0.#");
                SecondsUntilComplexityDescentField.text = CurrentSummary.SecondsUntilComplexityDescent.ToString("0.#");

                PermitAscensionToggle.isOn = CurrentSummary.AscensionIsPermitted;

                DestroySocietyButton.interactable = CanBeDestroyed;

                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            LocationIDField.text = "";
                
            CurrentComplexityNameField.text = "";
            DescentComplexityNameField.text = "";
            AscentComplexityNameField.text = "";

            NeedsAreSatisfiedToggle.isOn = false;
            SecondsOfUnsatisfiedNeedsField.text = "";
            SecondsUntilComplexityDescentField.text = "";

            PermitAscensionToggle.isOn = false;
        }

        #endregion

        #endregion

    }

}
