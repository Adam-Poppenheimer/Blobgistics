using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Highways;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of HighwayControlBase. It acts as a facade by which the UI can
    /// access parts of the simulation.
    /// </summary>
    public class HighwayControl : HighwayControlBase {

        #region static fields and properties

        private static string HighwayIDErrorMessage = "There exists no Highway with ID {0}";
        private static string MapNodeIDErrorMessage = "There exists no MapNode with ID {0}";

        #endregion

        #region instance fields and properties

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        #endregion

        #region instance methods

        #region from HighwayControlBase

        /// <inheritdoc/>
        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            var node1 = MapGraph.GetNodeOfID(node1ID);
            var node2 = MapGraph.GetNodeOfID(node2ID);

            if(node1 == null) {
               Debug.LogErrorFormat(MapNodeIDErrorMessage, node1ID);
                return false;
            }else if(node2 == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, node2ID);
                return false;
            }else {
                return MapGraph.GetEdge(node1, node2) != null && HighwayFactory.CanConstructHighwayBetween(node1, node2);
            }
        }

        /// <inheritdoc/>
        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            var node1 = MapGraph.GetNodeOfID(node1ID);
            var node2 = MapGraph.GetNodeOfID(node2ID);

            if(node1 == null) {
               Debug.LogErrorFormat(MapNodeIDErrorMessage, node1ID);
            }else if(node2 == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, node2ID);
            }else if(!HighwayFactory.CanConstructHighwayBetween(node1, node2)) {
                Debug.LogErrorFormat("A BlobHighway cannot be placed between node {0} and node {1}", node1, node2);
            }else {
                HighwayFactory.ConstructHighwayBetween(node1, node2);
            }
        }

        /// <inheritdoc/>
        public override void DestroyHighway(int highwayID) {
            var highwayToDestroy = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToDestroy != null) {
                HighwayFactory.DestroyHighway(highwayToDestroy);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        /// <inheritdoc/>
        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.SetPullingPermissionForFirstEndpoint(resourceType, isPermitted);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        /// <inheritdoc/>
        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.SetPullingPermissionForSecondEndpoint(resourceType, isPermitted);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        /// <inheritdoc/>
        public override void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.SetUpkeepRequestedForResource(resourceToChange, isBeingRequested);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        #endregion

        #endregion

    }

}
