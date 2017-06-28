using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.UI.HighwayManager {

    public abstract class HighwayManagerSummaryDisplayBase : IntelligentPanelBase {

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

    }

}
