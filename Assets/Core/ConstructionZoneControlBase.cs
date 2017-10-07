using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class designed to act as a facade by which the UI can
    /// access parts of the simulation related to construction zones.
    /// </summary>
    public abstract class ConstructionZoneControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Gets all construction zone projects available in the simulation.
        /// </summary>
        /// <returns>All of the construction zone projects in the simulation</returns>
        public abstract IEnumerable<ConstructionProjectUISummary> GetAllConstructionZoneProjects();

        /// <summary>
        /// Gets all of the construction projects that are valid for the node with the given ID,
        /// if such a node exists.
        /// </summary>
        /// <param name="nodeID">The Id of the node to consider</param>
        /// <returns></returns>
        public abstract IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID);

        /// <summary>
        /// Determines whether a construction node with a project of the given name can be created
        /// on a node with the given ID, if both exist.
        /// </summary>
        /// <param name="nodeID">The ID of the node in question</param>
        /// <param name="projectName">The name of the project in question</param>
        /// <returns></returns>
        public abstract bool CanCreateConstructionZoneOnNode(int nodeID, string projectName);

        /// <summary>
        /// Creates a construction zone on a node with the specified ID, with a project bearing the specified name,
        /// if both exist and the action is possible.
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="projectName"></param>
        public abstract void CreateConstructionZoneOnNode   (int nodeID, string projectName);

        /// <summary>
        /// Destroys the construction zone with the given ID if it exists.
        /// </summary>
        /// <param name="zoneID">The ID of the construction zone to destroy</param>
        public abstract void DestroyConstructionZone(int zoneID);

        #endregion

    }

}
