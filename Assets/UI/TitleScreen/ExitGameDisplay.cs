using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.TitleScreen {

    public class ExitGameDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private Button YesButton;
        [SerializeField] private Button NoButton;

        #endregion

        #region events

        public event EventHandler<EventArgs> ExitConfirmed;
        public event EventHandler<EventArgs> ExitRejected;

        protected void RaiseExitConfirmed() {
            if(ExitConfirmed != null) {
                ExitConfirmed(this, EventArgs.Empty);
            }
        }

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
