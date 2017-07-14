using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.EscapeMenu {

    public class EscapeMenuUI : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private EscapeMenuSaveSessionDisplay SaveSessionDisplay;
        [SerializeField] private EscapeMenuLoadSessionDisplay LoadSessionDisplay;

        [SerializeField] private RectTransform OptionsDisplay;

        #endregion

        #region events

        public event EventHandler<EventArgs> GameResumeRequested;
        public event EventHandler<EventArgs> GameExitRequested;
        public event EventHandler<EventArgs> ReturnToMainMenuRequested;

        protected void RaiseGameResumeRequested() {
            if(GameResumeRequested != null) {
                GameResumeRequested(this, EventArgs.Empty);
            }
        }

        protected void RaiseGameExitRequested() {
            if(GameExitRequested != null) {
                GameExitRequested(this, EventArgs.Empty);
            }
        }

        protected void RaiseReturnToMainMenuRequested() {
            if(ReturnToMainMenuRequested != null) {
                ReturnToMainMenuRequested(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            SaveSessionDisplay.DeactivationRequested += SaveSessionDisplay_DeactivationRequested;
            LoadSessionDisplay.DeactivationRequested += LoadSessionDisplay_DeactivationRequested;

            LoadSessionDisplay.MapLoaded += LoadSessionDisplay_MapLoaded;
        }

        private void OnEnable() {
            ActivateOptionsDisplay();
        }

        private void OnDisable() {
            DeactivateSaveSessionDisplay();
            DeactivateLoadSessionDisplay();
        }

        private void Update() {
            if(Input.GetButtonDown("Cancel")) {
                if(SaveSessionDisplay.gameObject.activeInHierarchy) {
                    DeactivateSaveSessionDisplay();
                    ActivateOptionsDisplay();
                }else if(LoadSessionDisplay.gameObject.activeInHierarchy) {
                    DeactivateLoadSessionDisplay();
                    ActivateOptionsDisplay();
                }else {
                    RaiseGameResumeRequested();
                }
            }
        }

        #endregion

        public void ActivateOptionsDisplay() {
            DeactivateSaveSessionDisplay();
            DeactivateLoadSessionDisplay();
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
            DeactivateOptionsDisplay();
            DeactivateLoadSessionDisplay();
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.Activate();
            }
        }

        public void DeactivateSaveSessionDisplay() {
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.Deactivate();
            }
        }

        public void ActivateLoadSessionDisplay() {
            DeactivateOptionsDisplay();
            DeactivateSaveSessionDisplay();
            if(LoadSessionDisplay != null) {
                LoadSessionDisplay.Activate();
            }
        }

        public void DeactivateLoadSessionDisplay() {
            if(LoadSessionDisplay != null) {
                LoadSessionDisplay.Deactivate();
            }
        }

        public void RaiseResumeRequest() {
            RaiseGameResumeRequested();
        }

        public void RaiseExitRequest() {
            RaiseGameExitRequested();
        }

        public void RaiseReturnToMainMenuRequest() {
            RaiseReturnToMainMenuRequested();
        }

        private void SaveSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            ActivateOptionsDisplay();
        }

        private void LoadSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            ActivateOptionsDisplay();
        }

        private void LoadSessionDisplay_MapLoaded(object sender, EventArgs e) {
            RaiseResumeRequest();
        }

        #endregion

    }

}
