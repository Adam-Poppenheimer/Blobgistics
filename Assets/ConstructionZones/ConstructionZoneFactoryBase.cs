using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ConstructionZones {

    /// <summary>
    /// The abstract base class for all construction zone factories. Handles creation and destruction
    /// of construction zones, keeps a record of available construction projects, and maintains
    /// construction zone placement restrictions.
    /// </summary>
    public abstract class ConstructionZoneFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All the construction zones created by this factory.
        /// </summary>
        public abstract ReadOnlyCollection<ConstructionZoneBase> ConstructionZones { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Retrieves the construction zone with the given ID.
        /// </summary>
        /// <param name="id">The id of the construction zone to get</param>
        /// <returns>The construction zone with the given ID, or null if none exists</returns>
        public abstract ConstructionZoneBase GetConstructionZoneOfID(int id);

        /// <summary>
        /// Determines whether or not a construction zone already exists at the given location.
        /// </summary>
        /// <param name="location">The location to check</param>
        /// <returns>Whether there is a construction zone there</returns>
        public abstract bool                 HasConstructionZoneAtLocation(MapNodeBase location);

        /// <summary>
        /// Finds the construction zone at the given location.
        /// </summary>
        /// <param name="location">The location to check</param>
        /// <returns>The construction zone at that location</returns>
        public abstract ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location);

        /// <summary>
        /// Determines whether or not a construction zone with a given project can be built at the given location.
        /// </summary>
        /// <param name="location">The desired location for the construction zone</param>
        /// <param name="project">The desired project for the construction zone</param>
        /// <returns>Whether or not the desired construction zone is valid</returns>
        public abstract bool                 CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project);

        /// <summary>
        /// Builds a construction zone with the given project at the given location.
        /// </summary>
        /// <param name="location">The location to build the construction zone</param>
        /// <param name="project">The active project that construction zone will contain</param>
        /// <returns>The newly created construction zone</returns>
        public abstract ConstructionZoneBase BuildConstructionZone   (MapNodeBase location, ConstructionProjectBase project);

        /// <summary>
        /// Unsubscribes and destroys the speicifed construction zone.
        /// </summary>
        /// <param name="constructionZone">The construction zone to destroy</param>
        public abstract void DestroyConstructionZone(ConstructionZoneBase constructionZone);

        /// <summary>
        /// Unsubscribes the given construction zone, so that the factory no longer considers its existence.
        /// </summary>
        /// <param name="constructionZone">The construction zone to unsubscribe</param>
        public abstract void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone);

        /// <summary>
        /// Attempts to acquire a project with a given name.
        /// </summary>
        /// <param name="projectName">The name of the project to get</param>
        /// <param name="project">The project of that name that the method found, or null if it found none</param>
        /// <returns>A value representing whether the operation was successful or not</returns>
        public abstract bool TryGetProjectOfName(string projectName, out ConstructionProjectBase project);

        /// <summary>
        /// Gets all projects that are available for construction.
        /// </summary>
        /// <returns>All of the construction projects this factory recognizes</returns>
        public abstract IEnumerable<ConstructionProjectBase> GetAvailableProjects();

        #endregion

    }

}
