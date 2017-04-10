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

    public class SocietyUISummaryDisplay : SocietyUISummaryDisplayBase, ISelectHandler, IDeselectHandler {

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

        [SerializeField] private Text LocationIDSlot;

        [SerializeField] private Text CurrentComplexitySlot;
        [SerializeField] private Text DescentComplexitySlot;
        [SerializeField] private Text AscentComplexitySlot;

        [SerializeField] private Text NeedsAreSatisfiedSlot;
        [SerializeField] private Text SecondsOfUnsatisfiedNeedsSlot;
        [SerializeField] private Text SecondsUntilComplexityDescentSlot;

        [SerializeField] private Toggle PermitAscensionToggle;

        [SerializeField] private Button DestroySocietyButton;

        private bool DeactivateOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
            }else {
                UpdateDisplay();
            }
        }

        #endregion

        #region Unity EventSystem interfaces

        public void OnSelect(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void OnDeselect(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        #endregion

        #region from SocietyUISummaryDisplayBase

        public override void Activate() {
            PermitAscensionToggle.onValueChanged.AddListener(delegate(bool newValue) {
                RaiseAscensionPermissionChangeRequested(newValue);
            });

            DestroySocietyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });
            gameObject.SetActive(true);
            UpdateDisplay();

            StartCoroutine(ReselectToThis());
        }

        public override void Deactivate() {
            DestroySocietyButton.onClick.RemoveAllListeners();
            PermitAscensionToggle.onValueChanged.RemoveAllListeners();
            ClearDisplay();
            gameObject.SetActive(false);
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                LocationIDSlot.text = CurrentSummary.Location != null ? CurrentSummary.Location.ID.ToString() : "";
                
                CurrentComplexitySlot.text = CurrentSummary.CurrentComplexity;
                DescentComplexitySlot.text = CurrentSummary.DescentComplexity;
                AscentComplexitySlot.text = CurrentSummary.AscentComplexity;

                NeedsAreSatisfiedSlot.text = CurrentSummary.NeedsAreSatisfied.ToString();
                SecondsOfUnsatisfiedNeedsSlot.text = CurrentSummary.SecondsOfUnsatisfiedNeeds.ToString("0.#");
                SecondsUntilComplexityDescentSlot.text = CurrentSummary.SecondsUntilComplexityDescent.ToString("0.#");

                PermitAscensionToggle.isOn = CurrentSummary.AscensionIsPermitted;

                DestroySocietyButton.interactable = CanBeDestroyed;

                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            LocationIDSlot.text = "";
                
            CurrentComplexitySlot.text = "";
            DescentComplexitySlot.text = "";
            AscentComplexitySlot.text = "";

            NeedsAreSatisfiedSlot.text = "";
            SecondsOfUnsatisfiedNeedsSlot.text = "";
            SecondsUntilComplexityDescentSlot.text = "";

            PermitAscensionToggle.isOn = false;
        }

        #endregion

        public void DoOnChildSelected(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void DoOnChildDeselected(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        private IEnumerator ReselectToThis() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        #endregion

    }

}
