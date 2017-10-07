using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.TitleScreen {

    /// <summary>
    /// A class that controls the exit game display within the title screen.
    /// </summary>
    public class ExitGameDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private Button YesButton;
        [SerializeField] private Button NoButton;

        #endregion

        #region events

        /// <summary>
        /// Fires when the player confirms the exit.
        /// </summary>
        public event EventHandler<EventArgs> ExitConfirmed;

        /// <summary>
        /// Fires when the player rejects the exit.
        /// </summary>
        public event EventHandler<EventArgs> ExitRejected;

        /// <summary>
        /// Fires the ExitConfirmed event.
        /// </summary>
        protected void RaiseExitConfirmed() {
            if(ExitConfirmed != null) {
                ExitConfirmed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the ExitRejected event.
        /// </summary>
        protected void RaiseExitRejected() {
            if(ExitRejected != null) {
                ExitRejected(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity Message methods

        private void Start() {
            YesButton.onClick.AddListener(delegate() { RaiseExitConfirmed(); });
            NoButton.onClick.AddListener (delegate() { RaiseExitRejected (); });
        }

        #endregion

        #endregion

    }

}
