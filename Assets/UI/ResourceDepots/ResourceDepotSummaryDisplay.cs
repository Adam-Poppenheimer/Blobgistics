using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.ResourceDepots;

namespace Assets.UI.ResourceDepots {

    public class ResourceDepotSummaryDisplay : ResourceDepotSummaryDisplayBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from ResourceDepotSummaryDisplayBase

        public override ResourceDepotUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Button DestroyButton;

        private bool DeactivateOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
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

        #region from ResourceDepotSummaryDisplayBase

        public override void Activate() {
            ClearDisplay();
            gameObject.SetActive(true);
            UpdateDisplay();

            DestroyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            } );

            StartCoroutine(ReselectToThis());
        }

        public override void ClearDisplay() { }

        public override void Deactivate() {
            gameObject.SetActive(false);

            DestroyButton.onClick.RemoveAllListeners();
        }

        public override void UpdateDisplay() {}

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
