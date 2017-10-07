using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones {

    /// <summary>
    /// The abstract base class for construction zone display, which provides information and
    /// commands for a particular construction zone.
    /// </summary>
    public abstract class ConstructionZoneSummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The summary the display is currently representing.
        /// </summary>
        public abstract ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fired when the player requests the destruction of the construction zone.
        /// </summary>
        public event EventHandler<EventArgs> ConstructionZoneDestructionRequested;

        /// <summary>
        /// Fires the ConstructionZoneDestructionRequested event.
        /// </summary>
        protected void RaiseDestructionRequested() { RaiseEvent(ConstructionZoneDestructionRequested, EventArgs.Empty); }

        #endregion

    }

}
