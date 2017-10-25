using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Societies;
using Assets.UI.Blobs;

namespace Assets.UI.Societies {

    /// <summary>
    /// The standard implementation of SocietyUISummaryDisplayBase, which gives players information
    /// and commands regarding societies.
    /// </summary>
    public class SocietyUISummaryDisplay : SocietyUISummaryDisplayBase {

        #region instance fields and properties

        #region from SocietyUISummaryDisplayBase

        /// <inheritdoc/>
        public override SocietyUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Text   NeedsAreSatisfiedField;
        [SerializeField] private Text   SecondsOfUnsatisfiedNeedsField;
        [SerializeField] private Text   SecondsUntilComplexityDescentField;
        [SerializeField] private Toggle PermitAscensionToggle;

        [SerializeField] private Button DestroySocietyButton;

        [SerializeField] private Text CurrentComplexityNameField;
        [SerializeField] private ResourceDisplayBase CurrentComplexityProductionField;
        [SerializeField] private ResourceDisplayBase CurrentComplexityNeedsField;
        [SerializeField] private ResourceDisplayBase CurrentComplexityCostToAscendIntoField;

        [SerializeField] private List<ResourceDisplayBase> WantSummaryDisplayFields;

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

        /// <inheritdoc/>
        protected override void DoOnUpdate() {
            UpdateDisplay();
        }

        #endregion

        #region from IntelligentPanel

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            PermitAscensionToggle.onValueChanged.AddListener(delegate(bool newValue) {
                RaiseAscensionPermissionChangeRequested(newValue);
            });

            DestroySocietyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });

            DesiredWorldPosition = CurrentSummary.Transform.position;
        }

        /// <inheritdoc/>
        protected override void DoOnDeactivate() {
            DestroySocietyButton.onClick.RemoveAllListeners();
            PermitAscensionToggle.onValueChanged.RemoveAllListeners();
        }

        /// <inheritdoc/>
        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                NeedsAreSatisfiedField.text = CurrentSummary.NeedsAreSatisfied.ToString();
                SecondsOfUnsatisfiedNeedsField.text = CurrentSummary.SecondsOfUnsatisfiedNeeds.ToString("0.#");
                SecondsUntilComplexityDescentField.text = CurrentSummary.SecondsUntilComplexityDescent.ToString("0.#");

                PermitAscensionToggle.isOn = CurrentSummary.AscensionIsPermitted;

                UpdateDescentDisplay();
                UpdateCurrentComplexityDisplay(CurrentSummary.CurrentComplexity);
                UpdateAscentDisplay();                
            }
        }

        /// <inheritdoc/>
        public override void ClearDisplay() {
            CurrentSummary = null;
                
            CurrentComplexityNameField.text = "";

            NeedsAreSatisfiedField.text = "false";
            SecondsOfUnsatisfiedNeedsField.text = "";
            SecondsUntilComplexityDescentField.text = "";

            foreach(var display in AscentComplexityShiftDisplays) {
                display.gameObject.SetActive(false);
                display.AscensionPermissionToggle.isOn = false;
            }

            foreach(var display in DescentComplexityShiftDisplays) {
                display.gameObject.SetActive(false);
            }

            PermitAscensionToggle.isOn = false;
        }

        #endregion

        private void UpdateDescentDisplay() {
            foreach(var shiftSummary in DescentComplexityShiftDisplays) {
                shiftSummary.gameObject.SetActive(false);
            }
            while(DescentComplexityShiftDisplays.Count < CurrentSummary.DescentComplexities.Count) {
                var newShiftPrefab = Instantiate(DescentComplexityShiftDisplayPrefab);
                newShiftPrefab.transform.SetParent(DescentComplexitySection, false);
                DescentComplexityShiftDisplays.Add(newShiftPrefab.GetComponent<ComplexityShiftDisplay>());
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
            CurrentComplexityNameField.text = currentComplexity.name;
            CurrentComplexityProductionField.PushAndDisplaySummary(currentComplexity.Production);
            CurrentComplexityNeedsField.PushAndDisplaySummary(currentComplexity.Needs);

            var wantList = currentComplexity.Wants.ToList();
            int i = 0;

            for(; i < wantList.Count; ++i) {
                WantSummaryDisplayFields[i].PushAndDisplaySummary(wantList[i]);
                WantSummaryDisplayFields[i].gameObject.SetActive(true);
            }
            for(; i < WantSummaryDisplayFields.Count; ++i) {
                WantSummaryDisplayFields[i].gameObject.SetActive(false);
            }

            CurrentComplexityCostToAscendIntoField.PushAndDisplaySummary(currentComplexity.CostToAscendInto);
        }

        private void UpdateAscentDisplay() {
            foreach(var shiftSummary in AscentComplexityShiftDisplays) {
                shiftSummary.gameObject.SetActive(false);
            }
            while(AscentComplexityShiftDisplays.Count < CurrentSummary.AscentComplexities.Count) {
                var newTextPrefab = Instantiate(AscentComplexityShiftDisplayPrefab);
                newTextPrefab.transform.SetParent(AscentComplexitySection, false);
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

                shiftSummary.AscensionPermissionToggle.isOn = CurrentSummary.GetAscensionPermissionForComplexity(ascentComplexity);
                shiftSummary.AscensionPermissionToggle.onValueChanged.AddListener(delegate(bool newValue) {
                    RaiseComplexityAscentPermissionChangeRequested(ascentComplexity, newValue);
                });

                shiftSummary.RefreshDisplay();
            }
        }

        #endregion

    }

}
