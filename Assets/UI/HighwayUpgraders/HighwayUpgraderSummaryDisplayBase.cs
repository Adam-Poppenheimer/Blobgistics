using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.HighwayUpgraders;

namespace Assets.UI.HighwayUpgraders {

    public abstract class HighwayUpgraderSummaryDisplayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract HighwayUpgraderUISummary SummaryToDisplay{ get; set; }

        public abstract bool IsActivated { get; }

        #endregion

        #region events

        public event EventHandler<EventArgs> UpgraderDestructionRequested;
        public event EventHandler<EventArgs> DisplayCloseRequested;

        protected void RaiseUpgraderDestructionRequested() {
            if(UpgraderDestructionRequested != null) {
                UpgraderDestructionRequested(this, EventArgs.Empty);
            }
        }

        protected void RaiseDisplayCloseRequested() {
            if(DisplayCloseRequested != null) {
                DisplayCloseRequested(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void Clear();

        #endregion

    }

}
