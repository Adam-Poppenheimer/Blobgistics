using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.Scoring {

    public class VictorySplashScreen : PanelBase {

        #region instance fields and properties

        [SerializeField] private Button ReturnToTitleScreenButton;

        #endregion

        #region events

        public event EventHandler<EventArgs> ReturnToTitleScreenRequested;

        protected void RaiseReturnToTitleScreenRequested() { RaiseEvent(ReturnToTitleScreenRequested, EventArgs.Empty); }

        #endregion

        #region instance methods

        protected override void DoOnActivate() {
            ReturnToTitleScreenButton.onClick.AddListener(delegate() { RaiseReturnToTitleScreenRequested(); });
        }

        protected override void DoOnDeactivate() {
            ReturnToTitleScreenButton.onClick.RemoveAllListeners();
        }

        #endregion

    }

}
