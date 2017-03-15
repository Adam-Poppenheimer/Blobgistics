using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.Societies;
using Assets.Depots;
using Assets.ConstructionZones;
using Assets.HighwayUpgrade;


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

        public SocietyFactoryBase SocietyFactory {
            get {
                if(_societyFactory == null) {
                    throw new InvalidOperationException("SocietyFactory is uninitialized");
                } else {
                    return _societyFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _societyFactory = value;
                }
            }
        }
        private SocietyFactoryBase _societyFactory;

        public ConstructionZoneFactoryBase ConstructionZoneFactory {
            get {
                if(_constructionZoneFactory == null) {
                    throw new InvalidOperationException("ConstructionZoneFactory is uninitialized");
                } else {
                    return _constructionZoneFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _constructionZoneFactory = value;
                }
            }
        }
        private ConstructionZoneFactoryBase _constructionZoneFactory;

        public HighwayUpgraderFactoryBase HighwayUpgraderFactory {
            get {
                if(_highwayUpgraderFactory == null) {
                    throw new InvalidOperationException("HighwayUpgraderFactory is uninitialized");
                } else {
                    return _highwayUpgraderFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _highwayUpgraderFactory = value;
                }
            }
        }
        private HighwayUpgraderFactoryBase _highwayUpgraderFactory;

        public ResourceDepotFactoryBase ResourceDepotFactory {
            get {
                if(_resourceDepotFactory == null) {
                    throw new InvalidOperationException("ResourceDepotFactory is uninitialized");
                } else {
                    return _resourceDepotFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _resourceDepotFactory = value;
                }
            }
        }
        private ResourceDepotFactoryBase _resourceDepotFactory;

        public BlobHighwayProfile UpgradedHighwayProfile{
            get { return _upgradedHighwayProfile; }
            set { _upgradedHighwayProfile = value; }
        }
        [SerializeField] private BlobHighwayProfile _upgradedHighwayProfile;

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
