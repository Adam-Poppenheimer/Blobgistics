using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.EscapeMenu {

    public class EscapeMenuUI : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private SaveSessionDisplay SaveSessionDisplay;
        [SerializeField] private RectTransform OptionsDisplay;

        #endregion

        #region events

        public event EventHandler<EventArgs> GameResumeRequested;

        protected void RaiseGameResumeRequested() {
            if(GameResumeRequested != null) {
                GameResumeRequested(null, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            SaveSessionDisplay.DeactivationRequested += SaveSessionDisplay_DeactivationRequested;
        }

        private void OnEnable() {
            ActivateOptionsDisplay();
        }

        private void OnDisable() {
            DeactivateSaveSessionDisplay();
        }

        private void Update() {
            if(Input.GetButtonDown("Cancel")) {
                if(SaveSessionDisplay.gameObject.activeInHierarchy) {
                    DeactivateSaveSessionDisplay();
                    ActivateOptionsDisplay();
                }else {
                    RaiseGameResumeRequested();
                }
                
            }
        }

        #endregion

        public void ActivateOptionsDisplay() {
            if(OptionsDisplay != null) {
                OptionsDisplay.gameObject.SetActive(true);
            }
        }

        public void DeactivateOptionsDisplay() {
            if(OptionsDisplay != null) {
                OptionsDisplay.gameObject.SetActive(false);
            }
        }

        public void ActivateSaveSessionDisplay() {
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.gameObject.SetActive(true);
            }
        }

        public void DeactivateSaveSessionDisplay() {
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.gameObject.SetActive(false);
            }
        }

        public void RaiseResumeRequest() {
            RaiseGameResumeRequested();
        }

        private void SaveSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            DeactivateSaveSessionDisplay();
            ActivateOptionsDisplay();
        }

        #endregion

    }

}
