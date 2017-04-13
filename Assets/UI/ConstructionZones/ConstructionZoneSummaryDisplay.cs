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

    public class ConstructionZoneSummaryDisplay : ConstructionZoneSummaryDisplayBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Text ProjectNameField;
        [SerializeField] private Text CostField;
        [SerializeField] private Button DestroyButton;

        private bool DeactivateOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(DestroyButton != null) {
                DestroyButton.onClick.AddListener(delegate() {
                    RaiseConstructionZoneDestructionRequested();
                });
            }
        }

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
            }else if(CurrentSummary != null) {
                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
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

        #region from ConstructionZoneSummaryDisplayBase

        public override void Activate() {
            gameObject.SetActive(true);
            if(CurrentSummary != null) {
                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
            }
            UpdateDisplay();
            StartCoroutine(ReselectToThis());
        }

        public override void Clear() {
            CurrentSummary = null;
        }

        public override void Deactivate() {
            Clear();
            gameObject.SetActive(false);
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                ProjectNameField.text = CurrentSummary.Project.Name;
                CostField.text = CurrentSummary.Project.CostSummaryString;
            }
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
