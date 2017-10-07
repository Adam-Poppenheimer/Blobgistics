using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class designed to act as a facade by which the UI can
    /// access parts of the simulation.
    /// </summary>
    /// <remarks>
    /// The intent of this class is to define all of the commands that the UI might
    /// make to the highway side of the simulation, and then obscure the 
    /// complexity of how those individual commands are implemented.
    /// </remarks>
    public abstract class HighwayControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Determines whether the nodes with the given IDs can be connected by a
        /// highway, if both exist.
        /// </summary>
        /// <param name="node1ID">The ID of the first node to consider</param>
        /// <param name="node2ID">The ID of the second node to consider</param>
        /// <returns>Whether or not the queried highway can be made</returns>
        public abstract bool CanConnectNodesWithHighway(int node1ID, int node2ID);

        /// <summary>
        /// Creates a highway between the nodes with the given IDs if they exist and
        /// such a highway can be constructed.
        /// </summary>
        /// <param name="node1ID">The ID of the first node to consider</param>
        /// <param name="node2ID">The ID of the second node to consider</param>
        public abstract void ConnectNodesWithHighway   (int node1ID, int node2ID);

        /// <summary>
        /// Sets the first endpoint pulling permission for the highway of the given ID, if it exists.
        /// </summary>
        /// <param name="highwayID">The ID of the highway to modify</param>
        /// <param name="resourceType">The ResourceType whose permission is being changed</param>
        /// <param name="isPermitted">Whether or not the ResourceType is now permitted</param>
        public abstract void SetHighwayPullingPermissionOnFirstEndpointForResource (int highwayID, ResourceType resourceType, bool isPermitted);

        /// <summary>
        /// Sets the second endpoint pulling permission for the highway of the given ID, if it exists.
        /// </summary>
        /// <param name="highwayID">The ID of the highway to modify</param>
        /// <param name="resourceType">The ResourceType whose permission is being changed</param>
        /// <param name="isPermitted">Whether or not the ResourceType is to be permitted</param>
        public abstract void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted);

        /// <summary>
        /// Sets the update requests for the highway of the given ID, if it exists.
        /// </summary>
        /// <param name="highwayID">The Id of the highway to modify</param>
        /// <param name="resourceToChange">The ResourceType whose upkeep request is being changed</param>
        /// <param name="isBeingRequested">Whether or not upkeep is to be requested</param>
        public abstract void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested);

        /// <summary>
        /// Destroys the highway of the given ID, if it exists.
        /// </summary>
        /// <param name="highwayID">The ID of the highway to destroy</param>
        public abstract void DestroyHighway(int highwayID);

        #endregion

    }

}
