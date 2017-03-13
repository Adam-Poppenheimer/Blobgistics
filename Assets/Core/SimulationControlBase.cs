using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Core {

    public abstract class SimulationControlBase : MonoBehaviour {

        #region instance methods

        public abstract bool CanConnectNodesWithHighway(int node1ID, int node2ID);
        public abstract void ConnectNodesWithHighway(int node1ID, int node2ID);

        public abstract void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPriority(int highwayID, int newPriority);

        public abstract void DestroyHighway(int highwayID);

        public abstract bool CanCreateResourceDepotConstructionSiteOnNode(int nodeID);
        public abstract void CreateResourceDepotConstructionSiteOnNode(int nodeID);

        public abstract void DestroyConstructionZone(int zoneID);

        public abstract bool CanDestroySociety(int societyID);
        public abstract void DestroySociety(int societyID);

        public abstract bool CanCreateHighwayUpgraderOnHighway(int highwayID);
        public abstract void CreateHighwayUpgraderOnHighway(int highwayID);

        public abstract void DestroyHighwayUpgrader(int highwayUpgraderID);

        public abstract void TickSimulation(float secondsPassed);

        #endregion

    }

}
