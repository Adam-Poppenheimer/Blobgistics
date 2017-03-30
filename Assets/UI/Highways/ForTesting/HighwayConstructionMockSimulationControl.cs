using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Core;
using Assets.Map;

namespace Assets.UI.Highways.ForTesting {

    public class HighwayConstructionMockSimulationControl : SimulationControlBase {

        #region instance fields and properties

        public int ChecksMade { get; set; }
        public bool EnableHighwayBuilding { get; set; }
        public int HighwaysAttempted { get; set; }

        public List<int> FirstEndpointsChecked = new List<int>();
        public List<int> SecondEndpointsChecked = new List<int>();

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            ++ChecksMade;
            FirstEndpointsChecked.Add(node1ID);
            SecondEndpointsChecked.Add(node2ID);
            return EnableHighwayBuilding;
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            ++HighwaysAttempted;
        }

        public override bool CanCreateHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override bool CanCreateResourceDepotConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override bool CanDestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override void CreateHighwayUpgraderOnHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void CreateResourceDepotConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(int zoneID) {
            throw new NotImplementedException();
        }

        public override void DestroyHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void DestroyHighwayUpgrader(int highwayUpgraderID) {
            throw new NotImplementedException();
        }

        public override void DestroySociety(int societyID) {
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

        public override void TickSimulation(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override bool CanCreateFarmlandConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override bool CanCreateVillageConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override void CreateFarmlandConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override void CreateVillageConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
