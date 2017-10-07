using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.TitleScreen {

    /// <summary>
    /// A class that controls the UI that appears at game start, and acts as
    /// the main menu hub for the game.
    /// </summary>
    /// <remarks>
    /// This class is controlled by UIControl, and produces events for UIControl's benefit.
    /// 
    /// Along with other aspects of the UI, this class should almost certainly operate under
    /// an FSM rather than the ad-hoc form it's taken here. It's a prime candidate for
    /// refactoring.
    /// </remarks>
    public class TitleScreenUI : PanelBase {

        #region instance fields and properties

        //Parts of this namespace were designed before PanelBase existed, so there
        //is inconsistency between the various panels. This should be resolved in any
        //meaningful refactor of the title screen.
        [SerializeField] private NewGameDisplay     NewGameDisplay;
        [SerializeField] private LoadSessionDisplay LoadSessionDisplay;
        [SerializeField] private ExitGameDisplay    ExitGameDisplay;
        [SerializeField] private RectTransform      OptionsDisplay;
        [SerializeField] private PanelBase          CreditsAndAttributionDisplay;
        [SerializeField] private PanelBase          HowToPlayDisplay;

        [SerializeField] private PanningZoomingCameraLogic MainCameraLogic;

        /// <summary>
        /// The display that's currently active. Modifying it activates
        /// and deactivates various panels.
        /// </summary>
        public TitleScreenActiveDisplayType CurrentActiveDisplay {
            get { return _currentActiveDisplay; }
            set {
                NewGameDisplay.gameObject.SetActive    (false);
                LoadSessionDisplay.gameObject.SetActive(false);
                ExitGameDisplay.gameObject.SetActive   (false);
                OptionsDisplay.gameObject.SetActive    (false);
                CreditsAndAttributionDisplay.Deactivate();
                HowToPlayDisplay.Deactivate();

                _currentActiveDisplay = value;

                switch(_currentActiveDisplay) {
                    case TitleScreenActiveDisplayType.NewGame:               NewGameDisplay.gameObject.SetActive    (true); break;
                    case TitleScreenActiveDisplayType.LoadSession:           LoadSessionDisplay.gameObject.SetActive(true); break;
                    case TitleScreenActiveDisplayType.ExitGame:              ExitGameDisplay.gameObject.SetActive   (true); break;
                    case TitleScreenActiveDisplayType.Options:               OptionsDisplay.gameObject.SetActive    (true); break;
                    case TitleScreenActiveDisplayType.CreditsAndAttribution: CreditsAndAttributionDisplay.Activate();       break;
                    case TitleScreenActiveDisplayType.Controls:              HowToPlayDisplay.Activate();                   break;
                    case TitleScreenActiveDisplayType.None: break;
                    default: break;
                }
            }
        }
        private TitleScreenActiveDisplayType _currentActiveDisplay;

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests the game to start its main sequence.
        /// </summary>
        public event EventHandler<EventArgs> GameStartRequested;

        /// <summary>
        /// Fires when the player requests the program to exit.
        /// </summary>
        public event EventHandler<EventArgs> GameExitRequested;

        /// <summary>
        /// Fires the GameStartRequested event.
        /// </summary>
        protected void RaiseGameStartRequested() {
            if(GameStartRequested != null) {
                GameStartRequested(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the GameExitRequested event.
        /// </summary>
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

        /// <summary>
        /// Activates the new game display and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateNewGameDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.NewGame;
        }

        /// <summary>
        /// Activates the load session display  and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateLoadSessionDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.LoadSession;
        }

        /// <summary>
        /// Activates the exit game display and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateExitGameDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.ExitGame;
        }

        /// <summary>
        /// Activates the options display and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateOptionsDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        /// <summary>
        /// Activates the credits and attribution display and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateCreditsAndAttributionDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.CreditsAndAttribution;
        }

        /// <summary>
        /// Activates the controls display and deactivates all other displays.
        /// </summary>
        /// <remarks>
        /// Though this method isn't called in the codebase, it is accessed through the OnClick event
        /// of certain buttons in the UI.
        /// </remarks>
        public void ActivateControlsDisplay() {
            CurrentActiveDisplay = TitleScreenActiveDisplayType.Controls;
        }

        /// <summary>
        /// Allows external entities to request a game exit. Mostly used on button callbacks.
        /// </summary>
        public void RequestGameExit() {
            RaiseGameExitRequested();
        }

        /// <summary>
        /// Allows external entities to request the game to start. Mostly used on button callbacks.
        /// </summary>
        public void RequestGameStart() {
            RaiseGameStartRequested();
        }

        //Another collection of event handlers that amount to an ad-hoc UI that
        //really should be refactored into something more extensible.
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
