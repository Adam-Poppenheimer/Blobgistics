using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways.ForTesting {
    public class HighwayDisplayMockSimulationControl : SimulationControlBase {

        public int LastIDRequested = 0;
        
        public int PriorityChangeRequestsPassed = 0;
        public int LastPriorityRequested = 0;

        public int FirstEndpointPermissionRequestsPassed = 0;
        public int SecondEndpointPermissionRequestsPassed = 0;

        public bool LastFirstEndpointPermissionRequested = false;
        public bool LastSecondEndpointPermissionRequested = false;

        public ResourceType LastFirstEndpointResourceTypeChangeRequested = ResourceType.Blue;
        public ResourceType LastSecondEndpointResourceTypeChangeRequested = ResourceType.Blue;

        public bool AcceptsUpgradeRequests = false;

        #region instance fields and properties



        #endregion

        #region events

        public event EventHandler<IntEventArgs> HighwayUpgradeRequested;

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            throw new NotImplementedException();
        }

        public override bool CanCreateHighwayUpgraderOnHighway(int highwayID) {
            return AcceptsUpgradeRequests;
        }

        public override bool CanCreateResourceDepotConstructionSiteOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        public override bool CanDestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            throw new NotImplementedException();
        }

        public override void CreateHighwayUpgraderOnHighway(int highwayID) {
            if(HighwayUpgradeRequested != null) {
                HighwayUpgradeRequested(this, new IntEventArgs(highwayID));
            }
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
            ++PriorityChangeRequestsPassed;
            LastIDRequested = highwayID;
            LastPriorityRequested = newPriority;
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            ++FirstEndpointPermissionRequestsPassed;
            LastIDRequested = highwayID;
            LastFirstEndpointResourceTypeChangeRequested = resourceType;
            LastFirstEndpointPermissionRequested = isPermitted;
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            ++SecondEndpointPermissionRequestsPassed;
            LastIDRequested = highwayID;
            LastSecondEndpointResourceTypeChangeRequested = resourceType;
            LastSecondEndpointPermissionRequested = isPermitted;
        }

        public override void TickSimulation(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }
}
