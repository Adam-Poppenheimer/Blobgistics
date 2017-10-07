using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.UI.HighwayManager {

    /// <summary>
    /// The abstract base class for the highway manager display, which displays information
    /// and transmit commands about highway managers.
    /// </summary>
    public abstract class HighwayManagerSummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The summary being displayed.
        /// </summary>
        public abstract HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests the destruction of the current highway manager.
        /// </summary>
        public event EventHandler<EventArgs> DestructionRequested;

        /// <summary>
        /// Fires the DestructionRequested event.
        /// </summary>
        protected void RaiseDestructionRequested() {
            if(DestructionRequested != null) {
                DestructionRequested(this, EventArgs.Empty);
            }
        }

        #endregion

    }

}
