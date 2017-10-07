using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.EscapeMenu {

    /// <summary>
    /// A class that controls the UI that appears when Cancel is pressed during
    /// gameplay.
    /// </summary>
    /// <remarks>
    /// This class is controlled by UIControl, and produces events for UIControl's benefit.
    /// 
    /// Along with other aspects of the UI, this class should almost certainly operate under
    /// an FSM rather than the ad-hoc form it's taken here. It's a prime candidate for
    /// refactoring.
    /// </remarks>
    public class EscapeMenuUI : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private EscapeMenuSaveSessionDisplay SaveSessionDisplay;
        [SerializeField] private EscapeMenuLoadSessionDisplay LoadSessionDisplay;

        [SerializeField] private RectTransform OptionsDisplay;

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests that the game resumes.
        /// </summary>
        public event EventHandler<EventArgs> GameResumeRequested;

        /// <summary>
        /// Fires when the player requests that the game exits.
        /// </summary>
        public event EventHandler<EventArgs> GameExitRequested;

        /// <summary>
        /// Fires when the player requests that the game return to the main menu.
        /// </summary>
        public event EventHandler<EventArgs> ReturnToMainMenuRequested;

        /// <summary>
        /// Fires the GameResumeRequested event.
        /// </summary>
        protected void RaiseGameResumeRequested() {
            if(GameResumeRequested != null) {
                GameResumeRequested(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the GameExitRequested event.
        /// </summary>
        protected void RaiseGameExitRequested() {
            if(GameExitRequested != null) {
                GameExitRequested(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the ReturnToMainMenuRequested event.
        /// </summary>
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

        //The UI backs out of menus in layers when the Cancel button
        //is pressed. This particular code is not robust to extension,
        //but is viewed as acceptable for the needs of the class.
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

        /// <summary>
        /// Activates the options display, which allows players to select
        /// between various escape menu tasks.
        /// </summary>
        public void ActivateOptionsDisplay() {
            DeactivateSaveSessionDisplay();
            DeactivateLoadSessionDisplay();
            if(OptionsDisplay != null) {
                OptionsDisplay.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Deactivates the options display.
        /// </summary>
        public void DeactivateOptionsDisplay() {
            if(OptionsDisplay != null) {
                OptionsDisplay.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Activates the save session display, which allows players to save their
        /// current session for resumption later.
        /// </summary>
        public void ActivateSaveSessionDisplay() {
            DeactivateOptionsDisplay();
            DeactivateLoadSessionDisplay();
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.Activate();
            }
        }

        /// <summary>
        /// Deactivates the save session display.
        /// </summary>
        public void DeactivateSaveSessionDisplay() {
            if(SaveSessionDisplay != null) {
                SaveSessionDisplay.Deactivate();
            }
        }

        /// <summary>
        /// Activates the load session display, which allows players to
        /// immediately load a previously saved session.
        /// </summary>
        public void ActivateLoadSessionDisplay() {
            DeactivateOptionsDisplay();
            DeactivateSaveSessionDisplay();
            if(LoadSessionDisplay != null) {
                LoadSessionDisplay.Activate();
            }
        }

        /// <summary>
        /// Deactivates the load session display.
        /// </summary>
        public void DeactivateLoadSessionDisplay() {
            if(LoadSessionDisplay != null) {
                LoadSessionDisplay.Deactivate();
            }
        }

        private void SaveSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            ActivateOptionsDisplay();
        }

        private void LoadSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            ActivateOptionsDisplay();
        }

        private void LoadSessionDisplay_MapLoaded(object sender, EventArgs e) {
            RaiseGameResumeRequested();
        }

        #endregion

    }

}
