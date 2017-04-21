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
        [SerializeField] private Text   NeedsAreSatisfiedField;
        [SerializeField] private Text   SecondsOfUnsatisfiedNeedsField;
        [SerializeField] private Text   SecondsUntilComplexityDescentField;
        [SerializeField] private Toggle PermitAscensionToggle;

        [SerializeField] private Button DestroySocietyButton;

        [SerializeField] private RectTransform DescentComplexitySection;
        [SerializeField] private Text DescentComplexityNameField;

        [SerializeField] private Text CurrentComplexityNameField;
        [SerializeField] private Text CurrentComplexityProductionField;
        [SerializeField] private Text CurrentComplexityNeedsField;
        [SerializeField] private Text CurrentComplexityWantsField;
        [SerializeField] private Text CurrentComplexityCostToAscendIntoField;
        [SerializeField] private Text CurrentComplexityCostToAscendFromField;

        [SerializeField] private RectTransform AscentComplexitySection;
        [SerializeField] private Text AscentComplexityNameField;

        

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            MovePanelWithCamera = true;
        }

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

            DesiredWorldPosition = CurrentSummary.Transform.position;
        }

        protected override void DoOnDeactivate() {
            DestroySocietyButton.onClick.RemoveAllListeners();
            PermitAscensionToggle.onValueChanged.RemoveAllListeners();
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                LocationIDField.text = CurrentSummary.Location != null ? CurrentSummary.Location.ID.ToString() : "";

                NeedsAreSatisfiedField.text = CurrentSummary.NeedsAreSatisfied.ToString();
                SecondsOfUnsatisfiedNeedsField.text = CurrentSummary.SecondsOfUnsatisfiedNeeds.ToString("0.#");
                SecondsUntilComplexityDescentField.text = CurrentSummary.SecondsUntilComplexityDescent.ToString("0.#");

                PermitAscensionToggle.isOn = CurrentSummary.AscensionIsPermitted;

                DestroySocietyButton.interactable = CanBeDestroyed;

                UpdateDescentComplexityDisplay(CurrentSummary.DescentComplexity);
                UpdateCurrentComplexityDisplay(CurrentSummary.CurrentComplexity);
                UpdateAscentComplexityDisplay(CurrentSummary.AscentComplexity);
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            LocationIDField.text = "";
                
            CurrentComplexityNameField.text = "";
            DescentComplexityNameField.text = "";
            AscentComplexityNameField.text = "";

            NeedsAreSatisfiedField.text = "false";
            SecondsOfUnsatisfiedNeedsField.text = "";
            SecondsUntilComplexityDescentField.text = "";

            PermitAscensionToggle.isOn = false;
        }

        #endregion

        private void UpdateDescentComplexityDisplay(ComplexityDefinitionBase descentComplexity) {
            if(descentComplexity != null) {
                DescentComplexitySection.gameObject.SetActive(true);
                DescentComplexityNameField.text = descentComplexity.Name;
            }else {
                DescentComplexitySection.gameObject.SetActive(false);
                DescentComplexityNameField.text = "--";
            }
        }

        private void UpdateCurrentComplexityDisplay(ComplexityDefinitionBase currentComplexity) {
            CurrentComplexityNameField.text = currentComplexity.Name;
            CurrentComplexityProductionField.text = currentComplexity.Production.GetSummaryString();
            CurrentComplexityNeedsField.text = currentComplexity.Needs.GetSummaryString();

            CurrentComplexityWantsField.text = "";
            foreach(var want in currentComplexity.Wants) {
                if(want == currentComplexity.Wants.Last()) {
                    CurrentComplexityWantsField.text += want.GetSummaryString();
                }else {
                    CurrentComplexityWantsField.text += want.GetSummaryString() + " OR ";
                }
            }
            CurrentComplexityCostToAscendIntoField.text = currentComplexity.CostOfAscent.GetSummaryString();
        }

        private void UpdateAscentComplexityDisplay(ComplexityDefinitionBase ascentComplexity) {
            if(ascentComplexity != null) {
                AscentComplexitySection.gameObject.SetActive(true);
                AscentComplexityNameField.text = ascentComplexity.Name;
                CurrentComplexityCostToAscendFromField.text = ascentComplexity.CostOfAscent.GetSummaryString();
            }else {
                AscentComplexitySection.gameObject.SetActive(false);
                AscentComplexityNameField.text = "--";
                CurrentComplexityCostToAscendFromField.text = "--";
            }
        }

        #endregion

    }

}
