using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.ConstructionZones {

    public abstract class ConstructionPanelBase : MonoBehaviour {

        #region instance fields and properties

        public abstract bool IsActivated { get; }

        public abstract MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        #region events

        public event EventHandler<StringEventArgs> ConstructionRequested;

        public event EventHandler<EventArgs> CloseRequested;

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        protected void RaiseConstructionRequested(string projectName) { RaiseEvent(ConstructionRequested, new StringEventArgs(projectName)); }

        protected void RaiseCloseRequested() { RaiseEvent(CloseRequested, EventArgs.Empty); }

        #endregion

        #region instance methods

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Clear();

        public abstract void SetPermissions(IEnumerable<string> permittedProjects);

        #endregion

    }

}
