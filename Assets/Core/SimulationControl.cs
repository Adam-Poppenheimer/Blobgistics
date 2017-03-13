using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;

namespace Assets.Core {

    public class SimulationControl : SimulationControlBase {

        #region instance fields and properties

        public MapGraphBase MapGraph {
            get {
                if(_mapGraph == null) {
                    throw new InvalidOperationException("MapGraph is uninitialized");
                } else {
                    return _mapGraph;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _mapGraph = value;
                }
            }
        }
        private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get {
                if(_highwayFactory == null) {
                    throw new InvalidOperationException("HighwayFactory is uninitialized");
                } else {
                    return _highwayFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _highwayFactory = value;
                }
            }
        }
        private BlobHighwayFactoryBase _highwayFactory;

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
            throw new NotImplementedException();
        }

        public override void DestroySociety(int societyID) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetHighwayPriority(int highwayID, int newPriority) {
            throw new NotImplementedException();
        }

        public override void TickSimulation(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
