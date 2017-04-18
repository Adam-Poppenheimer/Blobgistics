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

namespace Assets.UI.HighwayManager {

    public class HighwayManagerSummaryDisplay : HighwayManagerSummaryDisplayBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from HighwayManagerSummaryDisplayBase

        public override HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Text LastEfficiencyField;
        
        [SerializeField] private Text UpkeepFoodField;
        [SerializeField] private Text UpkeepYellowField;
        [SerializeField] private Text UpkeepWhiteField;
        [SerializeField] private Text UpkeepBlueField;

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

        #region from HighwayManagerSummaryDisplayBase

        public override void Activate() {
            ClearDisplay();
            gameObject.SetActive(true);
            UpdateDisplay();

            DestroyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });

            StartCoroutine(ReselectToThis());
        }

        public override void ClearDisplay() {
            LastEfficiencyField.text = "0.0";

            UpkeepFoodField.text   = "0";
            UpkeepYellowField.text = "0";
            UpkeepWhiteField.text  = "0";
            UpkeepBlueField.text   = "0";
        }

        public override void Deactivate() {
            gameObject.SetActive(false);

            DestroyButton.onClick.RemoveAllListeners();
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                LastEfficiencyField.text = CurrentSummary.LastEfficiency.ToString();

                var upkeep = CurrentSummary.LastUpkeep;

                int foodValue = 0;
                int yellowValue = 0;
                int whiteValue = 0;
                int blueValue = 0;

                upkeep.TryGetValue(ResourceType.Food,   out foodValue  );
                upkeep.TryGetValue(ResourceType.Yellow, out yellowValue);
                upkeep.TryGetValue(ResourceType.White,  out whiteValue );
                upkeep.TryGetValue(ResourceType.Blue,   out blueValue  );

                UpkeepFoodField.text   = foodValue.ToString();
                UpkeepYellowField.text = yellowValue.ToString();
                UpkeepWhiteField.text  = whiteValue.ToString();
                UpkeepBlueField.text   = blueValue.ToString();
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
