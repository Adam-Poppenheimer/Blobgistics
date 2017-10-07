using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ResourceDepots;

namespace Assets.UI.ResourceDepots {

    /// <summary>
    /// The abstract base class for the resource depot summary, which provides information
    /// and commands involving resource depots to the player.
    /// </summary>
    public abstract class ResourceDepotSummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The resource depot summary being displayed.
        /// </summary>
        public abstract ResourceDepotUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires when the player has requested the destruction of the current resource depot.
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
