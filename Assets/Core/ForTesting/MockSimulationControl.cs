using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockSimulationControl : SimulationControlBase {

        #region instance fields and properties

        public int PriorityChangeRequestsPassed;

        public int LastHighwayIDRequested;
        public int LastHighwayPriorityRequested;

        public int          FirstEndpointPermissionRequestsPassed;
        public ResourceType FirstEndpointResourceModified;
        public bool         FirstEndpointPermissionRequested;

        public int          SecondEndpointPermissionRequestsPassed;
        public ResourceType SecondEndpointResourceModified;
        public bool         SecondEndpointPermissionRequested;

        public Dictionary<int, Dictionary<ResourceType, bool>> UpkeepRequestForResourceByID =
            new Dictionary<int, Dictionary<ResourceType, bool>>();

        public int IDOfRequestedDepot;

        public int IDOfRequestedManager;

        public bool EnableHighwayBuilding;
        public int CanBuildHighwayBetweenChecksMade;
        public int HighwaysAttempted;

        public List<int> FirstEndpointsChecked = new List<int>();
        public List<int> SecondEndpointsChecked = new List<int>();

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            ++CanBuildHighwayBetweenChecksMade;
            FirstEndpointsChecked.Add(node1ID);
            SecondEndpointsChecked.Add(node2ID);
            return EnableHighwayBuilding;
        }

        public override bool CanCreateConstructionSiteOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override bool CanDestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            ++HighwaysAttempted;
        }

        public override void CreateConstructionSiteOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(int zoneID) {
            throw new NotImplementedException();
        }

        public override void DestroyHighway(int highwayID) {
            LastHighwayIDRequested = highwayID;
        }

        public override void DestroyResourceDepotOfID(int depotID) {
            IDOfRequestedDepot = depotID;
        }

        public override void DestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override void SetAscensionPermissionForSociety(int societyID, bool ascensionPermitted) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPriority(int highwayID, int newPriority) {
            ++PriorityChangeRequestsPassed;
            LastHighwayIDRequested = highwayID;
            LastHighwayPriorityRequested = newPriority;
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            LastHighwayIDRequested = highwayID;
            ++FirstEndpointPermissionRequestsPassed;
            FirstEndpointResourceModified = resourceType;
            FirstEndpointPermissionRequested = isPermitted;
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            LastHighwayIDRequested = highwayID;
            ++SecondEndpointPermissionRequestsPassed;
            SecondEndpointResourceModified = resourceType;
            SecondEndpointPermissionRequested = isPermitted;
        }

        public override void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested) {
            if(!UpkeepRequestForResourceByID.ContainsKey(highwayID)) {
                UpkeepRequestForResourceByID[highwayID] = new Dictionary<ResourceType, bool>();
            }
            UpkeepRequestForResourceByID[highwayID][resourceToChange] = isBeingRequested;
        }

        public override void DestroyHighwayManagerOfID(int managerID) {
            IDOfRequestedManager = managerID;
        }

        public override void TickSimulation(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
