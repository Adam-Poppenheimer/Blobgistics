using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.Scoring {

    /// <summary>
    /// Class that controls the splash screen that appears when victory has been
    /// achieved.
    /// </summary>
    public class VictorySplashScreen : PanelBase {

        #region instance fields and properties

        [SerializeField] private Button ReturnToTitleScreenButton;

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests a return to the title screen.
        /// </summary>
        public event EventHandler<EventArgs> ReturnToTitleScreenRequested;
        
        /// <summary>
        /// Fires the ReturnToTitleScreenRequested event.
        /// </summary>
        protected void RaiseReturnToTitleScreenRequested() { RaiseEvent(ReturnToTitleScreenRequested, EventArgs.Empty); }

        #endregion

        #region instance methods

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            ReturnToTitleScreenButton.onClick.AddListener(delegate() { RaiseReturnToTitleScreenRequested(); });
        }

        /// <inheritdoc/>
        protected override void DoOnDeactivate() {
            ReturnToTitleScreenButton.onClick.RemoveAllListeners();
        }

        #endregion

    }

}
