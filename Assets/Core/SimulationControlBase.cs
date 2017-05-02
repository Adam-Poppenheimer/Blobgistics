using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.Blobs;

namespace Assets.Core {

    public abstract class SimulationControlBase : MonoBehaviour {

        #region instance methods

        public abstract bool CanConnectNodesWithHighway(int node1ID, int node2ID);
        public abstract void ConnectNodesWithHighway   (int node1ID, int node2ID);

        public abstract void SetHighwayPullingPermissionOnFirstEndpointForResource (int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPriority(int highwayID, int newPriority);
        public abstract void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested);

        public abstract void DestroyHighway(int highwayID);

        public abstract IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID);

        public abstract bool CanCreateConstructionSiteOnNode(int nodeID, string buildingName);
        public abstract void CreateConstructionSiteOnNode   (int nodeID, string buildingName);

        public abstract void DestroyConstructionZone(int zoneID);

        public abstract void SetAscensionPermissionForSociety(int societyID, bool ascensionPermitted);

        public abstract bool CanDestroySociety(int societyID);
        public abstract void DestroySociety   (int societyID);

        public abstract void DestroyResourceDepotOfID(int depotID);

        public abstract void DestroyHighwayManagerOfID(int managerID);

        public abstract void TickSimulation(float secondsPassed);

        #endregion

    }

}
