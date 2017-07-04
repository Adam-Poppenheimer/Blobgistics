using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.ConstructionZones;
using Assets.HighwayManager;
using Assets.Highways;
using Assets.ResourceDepots;
using Assets.Societies;
using Assets.Scoring;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    public class SessionManager : SessionManagerBase {

        #region instance fields and properties

        public override SerializableSession CurrentSession {
            get { return _currentSession; }
            set { _currentSession = value; }
        }
        [SerializeField] private SerializableSession _currentSession;

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public ConstructionZoneFactoryBase ConstructionZoneFactory {
            get { return _constructionZoneFactory; }
            set { _constructionZoneFactory = value; }
        }
        [SerializeField] private ConstructionZoneFactoryBase _constructionZoneFactory;

        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        public ResourceDepotFactoryBase ResourceDepotFactory {
            get { return _resourceDepotFactory; }
            set { _resourceDepotFactory = value; }
        }
        [SerializeField] private ResourceDepotFactoryBase _resourceDepotFactory;

        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public FileSystemLiaison FileSystemLiaison {
            get { return _fileSystemLiaison; }
            set { _fileSystemLiaison = value; }
        }
        [SerializeField] private FileSystemLiaison _fileSystemLiaison;

        public VictoryManagerBase VictoryManager {
            get { return _victoryManager; }
            set { _victoryManager = value; }
        }
        [SerializeField] private VictoryManagerBase _victoryManager;

        public TerrainGridBase TerrainGrid {
            get { return _terrainGrid; }
            set { _terrainGrid = value; }
        }
        [SerializeField] private TerrainGridBase _terrainGrid;

        public Camera MainCamera {
            get { return _mainCamera; }
            set { _mainCamera = value; }
        }
        [SerializeField] private Camera _mainCamera;

        #endregion

        #region instance methods

        public override void PushRuntimeIntoCurrentSession() {
            if(CurrentSession == null) {
                throw new InvalidOperationException("CurrentSession must be initialized in order to push the session into it");
            }
            CurrentSession.Clear();

            PushMapGraphIntoSession         (CurrentSession);
            PushHighwaysIntoSession         (CurrentSession);
            PushConstructionZonesIntoSession(CurrentSession);
            PushHighwayManagersIntoSession  (CurrentSession);
            PushResourceDepotsIntoSession   (CurrentSession);
            PushSocietiesIntoSession        (CurrentSession);

            CurrentSession.TerrainData = new SerializableTerrainData(TerrainGrid);
            CurrentSession.CameraData = new SerializableCameraData(MainCamera);
        }

        public override void PullRuntimeFromCurrentSession() {
            if(CurrentSession == null) {
                throw new InvalidOperationException("CurrentSession must be initialized in order to pull the session from it");
            }
            ClearRuntime();

            if(VictoryManager != null) {
                VictoryManager.TierOneSocietiesToWin   = CurrentSession.TierOneSocietiesToWin;
                VictoryManager.TierTwoSocietiesToWin   = CurrentSession.TierTwoSocietiesToWin;
                VictoryManager.TierThreeSocietiesToWin = CurrentSession.TierThreeSocietiesToWin;
                VictoryManager.TierFourSocietiesToWin  = CurrentSession.TierFourSocietiesToWin;
            }

            var mapNodeIDMapping = LoadMapNodes(CurrentSession);
            var mapEdgeIDMapping = LoadMapEdges(CurrentSession, mapNodeIDMapping);

            LoadNeighborhoods    (CurrentSession, mapNodeIDMapping, mapEdgeIDMapping);
            LoadHighways         (CurrentSession, mapNodeIDMapping);
            LoadConstructionZones(CurrentSession, mapNodeIDMapping);
            LoadHighwayManagers  (CurrentSession, mapNodeIDMapping);
            LoadResourceDepots   (CurrentSession, mapNodeIDMapping);
            LoadSocieties        (CurrentSession, mapNodeIDMapping);
            LoadTerrainData      (CurrentSession);
            LoadCameraData       (CurrentSession);
        }

        private void PushMapGraphIntoSession(SerializableSession session) {
            foreach(var node in MapGraph.Nodes) {
                session.MapNodes.Add(new SerializableMapNodeData(node));
            }
            foreach(var edge in MapGraph.Edges) {
                session.MapEdges.Add(new SerializableMapEdgeData(edge));
            }
            foreach(var neighborhood in MapGraph.transform.GetComponentsInChildren<Neighborhood>()) {
                session.Neighborhoods.Add(new SerializableNeighborhoodData(neighborhood));
            }
        }

        private void PushHighwaysIntoSession(SerializableSession session) {
            foreach(var highway in HighwayFactory.Highways) {
                session.Highways.Add(new SerializableHighwayData(highway));
            }
        }

        private void PushConstructionZonesIntoSession(SerializableSession session) {
            foreach(var zone in ConstructionZoneFactory.ConstructionZones) {
                session.ConstructionZones.Add(new SerializableConstructionZoneData(zone));
            }
        }

        private void PushHighwayManagersIntoSession(SerializableSession session) {
            foreach(var manager in HighwayManagerFactory.Managers) {
                session.HighwayManagers.Add(new SerializableHighwayManagerData(manager));
            }
        }

        private void PushResourceDepotsIntoSession(SerializableSession session) {
            foreach(var depot in ResourceDepotFactory.ResourceDepots) {
                session.ResourceDepots.Add(new SerializableResourceDepotData(depot));
            }
        }

        private void PushSocietiesIntoSession(SerializableSession session) {
            foreach(var society in SocietyFactory.Societies) {
                session.Societies.Add(new SerializableSocietyData(society));
            }
        }        

        private void ClearRuntime() {
            foreach(var node in new List<MapNodeBase>(MapGraph.Nodes)) {
                MapGraph.DestroyNode(node);
            }
            foreach(var mapEdge in new List<MapEdgeBase>(MapGraph.Edges)) {
                MapGraph.DestroyMapEdge(mapEdge);
            }
            foreach(var highway in new List<BlobHighwayBase>(HighwayFactory.Highways)) {
                HighwayFactory.DestroyHighway(highway);
            }
            foreach(var constructionZone in new List<ConstructionZoneBase>(ConstructionZoneFactory.ConstructionZones)) {
                ConstructionZoneFactory.DestroyConstructionZone(constructionZone);
            }
            foreach(var highwayManager in new List<HighwayManagerBase>(HighwayManagerFactory.Managers)) {
                HighwayManagerFactory.DestroyHighwayManager(highwayManager);
            }
            foreach(var resourceDepot in new List<ResourceDepotBase>(ResourceDepotFactory.ResourceDepots)) {
                ResourceDepotFactory.DestroyDepot(resourceDepot);
            }
            foreach(var society in new List<SocietyBase>(SocietyFactory.Societies)) {
                SocietyFactory.DestroySociety(society);
            }
        }

        private Dictionary<int, MapNodeBase> LoadMapNodes(SerializableSession session) {
            var mapNodeIDMapping = new Dictionary<int, MapNodeBase>();

            foreach(var nodeData in session.MapNodes) {
                var successorNode = MapGraph.BuildNode(nodeData.LocalPosition, nodeData.LandType);

                BlobSitePermissionProfile.AllPermissiveProfile.InsertProfileIntoBlobSite(successorNode.BlobSite);
                foreach(var stockpilePair in nodeData.ResourceStockpileOfType) {
                    for(int i = 0; i < stockpilePair.Value; ++i) {
                        successorNode.BlobSite.PlaceBlobInto(BlobFactory.BuildBlob(stockpilePair.Key, successorNode.transform.position));
                    }
                }

                successorNode.BlobSite.ClearPermissionsAndCapacity();
                if(nodeData.CurrentBlobSitePermissionProfile != null) {
                    nodeData.CurrentBlobSitePermissionProfile.InsertProfileIntoBlobSite(successorNode.BlobSite);
                }
                mapNodeIDMapping[nodeData.ID] = successorNode;
            }

            return mapNodeIDMapping;
        }

        private Dictionary<int, MapEdgeBase> LoadMapEdges(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            var mapEdgeIDMapping = new Dictionary<int, MapEdgeBase>();

            foreach(var edgeData in session.MapEdges) {
                MapNodeBase firstEndpoint;
                MapNodeBase secondEndpoint;
                if(
                    mapNodeIDMapping.TryGetValue(edgeData.FirstEndpointID, out firstEndpoint) && 
                    mapNodeIDMapping.TryGetValue(edgeData.SecondEndpointID, out secondEndpoint)
                ){
                    var successorEdge = MapGraph.BuildMapEdge(firstEndpoint, secondEndpoint);
                    mapEdgeIDMapping[edgeData.ID] = successorEdge;
                }else {
                    Debug.LogErrorFormat("Failed to create map edge between nodes of ID {0} and {1}: one of the nodes does not exist",
                        edgeData.FirstEndpointID, edgeData.SecondEndpointID);
                }
            }

            return mapEdgeIDMapping;
        }

        private void LoadNeighborhoods(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping,
            Dictionary<int, MapEdgeBase> mapEdgeIDMapping) {
            foreach(var neighborhoodData in session.Neighborhoods) {
                var newNeighborhood = (new GameObject()).AddComponent<Neighborhood>();
                newNeighborhood.gameObject.name = neighborhoodData.Name;
                var neigborhoodTransform = newNeighborhood.transform;
                neigborhoodTransform.SetParent(MapGraph.transform);
                neigborhoodTransform.localPosition = neighborhoodData.LocalPosition;

                foreach(var nodeID in neighborhoodData.ChildNodeIDs) {
                    mapNodeIDMapping[nodeID].transform.SetParent(neigborhoodTransform, false);
                }
                foreach(var edgeID in neighborhoodData.ChildEdgeIDs) {
                    mapEdgeIDMapping[edgeID].transform.SetParent(neigborhoodTransform, true);
                }
            }
        }

        private void LoadHighways(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            foreach(var highwayData in session.Highways) {
                MapNodeBase firstEndpoint;
                MapNodeBase secondEndpoint;
                if(
                    mapNodeIDMapping.TryGetValue(highwayData.FirstEndpointID, out firstEndpoint) && 
                    mapNodeIDMapping.TryGetValue(highwayData.SecondEndpointID, out secondEndpoint)
                ){
                    var successorHighway = HighwayFactory.ConstructHighwayBetween(firstEndpoint, secondEndpoint);

                    successorHighway.Priority = highwayData.Priority;
                    successorHighway.Efficiency = highwayData.Efficiency;

                    foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                        successorHighway.SetUpkeepRequestedForResource(resourceType, highwayData.UpkeepRequestedForResource[resourceType]);
                        successorHighway.SetPullingPermissionForFirstEndpoint(resourceType, highwayData.PullingPermissionForFirstEndpoint[resourceType]);
                        successorHighway.SetPullingPermissionForSecondEndpoint(resourceType, highwayData.PullingPermissionForSecondEndpoint[resourceType]);
                    }
                }
            }
        }

        private void LoadConstructionZones(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            foreach(var zoneData in session.ConstructionZones) {
                MapNodeBase location;
                ConstructionProjectBase project;

                if( mapNodeIDMapping.TryGetValue(zoneData.LocationID, out location) &&
                    ConstructionZoneFactory.TryGetProjectOfName(zoneData.ProjectName, out project)
                ){
                    ConstructionZoneFactory.BuildConstructionZone(location, project);
                }
            }
        }

        private void LoadHighwayManagers(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            foreach(var managerData in session.HighwayManagers) {
                MapNodeBase location;
                if(mapNodeIDMapping.TryGetValue(managerData.LocationID, out location)) {
                    HighwayManagerFactory.ConstructHighwayManagerAtLocation(location);
                }
            }
        }

        private void LoadResourceDepots(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            foreach(var depotData in session.ResourceDepots) {
                MapNodeBase location;
                if(mapNodeIDMapping.TryGetValue(depotData.LocationID, out location)) {
                    ResourceDepotFactory.ConstructDepotAt(location);
                }
            }
        }

        private void LoadSocieties(SerializableSession session, Dictionary<int, MapNodeBase> mapNodeIDMapping) {
            foreach(var societyData in session.Societies) {
                MapNodeBase location;
                ComplexityLadderBase ladder = SocietyFactory.GetComplexityLadderOfName(societyData.ActiveComplexityLadderName);
                ComplexityDefinitionBase complexity = SocietyFactory.GetComplexityDefinitionOfName(societyData.CurrentComplexityName);

                if(mapNodeIDMapping.TryGetValue(societyData.LocationID, out location) && complexity != null && ladder != null) {
                    var successorSociety = SocietyFactory.ConstructSocietyAt(location, ladder, complexity);
                    successorSociety.AscensionIsPermitted = societyData.AscensionIsPermitted;
                    successorSociety.SecondsOfUnsatisfiedNeeds = societyData.SecondsOfUnsatisfiedNeeds;
                }
            }
        }

        private void LoadTerrainData(SerializableSession session) {
            if(session.TerrainData != null) {
                var terrainData = session.TerrainData;
                TerrainGrid.Radius = terrainData.Radius;
                TerrainGrid.MaxAcquisitionDistance = terrainData.MaxTerrainAcquisitionRange;
                TerrainGrid.Layout = terrainData.Layout;

                TerrainGrid.ClearMap();
                TerrainGrid.CreateMap();
                TerrainGrid.RefreshMapTerrains();
            }
        }

        private void LoadCameraData(SerializableSession session) {
            if(session.CameraData != null) {
                MainCamera.orthographicSize = session.CameraData.Size;
                MainCamera.transform.position = session.CameraData.Position;
            }
        }

        #endregion

    }

}
