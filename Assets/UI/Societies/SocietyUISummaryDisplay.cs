using System;
using System.Collections;
using System.Collections.ObjectModel;
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

        #endregion

        [SerializeField] private Text   LocationIDField;
        [SerializeField] private Text   NeedsAreSatisfiedField;
        [SerializeField] private Text   SecondsOfUnsatisfiedNeedsField;
        [SerializeField] private Text   SecondsUntilComplexityDescentField;
        [SerializeField] private Toggle PermitAscensionToggle;

        [SerializeField] private Button DestroySocietyButton;

        [SerializeField] private Text CurrentComplexityNameField;
        [SerializeField] private Text CurrentComplexityProductionField;
        [SerializeField] private Text CurrentComplexityNeedsField;
        [SerializeField] private Text CurrentComplexityWantsField;
        [SerializeField] private Text CurrentComplexityCostToAscendIntoField;

        [SerializeField] private RectTransform DescentComplexitySection;
        [SerializeField] private RectTransform AscentComplexitySection;

        [SerializeField] private GameObject AscentComplexityShiftDisplayPrefab;
        [SerializeField] private GameObject DescentComplexityShiftDisplayPrefab;

        private List<ComplexityShiftDisplay> AscentComplexityShiftDisplays  = new List<ComplexityShiftDisplay>();
        private List<ComplexityShiftDisplay> DescentComplexityShiftDisplays = new List<ComplexityShiftDisplay>();

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

                UpdateDescentDisplay();
                UpdateCurrentComplexityDisplay(CurrentSummary.CurrentComplexity);
                UpdateAscentDisplay();                
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            LocationIDField.text = "";
                
            CurrentComplexityNameField.text = "";

            NeedsAreSatisfiedField.text = "false";
            SecondsOfUnsatisfiedNeedsField.text = "";
            SecondsUntilComplexityDescentField.text = "";

            foreach(var textField in AscentComplexityShiftDisplays) {
                textField.gameObject.SetActive(false);
            }

            foreach(var textField in DescentComplexityShiftDisplays) {
                textField.gameObject.SetActive(false);
            }

            PermitAscensionToggle.isOn = false;
        }

        #endregion

        private void UpdateDescentDisplay() {
            foreach(var shiftSummary in DescentComplexityShiftDisplays) {
                shiftSummary.gameObject.SetActive(false);
            }
            while(DescentComplexityShiftDisplays.Count < CurrentSummary.DescentComplexities.Count) {
                var newTextPrefab = Instantiate(AscentComplexityShiftDisplayPrefab);
                newTextPrefab.transform.SetParent(DescentComplexitySection);
                DescentComplexityShiftDisplays.Add(newTextPrefab.GetComponent<ComplexityShiftDisplay>());
            }

            for(int i = 0; i < CurrentSummary.DescentComplexities.Count; ++i) {
                var descentComplexity = CurrentSummary.DescentComplexities[i];
                var shiftSummary = DescentComplexityShiftDisplays[i];

                shiftSummary.gameObject.SetActive(true);
                shiftSummary.ComplexityToDisplay = descentComplexity;
                if(descentComplexity.PermittedTerrains.Contains(CurrentSummary.Location.Terrain)) {
                    shiftSummary.IsCandidateForShift = true;
                }else {
                    shiftSummary.IsCandidateForShift = false;
                }

                shiftSummary.RefreshDisplay();
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
            CurrentComplexityCostToAscendIntoField.text = currentComplexity.CostToAscendInto.GetSummaryString();
        }

        private void UpdateAscentDisplay() {
            foreach(var shiftSummary in AscentComplexityShiftDisplays) {
                shiftSummary.gameObject.SetActive(false);
            }
            while(AscentComplexityShiftDisplays.Count < CurrentSummary.AscentComplexities.Count) {
                var newTextPrefab = Instantiate(AscentComplexityShiftDisplayPrefab);
                newTextPrefab.transform.SetParent(AscentComplexitySection);
                AscentComplexityShiftDisplays.Add(newTextPrefab.GetComponent<ComplexityShiftDisplay>());
            }

            for(int i = 0; i < CurrentSummary.AscentComplexities.Count; ++i) {
                var ascentComplexity = CurrentSummary.AscentComplexities[i];
                var shiftSummary = AscentComplexityShiftDisplays[i];

                shiftSummary.gameObject.SetActive(true);
                shiftSummary.ComplexityToDisplay = ascentComplexity;
                if(ascentComplexity.PermittedTerrains.Contains(CurrentSummary.Location.Terrain)) {
                    shiftSummary.IsCandidateForShift = true;
                }else {
                    shiftSummary.IsCandidateForShift = false;
                }

                shiftSummary.RefreshDisplay();
            }
        }

        #endregion

    }

}
