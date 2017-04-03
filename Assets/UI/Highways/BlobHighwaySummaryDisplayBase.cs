using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways {

    public abstract class BlobHighwaySummaryDisplayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract BlobHighwayUISummary CurrentSummary { get; set; }

        public abstract bool CanBeUpgraded { get; set; }
        public abstract bool IsBeingUpgraded { get; set; }

        #endregion

        #region events

        public event EventHandler<IntEventArgs> PriorityChanged;

        public event EventHandler<ResourcePermissionEventArgs> FirstEndpointResourcePermissionChanged;
        public event EventHandler<ResourcePermissionEventArgs> SecondEndpointResourcePermissionChanged;

        public event EventHandler<EventArgs> BeginHighwayUpgradeRequested;
        public event EventHandler<EventArgs> CancelHighwayUpgradeRequested;

        protected void RaisePriorityChanged(int newPriority) { RaiseEvent(PriorityChanged, new IntEventArgs(newPriority)); }

        protected void RaiseFirstEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(FirstEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        protected void RaiseSecondEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(SecondEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        protected void RaiseBeginHighwayUpgradeRequested() { RaiseEvent(BeginHighwayUpgradeRequested, EventArgs.Empty); }
        protected void RaiseCancelHighwayUpgradeRequested() { RaiseEvent(CancelHighwayUpgradeRequested, EventArgs.Empty); }

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs{
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void UpdateDisplay();
        public abstract void ClearDisplay();

        #endregion

    }

}
