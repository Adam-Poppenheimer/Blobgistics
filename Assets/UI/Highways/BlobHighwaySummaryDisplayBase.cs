using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;
using Assets.Util;

namespace Assets.UI.Highways {

    /// <summary>
    /// The abstract base class for the highway summary display, which gives players information
    /// and commands relating to highways.
    /// </summary>
    public abstract class BlobHighwaySummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The highway summary being displayed.
        /// </summary>
        public abstract BlobHighwayUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires when the player has requested a change in the resource
        /// permissions for the first endpoint of the highway.
        /// </summary>
        public event EventHandler<ResourcePermissionEventArgs> FirstEndpointResourcePermissionChanged;

        /// <summary>
        /// Fires when the player has requested a change in the resource
        /// permissions for the first endpoint of the highway.
        /// </summary>
        public event EventHandler<ResourcePermissionEventArgs> SecondEndpointResourcePermissionChanged;

        /// <summary>
        /// Fires when the player has requested a change in the upkeep requests of the highway.
        /// </summary>
        public event EventHandler<UpkeepRequestEventArgs> ResourceRequestedForUpkeep;

        /// <summary>
        /// Fires the FirstEndpointResourcePermissionChanged event.
        /// </summary>
        /// <param name="typeChanged">The ResourceType being changed</param>
        /// <param name="isNowPermitted">Whether it is now permitted</param>
        protected void RaiseFirstEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(FirstEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        /// <summary>
        /// Fires the SecondEndpointResourcePermissionChanged event.
        /// </summary>
        /// <param name="typeChanged">The ResourceType being changed</param>
        /// <param name="isNowPermitted">Whether it is now permitted</param>
        protected void RaiseSecondEndpointPermissionChanged(ResourceType typeChanged, bool isNowPermitted) {
            RaiseEvent(SecondEndpointResourcePermissionChanged, new ResourcePermissionEventArgs(typeChanged, isNowPermitted));
        }

        /// <summary>
        /// Fires the ResourceRequestedForUpkeep event.
        /// </summary>
        /// <param name="typeChanged">The ResourceType being changed</param>
        /// <param name="isBeingRequested">Whether it is now being requested</param>
        protected void RaiseResourceRequestedForUpkeep(ResourceType typeChanged, bool isBeingRequested) {
            RaiseEvent(ResourceRequestedForUpkeep, new UpkeepRequestEventArgs(typeChanged, isBeingRequested));
        }

        #endregion

    }

}
