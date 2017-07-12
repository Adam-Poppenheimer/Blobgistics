using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.TitleScreen {

    public class TitleScreenUI : PanelBase {

        #region static fields and properties

        private static string DisplayErrorMessage = "Failed to activate uninitialized display {0}";

        #endregion

        #region instance fields and properties

        [SerializeField] private NewGameDisplay     NewGameDisplay;
        [SerializeField] private LoadSessionDisplay LoadSessionDisplay;
        [SerializeField] private ExitGameDisplay    ExitGameDisplay;
        [SerializeField] private RectTransform      OptionsDisplay;
        [SerializeField] private PanelBase          CreditsAndAttributionDisplay;

        [SerializeField] private PanningZoomingCameraLogic MainCameraLogic;

        public TitleScreenActiveDisplayType CurrentActiveDisplay {
            get { return _currentActiveDisplay; }
            set {
                NewGameDisplay.gameObject.SetActive    (false);
                LoadSessionDisplay.gameObject.SetActive(false);
                ExitGameDisplay.gameObject.SetActive   (false);
                OptionsDisplay.gameObject.SetActive    (false);
                CreditsAndAttributionDisplay.Deactivate();

                _currentActiveDisplay = value;

                switch(_currentActiveDisplay) {
                    case TitleScreenActiveDisplayType.NewGame:               NewGameDisplay.gameObject.SetActive    (true); break;
                    case TitleScreenActiveDisplayType.LoadSession:           LoadSessionDisplay.gameObject.SetActive(true); break;
                    case TitleScreenActiveDisplayType.ExitGame:              ExitGameDisplay.gameObject.SetActive   (true); break;
                    case TitleScreenActiveDisplayType.Options:               OptionsDisplay.gameObject.SetActive    (true); break;
                    case TitleScreenActiveDisplayType.CreditsAndAttribution: CreditsAndAttributionDisplay.Activate();       break;
                    case TitleScreenActiveDisplayType.None: break;
                    default: break;
                }
            }
        }
        private TitleScreenActiveDisplayType _currentActiveDisplay;

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

            LoadSessionDisplay.DeactivationRequested += LoadSessionDisplay_DeactivationRequested;
            LoadSessionDisplay.SessionLoaded += LoadSessionDisplay_SessionLoaded;

            ExitGameDisplay.ExitConfirmed += ExitGameDisplay_ExitConfirmed;
            ExitGameDisplay.ExitRejected += ExitGameDisplay_ExitRejected;

            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        private void OnEnable() {
            MainCameraLogic.IsReceivingInput = false;
        }

        private void OnDisable() {
            MainCameraLogic.IsReceivingInput = true;
        }

        #endregion

        public void ActivateNewGameDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.NewGame;
        }

        public void ActivateLoadSessionDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.LoadSession;
        }

        public void ActivateExitGameDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.ExitGame;
        }

        public void ActivateOptionsDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        public void ActivateCreditsAndAttributionDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.CreditsAndAttribution;
        }

        private void NewGameDisplay_DeactivationRequested(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        private void NewGameDisplay_MapLoaded(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.None;
            RaiseGameStartRequested();
        }

        private void ExitGameDisplay_ExitRejected(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        private void ExitGameDisplay_ExitConfirmed(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.None;
            RaiseGameExitRequested();
        }

        private void LoadSessionDisplay_DeactivationRequested(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        private void LoadSessionDisplay_SessionLoaded(object sender, EventArgs e) {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.None;
            RaiseGameStartRequested();
        }

        #endregion

    }

}
