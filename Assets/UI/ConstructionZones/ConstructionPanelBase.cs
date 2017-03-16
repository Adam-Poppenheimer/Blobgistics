using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.UI.ConstructionZones {

    public abstract class ConstructionPanelBase : MonoBehaviour {

        #region instance fields and properties

        public abstract bool IsActivated { get; }

        public abstract MapNodeUISummary LocationToConstruct { get; set; }

        public abstract bool HasPermissionForResourceDepot { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> DepotConstructionRequested;
        public event EventHandler<EventArgs> CloseRequested;

        protected void RaiseDepotConstructionRequested() {
            if(DepotConstructionRequested != null) {
                DepotConstructionRequested(this, EventArgs.Empty);
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
