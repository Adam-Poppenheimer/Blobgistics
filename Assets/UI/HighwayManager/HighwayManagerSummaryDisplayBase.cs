using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.HighwayManager;

namespace Assets.UI.HighwayManager {

    public abstract class HighwayManagerSummaryDisplayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> DestructionRequested;

        protected void RaiseDestructionRequested() {
            if(DestructionRequested != null) {
                DestructionRequested(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void UpdateDisplay();
        public abstract void ClearDisplay();

        #endregion

    }

}
