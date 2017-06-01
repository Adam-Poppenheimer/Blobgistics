using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.TitleScreen {

    public class TitleScreenUI : MonoBehaviour {

        #region static fields and properties

        private static string DisplayErrorMessage = "Failed to activate uninitialized display {0}";

        #endregion

        #region instance fields and properties

        [SerializeField] private NewGameDisplay     NewGameDisplay;
        [SerializeField] private LoadSessionDisplay LoadSessionDisplay;
        [SerializeField] private ExitGameDisplay    ExitGameDisplay;
        [SerializeField] private RectTransform      OptionsDisplay;

        #endregion

        #region events

        public event EventHandler<EventArgs> GameStartRequested;
        public event EventHandler<EventArgs> GameExitRequested;

        protected void RaiseGameStartRequested() {
            if(GameStartRequested != null) {
                GameStartRequested(null, EventArgs.Empty);
            }
        }

        protected void RaiseGameExitRequested() {
            if(GameExitRequested != null) {
                GameExitRequested(null, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity Message methods

        private void Start() {
            NewGameDisplay.DeactivationRequested += NewGameDisplay_DeactivationRequested;
            NewGameDisplay.MapLoaded += NewGameDisplay_MapLoaded;
            DeactivateNewGameDisplay();

            LoadSessionDisplay.DeactivationRequested += LoadSessionDisplay_DeactivationRequested;
            LoadSessionDisplay.SessionLoaded += LoadSessionDisplay_SessionLoaded;

            ExitGameDisplay.ExitConfirmed += ExitGameDisplay_ExitConfirmed;
            ExitGameDisplay.ExitRejected += ExitGameDisplay_ExitRejected;
            DeactivateExitGameDisplay();

            ActivateOptionsDisplay();
        }

        #endregion

        public void ActivateNewGameDisplay() {
            if(NewGameDisplay != null) {
                DeactivateOptionsDisplay();
                NewGameDisplay.gameObject.SetActive(true);
            }else {
                Debug.LogErrorFormat(DisplayErrorMessage, "NewGameDisplay");
            }
        }

        public void DeactivateNewGameDisplay() {
            if(NewGameDisplay != null) {
                NewGameDisplay.gameObject.SetActive(false);
            }
        }

        public void ActivateLoadSessionDisplay() {
            if(LoadSessionDisplay != null) {
                DeactivateOptionsDisplay();
                LoadSessionDisplay.gameObject.SetActive(true);
            }else {
                Debug.LogErrorFormat(DisplayErrorMessage, "LoadSessionDisplay");
            }
        }

        public void DeactivateLoadSessionDisplay() {
            if(LoadSessionDisplay != null) {
                LoadSessionDisplay.gameObject.SetActive(false);
            }
        }

        public void ActivateExitGameDisplay() {
            if(ExitGameDisplay != null) {
                DeactivateOptionsDisplay();
                ExitGameDisplay.gameObject.SetActive(true);
            }else {
                Debug.LogErrorFormat(DisplayErrorMessage, "ExitGameDisplay");
            }
        }

        public void DeactivateExitGameDisplay() {
            if(ExitGameDisplay != null){
                ExitGameDisplay.gameObject.SetActive(false);
            }
        }

        public void ActivateOptionsDisplay() {
            if(OptionsDisplay != null){
                OptionsDisplay.gameObject.SetActive(true);
            }else {
                Debug.LogErrorFormat(DisplayErrorMessage, "ActivateOptionsDisplay");
            }
        }

        public void DeactivateOptionsDisplay() {
            if(OptionsDisplay != null){
                OptionsDisplay.gameObject.SetActive(false);
            }
        }

        private void NewGameDisplay_DeactivationRequested(object sender, EventArgs e) {
            DeactivateNewGameDisplay();
            ActivateOptionsDisplay();
        }

        private void NewGameDisplay_MapLoaded(object sender, EventArgs e) {
            DeactivateNewGameDisplay();
            RaiseGameStartRequested();
        }

        private void ExitGameDisplay_ExitRejected(object sender, EventArgs e) {
            DeactivateExitGameDisplay();
            ActivateOptionsDisplay();
        }

        private void ExitGameDisplay_ExitConfirmed(object sender, EventArgs e) {
            DeactivateExitGameDisplay();
            RaiseGameExitRequested();
        }

        private void LoadSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            DeactivateLoadSessionDisplay();
            ActivateOptionsDisplay();
        }

        private void LoadSessionDisplay_SessionLoaded(object sender, EventArgs e) {
            DeactivateLoadSessionDisplay();
            RaiseGameStartRequested();
        }

        #endregion

    }

}
