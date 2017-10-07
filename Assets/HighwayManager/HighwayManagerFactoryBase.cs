using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;

namespace Assets.HighwayManager {

    /// <summary>
    /// The abstract base class for all highway manager factories. This class builds and destroys
    /// highway managers, ticks them, and deteremines which highways are served by which managers.
    /// </summary>
    public abstract class HighwayManagerFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All managers currently subscribed to the factory.
        /// </summary>
        public abstract ReadOnlyCollection<HighwayManagerBase> Managers { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the highway manager with the given ID, or null if none exists.
        /// </summary>
        /// <param name="id">The ID of the highway to get</param>
        /// <returns>The highway with the given ID, or null of non exists</returns>
        public abstract HighwayManagerBase GetHighwayManagerOfID(int id);

        /// <summary>
        /// Gets the highway manager at the given location, or null if none exists.
        /// </summary>
        /// <param name="location">The location of the highway to get</param>
        /// <returns>The highway manager at the given location, or null if none exists</returns>
        public abstract HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location);

        /// <summary>
        /// Get the highway manager tasked with serving the given highway, or null if none exists.
        /// </summary>
        /// <param name="highway">The highway to consider</param>
        /// <returns>The manager serving that highway, or null if none exists</returns>
        public abstract HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway);

        /// <summary>
        /// Gets all highways being managed by the given manager.
        /// </summary>
        /// <param name="manager">The manager to consider</param>
        /// <returns>All the highways being served by the manager</returns>
        public abstract IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager);

        /// <summary>
        /// Determines whether a highway manager can be constructed at the given location.
        /// </summary>
        /// <param name="location">The location to consider</param>
        /// <returns>Whether or not a highway manager can be built at the location</returns>
        public abstract bool               CanConstructHighwayManagerAtLocation(MapNodeBase location);

        /// <summary>
        /// Constructs a highway manager at the given location.
        /// </summary>
        /// <param name="location">The location upon which to build the highway manager</param>
        /// <returns>The highway manager created</returns>
        public abstract HighwayManagerBase ConstructHighwayManagerAtLocation   (MapNodeBase location);

        /// <summary>
        /// Unsubscribes and destroys the given highway manager.
        /// </summary>
        /// <param name="manager">The manager to destroy</param>
        public abstract void DestroyHighwayManager    (HighwayManagerBase manager);

        /// <summary>
        /// Unsubscribes the given highway manager, so that the factory no longer considers its existence.
        /// </summary>
        /// <param name="manager">The manager to unsubscribe</param>
        public abstract void UnsubscribeHighwayManager(HighwayManagerBase manager);

        /// <summary>
        /// Increments the simulation of all highway managers by the given number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds since the last tick call</param>
        public abstract void TickAllManangers(float secondsPassed);

        #endregion

    }

}
