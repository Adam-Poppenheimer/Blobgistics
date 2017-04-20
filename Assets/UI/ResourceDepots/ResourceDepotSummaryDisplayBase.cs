using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ResourceDepots;

namespace Assets.UI.ResourceDepots {

    public abstract class ResourceDepotSummaryDisplayBase : IntelligentPanel {

        #region instance fields and properties

        public abstract ResourceDepotUISummary CurrentSummary { get; set; }

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
