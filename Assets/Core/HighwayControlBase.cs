using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;

namespace Assets.Core {

    public abstract class HighwayControlBase : MonoBehaviour {

        #region instance methods

        public abstract bool CanConnectNodesWithHighway(int node1ID, int node2ID);
        public abstract void ConnectNodesWithHighway   (int node1ID, int node2ID);

        public abstract void SetHighwayPullingPermissionOnFirstEndpointForResource (int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted);
        public abstract void SetHighwayPriority(int highwayID, int newPriority);
        public abstract void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested);

        public abstract void DestroyHighway(int highwayID);

        #endregion

    }

}
