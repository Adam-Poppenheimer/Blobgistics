using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ResourceDepots {

    /// <summary>
    /// The abstract base class for all resource depot factories.
    /// </summary>
    /// <remarks>
    /// The abscence of a Subscribe method may be an error. Consider investigating.
    /// </remarks>
    public abstract class ResourceDepotFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All the resource depots subscribed to this factory.
        /// </summary>
        public abstract ReadOnlyCollection<ResourceDepotBase> ResourceDepots { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the depot of the given ID, if it exists.
        /// </summary>
        /// <param name="id">The ID of the depot to retrieve</param>
        /// <returns>The depot with the given ID, or null if none exists</returns>
        public abstract ResourceDepotBase GetDepotOfID(int id);

        /// <summary>
        /// Determines where a resource depot exists at the given location.
        /// </summary>
        /// <param name="location">The location to consider</param>
        /// <returns>Whether there is a depot at the location</returns>
        public abstract bool HasDepotAtLocation(MapNodeBase location);

        /// <summary>
        /// Gets the resource depot at the given location.
        /// </summary>
        /// <param name="location">The location to consider</param>
        /// <returns>The depot at that location</returns>
        public abstract ResourceDepotBase GetDepotAtLocation(MapNodeBase location);

        /// <summary>
        /// Constructs a depot at the given location.
        /// </summary>
        /// <param name="location">The location to build</param>
        /// <returns>The new depot constructed at that location</returns>
        public abstract ResourceDepotBase ConstructDepotAt(MapNodeBase location);

        /// <summary>
        /// Unsubscribes and destroys the given depot.
        /// </summary>
        /// <param name="depot">The depot to destroy</param>
        public abstract void DestroyDepot(ResourceDepotBase depot);

        /// <summary>
        /// Unsubscribes the given depot, removing it from this factory's consideration.
        /// </summary>
        /// <param name="depot">The depot to unsubscribe</param>
        public abstract void UnsubscribeDepot(ResourceDepotBase depot);

        #endregion

    }

}
