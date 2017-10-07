using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;
using Assets.Core;

namespace Assets.Highways {

    /// <summary>
    /// The abstract base class for all blob highways, gameplay elements that transfer blobs
    /// between map nodes.
    /// </summary>
    public abstract class BlobHighwayBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// A BlobHighwayBase-unique ID for this object.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The profile that contains information about how and when the blob highway should transport blobs.
        /// </summary>
        public abstract BlobHighwayProfile Profile { get; set; }

        /// <summary>
        /// The factory that's subscribed this highway.
        /// </summary>
        public abstract BlobHighwayFactoryBase  ParentFactory { get; set; }

        /// <summary>
        /// The UIControl this highway should send UI events to.
        /// </summary>
        public abstract UIControlBase           UIControl     { get; set; }

        /// <summary>
        /// The blob factory this highway should use to destroy blobs.
        /// </summary>
        public abstract ResourceBlobFactoryBase BlobFactory   { get; set; }

        /// <summary>
        /// The first node this highway connects to.
        /// </summary>
        public abstract MapNodeBase FirstEndpoint  { get; } 

        /// <summary>
        /// The second node this highway connects to.
        /// </summary>
        public abstract MapNodeBase SecondEndpoint { get; }

        /// <summary>
        /// All contents pulled from the first endpoint that are traveling to the second endpoint.
        /// </summary>
        public abstract ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint  { get; }

        /// <summary>
        /// All contents pulled from the second endpoint that are traveling to the first endpoint.
        /// </summary>
        public abstract ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint { get; }

        /// <summary>
        /// The priority for blob distribution that this highway has.
        /// </summary>
        /// <remarks>
        /// Priority is a holdover from a previous design and should be removed in the next refactor of
        /// the Highways namespace.
        /// </remarks>
        public abstract int Priority { get; set; }

        /// <summary>
        /// A ratio, by policy never less than 1, that approximately multiplies the speed at which
        /// this highway transfers resources.
        /// </summary>
        public abstract float Efficiency { get; set; }

        /// <summary>
        /// The cooldown between blob pull attempts. This sets the maximum rate at which this highway
        /// can transfer resources.
        /// </summary>
        public abstract float BlobPullCooldownInSeconds { get; }

        #endregion

        #region instance methods

        #region from Object

        /// <inheritdoc/>
        public override string ToString() {
            return string.Format("Highway {0} [{1} <--> {2}]", ID, FirstEndpoint, SecondEndpoint);
        }

        #endregion

        /// <summary>
        /// Modifies the endpoints of the highway.
        /// </summary>
        /// <param name="firstEndpoint">The new first endpoint</param>
        /// <param name="secondEndpoint">The new second endpoint</param>
        public abstract void SetEndpoints(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);


        /// <summary>
        /// Gets whether the highway permits pulls of the given resource type from the first endpoint.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <returns>Whether the pull is permitted</returns>
        public abstract bool GetPullingPermissionForFirstEndpoint(ResourceType type);


        /// <summary>
        /// Sets whether the highway permits pulls of the given resource type from the first endpoint.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <param name="isPermitted">Whether the pull will be permitted</param>
        public abstract void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted);


        /// <summary>
        /// Gets whether the highway permits pulls of the given resource type from the second endpoint.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <returns>Whether the pull is permitted</returns>
        public abstract bool GetPullingPermissionForSecondEndpoint(ResourceType type);

        /// <summary>
        /// Sets whether the highway permits pulls of the given resource type from the second endpoint.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <param name="isPermitted">Whether the pull will be permitted</param>
        public abstract void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted);


        /// <summary>
        /// Whether a resource can currently be pulled from the first endpoint.
        /// </summary>
        /// <remarks>
        /// Whether a resource can be pulled from an endpoint relies on this highway and the
        /// state of both of its endpoints.
        /// </remarks>
        /// <returns>Whether a pull can be executed from the first endpoint</returns>
        public abstract bool CanPullFromFirstEndpoint();

        /// <summary>
        /// Pulls a resource from the first endpoint and sets it on its journey
        /// to the second endpoint.
        /// </summary>
        public abstract void PullFromFirstEndpoint();


        /// <summary>
        /// Whether a resource can currently be pulled from the first endpoint.
        /// </summary>
        /// <remarks>
        /// Whether a resource can be pulled from an endpoint relies on this highway and the
        /// state of both of its endpoints.
        /// </remarks>
        /// <returns>Whether a pull can be executed from the first endpoint</returns>
        public abstract bool CanPullFromSecondEndpoint();

        /// <summary>
        /// Pulls a resource from the second endpoint and sets it on its journey
        /// to the first endpoint.
        /// </summary>
        public abstract void PullFromSecondEndpoint();


        /// <summary>
        /// Gets whether this highway is requesting upkeep for the given ResourceType.
        /// </summary>
        /// <param name="type">The ResourceType to be considered</param>
        /// <returns>Whether the upkeep is being requested</returns>
        public abstract bool GetUpkeepRequestedForResource(ResourceType type);

        /// <summary>
        /// Sets whether this highway is requesting upkeep for the given ResourceType.
        /// </summary>
        /// <param name="type">The ResourceType to be changed</param>
        /// <param name="isBeingRequested">Whether the upkeep is to be requested</param>
        public abstract void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested);

        /// <summary>
        /// Clears the highway of all contents.
        /// </summary>
        public abstract void Clear();

        #endregion

    }

}
