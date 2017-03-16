using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones {

    public abstract class ConstructionZoneSummaryDisplayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract bool IsActivated { get; }

        public abstract ConstructionZoneUISummary SummaryToDisplay { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> ConstructionZoneDestructionRequested;
        public event EventHandler<EventArgs> CloseRequested;

        protected void RaiseConstructionZoneDestructionRequested() {
            if(ConstructionZoneDestructionRequested != null) {
                ConstructionZoneDestructionRequested(this, EventArgs.Empty);
            }
        }

        protected void RaiseCloseRequested() {
            if(CloseRequested != null) {
                CloseRequested(this, EventArgs.Empty);
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
