using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways {

    public abstract class BlobHighwaySummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        public abstract BlobHighwayUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        public event EventHandler<IntEventArgs> PriorityChanged;

        public event EventHandler<ResourcePermissionEventArgs> FirstEndpointResourcePermissionChanged;
        public event EventHandler<ResourcePermissionEventArgs> SecondEndpointResourcePermissionChanged;

        public event EventHandler<UpkeepRequestEventArgs> ResourceRequestedForUpkeep;

        protected void RaisePriorityChanged(int newPriority) { RaiseEvent(PriorityChanged, new IntEventArgs(newPriority)); }

        protected void RaiseFirstEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(FirstEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        protected void RaiseSecondEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(SecondEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        protected void RaiseResourceRequestedForUpkeep(ResourceType type, bool isBeingRequested) {
            RaiseEvent(ResourceRequestedForUpkeep, new UpkeepRequestEventArgs(type, isBeingRequested));
        }

        #endregion

    }

}
