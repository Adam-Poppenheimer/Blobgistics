using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.ConstructionZones;
using Assets.BlobDistributors;
using Assets.HighwayManager;
using Assets.Generator;

namespace Assets.Core {

    public class SimulationControl : SimulationControlBase {

        #region static fields and properties

        private static string HighwayIDErrorMessage = "There exists no Highway with ID {0}";
        private static string SocietyIDErrorMessage = "There exists no Society with ID {0}";
        private static string ConstructionZoneIDErrorMessage = "There exists no ConstructionZone with ID {0}";
        private static string MapNodeIDErrorMessage = "There exists no MapNode with ID {0}";
        private static string DepotIDErrorMessage = "There exists no ResourceDepot with ID {0}";
        private static string HighwayManagerIDErrorMessage = "There exists no HighwayManager with ID {0}";

        #endregion

        #region instance fields and properties

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        public ConstructionZoneFactoryBase ConstructionZoneFactory {
            get { return _constructionZoneFactory; }
            set { _constructionZoneFactory = value; }
        }
        [SerializeField] private ConstructionZoneFactoryBase _constructionZoneFactory;

        public ResourceDepotFactoryBase ResourceDepotFactory {
            get { return _resourceDepotFactory; }
            set { _resourceDepotFactory = value; }
        }
        [SerializeField] private ResourceDepotFactoryBase _resourceDepotFactory;

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        public BlobDistributorBase BlobDistributor{
            get { return _blobDistributor; }
            set { _blobDistributor = value; }
        }
        [SerializeField] private BlobDistributorBase _blobDistributor;

        public ResourceGeneratorFactory GeneratorFactory {
            get { return _generatorFactory; }
            set { _generatorFactory = value; }
        }
        [SerializeField] private ResourceGeneratorFactory _generatorFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            TickSimulation(Time.deltaTime);
        }

        #endregion

        #region from SimulationControlBase

        public override bool CanConnectNodesWithHighway(int node1ID, int node2ID) {
            var node1 = MapGraph.GetNodeOfID(node1ID);
            var node2 = MapGraph.GetNodeOfID(node2ID);

            if(node1 == null) {
               Debug.LogErrorFormat(MapNodeIDErrorMessage, node1ID);
                return false;
            }else if(node2 == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, node2ID);
                return false;
            }else {
                return HighwayFactory.CanConstructHighwayBetween(node1, node2);
            }
        }

        public override IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID) {
            var retval = new List<ConstructionProjectUISummary>();

            var node = MapGraph.GetNodeOfID(nodeID);
            if(node != null) {
                if( ConstructionZoneFactory.HasConstructionZoneAtLocation(node) ||
                    ResourceDepotFactory.HasDepotAtLocation(node) ||
                    SocietyFactory.HasSocietyAtLocation(node)
                ){
                    return new List<ConstructionProjectUISummary>();
                }

                foreach(var project in ConstructionZoneFactory.GetAvailableProjects()) {
                    if(ConstructionZoneFactory.CanBuildConstructionZone(node, project)) {
                        retval.Add(new ConstructionProjectUISummary(project));
                    }
                }
            }else {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
            }
            return retval;
        }

        public override bool CanCreateConstructionSiteOnNode(int nodeID, string projectName) {
            var node = MapGraph.GetNodeOfID(nodeID);
            if(node != null) {
                ConstructionProjectBase project;
                return (
                    ConstructionZoneFactory.TryGetProjectOfName(projectName, out project) &&
                    !SocietyFactory.HasSocietyAtLocation(node) &&
                    !ResourceDepotFactory.HasDepotAtLocation(node) &&
                    HighwayManagerFactory.GetHighwayManagerAtLocation(node) == null &&
                    ConstructionZoneFactory.CanBuildConstructionZone(node, project)
                );
            }else {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
                return false;
            }
        }

        public override void CreateConstructionSiteOnNode(int nodeID, string projectName) {
            var node = MapGraph.GetNodeOfID(nodeID);

            if(node == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
            }else if(!CanCreateConstructionSiteOnNode(nodeID, projectName)) {
                Debug.LogErrorFormat("A ConstructionZone for a building of name {0} cannot be built on node {1}",
                    projectName, node);
            }else {
                ConstructionProjectBase project;
                ConstructionZoneFactory.TryGetProjectOfName(projectName, out project);
                ConstructionZoneFactory.BuildConstructionZone(node, project);
            }
        }

        public override bool CanDestroySociety(int societyID) {
            var society = SocietyFactory.GetSocietyOfID(societyID);
            if(society != null) {
                return society.CurrentComplexity == society.ActiveComplexityLadder.GetStartingComplexity();
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
                return false;
            }
        }

        public override void ConnectNodesWithHighway(int node1ID, int node2ID) {
            var node1 = MapGraph.GetNodeOfID(node1ID);
            var node2 = MapGraph.GetNodeOfID(node2ID);

            if(node1 == null) {
               Debug.LogErrorFormat(MapNodeIDErrorMessage, node1ID);
            }else if(node2 == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, node2ID);
            }else if(!HighwayFactory.CanConstructHighwayBetween(node1, node2)) {
                Debug.LogErrorFormat("A BlobHighway cannot be placed between node {0} and node {1}", node1, node2);
            }else {
                HighwayFactory.ConstructHighwayBetween(node1, node2);
            }
        }

        public override void DestroyConstructionZone(int zoneID) {
            var zoneToDestroy = ConstructionZoneFactory.GetConstructionZoneOfID(zoneID);
            if(zoneToDestroy != null) {
                ConstructionZoneFactory.DestroyConstructionZone(zoneToDestroy);
            }else {
                Debug.LogErrorFormat(ConstructionZoneIDErrorMessage, zoneID);
            }
        }

        public override void DestroyHighway(int highwayID) {
            var highwayToDestroy = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToDestroy != null) {
                HighwayFactory.DestroyHighway(highwayToDestroy);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        public override void DestroySociety(int societyID) {
            var societyToDestroy = SocietyFactory.GetSocietyOfID(societyID);
            if(societyToDestroy != null) {
                SocietyFactory.DestroySociety(societyToDestroy);
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
            }
        }

        public override void SetHighwayPullingPermissionOnFirstEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.SetPullingPermissionForFirstEndpoint(resourceType, isPermitted);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        public override void SetHighwayPullingPermissionOnSecondEndpointForResource(int highwayID, ResourceType resourceType, bool isPermitted) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.SetPullingPermissionForSecondEndpoint(resourceType, isPermitted);
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        public override void SetHighwayPriority(int highwayID, int newPriority) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                highwayToChange.Priority = newPriority;
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        public override void SetHighwayUpkeepRequest(int highwayID, ResourceType resourceToChange, bool isBeingRequested) {
            var highwayToChange = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToChange != null) {
                switch(resourceToChange) {
                    case ResourceType.Food:   highwayToChange.IsRequestingFood   = isBeingRequested; break;
                    case ResourceType.Yellow: highwayToChange.IsRequestingYellow = isBeingRequested; break;
                    case ResourceType.White:  highwayToChange.IsRequestingWhite  = isBeingRequested; break;
                    case ResourceType.Blue:   highwayToChange.IsRequestingBlue   = isBeingRequested; break;
                }
            }else {
                Debug.LogErrorFormat(HighwayIDErrorMessage, highwayID);
            }
        }

        public override void TickSimulation(float secondsPassed) {
            if(SocietyFactory        != null) SocietyFactory.TickSocieties          (secondsPassed);
            if(BlobDistributor       != null) BlobDistributor.Tick                  (secondsPassed);
            if(BlobFactory           != null) BlobFactory.TickAllBlobs              (secondsPassed);
            if(HighwayManagerFactory != null) HighwayManagerFactory.TickAllManangers(secondsPassed);
            if(GeneratorFactory      != null) GeneratorFactory.TickAllGenerators    (secondsPassed);
        }

        public override void SetAscensionPermissionForSociety(int societyID, bool ascensionPermitted) {
            var societyToChange = SocietyFactory.GetSocietyOfID(societyID);
            if(societyToChange != null) {
                societyToChange.AscensionIsPermitted = ascensionPermitted;
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
            }
        }

        public override void DestroyResourceDepotOfID(int depotID) {
            var depotToDestroy = ResourceDepotFactory.GetDepotOfID(depotID);
            if(depotToDestroy != null) {
                ResourceDepotFactory.DestroyDepot(depotToDestroy);
            }else {
                Debug.LogErrorFormat(DepotIDErrorMessage, depotID);
            }
        }

        public override void DestroyHighwayManagerOfID(int managerID) {
            var managerToDestroy = HighwayManagerFactory.GetHighwayManagerOfID(managerID);
            if(managerToDestroy != null) {
                HighwayManagerFactory.DestroyHighwayManager(managerToDestroy);
            }else {
                Debug.LogErrorFormat(HighwayManagerIDErrorMessage, managerID);
            }
        }

        #endregion

        #endregion

    }

}
