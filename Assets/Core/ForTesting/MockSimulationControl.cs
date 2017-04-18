using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockSimulationControl : SimulationControlBase {

        #region instance fields and properties

        public int IDOfRequestedDepot;

        public int IDOfRequestedManager;

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            throw new NotImplementedException();
        }

        public override bool CanCreateConstructionSiteOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override bool CanCreateHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override bool CanDestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            throw new NotImplementedException();
        }

        public override void CreateConstructionSiteOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override void CreateHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(int zoneID) {
            throw new NotImplementedException();
        }

        public override void DestroyHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void DestroyHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
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

        public override bool HasHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void SetAscensionPermissionForSociety(int societyID, bool ascensionPermitted) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPriority(int highwayID, int newPriority) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            throw new NotImplementedException();
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
