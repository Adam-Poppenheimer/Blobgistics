using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;

namespace Assets.Core.ForTesting {

    public class MockHighwayControl : HighwayControlBase {

        #region instance fields and properties

        public bool HighwayBuildingIsPermitted = false;

        #endregion

        #region events

        public event Action<int, int> CanConnectNodesWithHighwayCalled;
        public event Action<int, int> ConnectNodesWithHighwayCalled;
        public event Action<int, int> SetHighwayPriorityCalled;
        public event Action<int, ResourceType, bool> SetHighwayPullingPermissionOnFirstEndpointForResourceCalled;
        public event Action<int, ResourceType, bool> SetHighwayPullingPermissionOnSecondEndpointForResourceCalled;
        public event Action<int, ResourceType, bool> SetHighwayUpkeepRequestCalled;

        #endregion

        #region instance methods

        #region from HighwayControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            if(CanConnectNodesWithHighwayCalled != null) {
                CanConnectNodesWithHighwayCalled(node1ID, node2ID);
            }
            return HighwayBuildingIsPermitted;
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            if(ConnectNodesWithHighwayCalled != null) {
                ConnectNodesWithHighwayCalled(node1ID, node2ID);
            }
        }

        public override void DestroyHighway(int highwayID) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPriority(int highwayID, int newPriority) {
            if(SetHighwayPriorityCalled != null) {
                SetHighwayPriorityCalled(highwayID, newPriority);
            }
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            if(SetHighwayPullingPermissionOnFirstEndpointForResourceCalled != null) {
                SetHighwayPullingPermissionOnFirstEndpointForResourceCalled(highwayID, resourceType, isPermitted);
            }
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            if(SetHighwayPullingPermissionOnSecondEndpointForResourceCalled != null) {
                SetHighwayPullingPermissionOnSecondEndpointForResourceCalled(highwayID, resourceType, isPermitted);
            }
        }

        public override void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested) {
            if(SetHighwayUpkeepRequestCalled != null) {
                SetHighwayUpkeepRequestCalled(highwayID, resourceToChange, isBeingRequested);
            }
        }

        #endregion

        #endregion
        
    }

}
