using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.HighwayUpgraders.ForTesting {

    public class MockSimulationControl : SimulationControlBase {

        #region events

        public event EventHandler<IntEventArgs> OnHighwayUpgraderDestructionRequested;

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            throw new NotImplementedException();
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

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
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
            if(OnHighwayUpgraderDestructionRequested != null) {
                OnHighwayUpgraderDestructionRequested(this, new IntEventArgs(highwayUpgraderID));
            }
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

        #endregion

        #endregion
        
    }

}
