using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones {

    public abstract class ConstructionZoneSummaryDisplayBase : IntelligentPanel {

        #region instance fields and properties

        public abstract ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> ConstructionZoneDestructionRequested;

        protected void RaiseDestructionRequested() { RaiseEvent(ConstructionZoneDestructionRequested, EventArgs.Empty); }

        #endregion

    }

}
