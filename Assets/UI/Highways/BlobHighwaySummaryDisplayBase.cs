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

        #endregion

        #region events

        public event EventHandler<IntEventArgs> PriorityChanged;
        public event EventHandler<ResourcePermissionEventArgs> FirstEndpointResourcePermissionChanged;
        public event EventHandler<ResourcePermissionEventArgs> SecondEndpointResourcePermissionChanged;
        public event EventHandler<EventArgs> HighwayUpgradeRequested;

        protected void RaisePriorityChanged(int newPriority) {
            if(PriorityChanged != null) {
                PriorityChanged(this, new IntEventArgs(newPriority));
            }
        }

        protected void RaiseFirstEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            if(FirstEndpointResourcePermissionChanged != null) {
                FirstEndpointResourcePermissionChanged(this, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
            }
        }

        protected void RaiseSecondEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            if(SecondEndpointResourcePermissionChanged != null) {
                SecondEndpointResourcePermissionChanged(this, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
            }
        }

        protected void RaiseHighwayUpgradeRequested() {
            if(HighwayUpgradeRequested != null) {
                HighwayUpgradeRequested(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        public abstract void UpdateDisplay();
        public abstract void ClearDisplay();

        #endregion

    }

}
