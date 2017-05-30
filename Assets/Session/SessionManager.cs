using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

using Assets.Map;
using Assets.ConstructionZones;
using Assets.HighwayManager;
using Assets.Highways;
using Assets.ResourceDepots;
using Assets.Societies;

namespace Assets.Session {

    public class SessionManager : MonoBehaviour {

        #region instance fields and properties

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

        #endregion

        #region instance methods

        public void SaveCurrentSessionToFile(string sessionName, string filePath) {
            var session = PullSessionFromRuntime(sessionName);

            using(FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                try {
                    formatter.Serialize(fileStream, session);
                }catch(SerializationException e) {
                    Debug.LogError("Failed to serialize session. Reason given: " + e.Message);
                    throw;
                }
            }
        }

        public void LoadSessionFromFile(string filePath) {
            using(FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
                try {
                    var formatter = new BinaryFormatter();
                    var session = formatter.Deserialize(fileStream) as SerializableSession;
                    if(session != null) {
                        PushSessionIntoRuntime(session);
                    }else {
                        Debug.LogError("Failed to load session from file: the file did not represent a SerializableSession object");
                    }
                }catch(SerializationException e) {
                    Debug.LogError("Failed to deserialize. Reason given: " + e.Message);
                    throw;
                }
            }
        }

        public SerializableSession PullSessionFromRuntime(string sessionName) {
            var retval = new SerializableSession(sessionName);

            PushMapGraphIntoSession(MapGraph,       retval);
            PushHighwaysIntoSession(HighwayFactory, retval);

            return retval;
        }

        public void PushSessionIntoRuntime(SerializableSession session) {
            ClearRuntime();

            var mapNodeIDMapping = LoadMapNodes(session);
            LoadMapEdges(session, mapNodeIDMapping);
            LoadHighways(session, mapNodeIDMapping);
        }

        private void PushMapGraphIntoSession(MapGraphBase mapGraph, SerializableSession session) {
            foreach(var node in mapGraph.Nodes) {
                session.MapNodes.Add(new SerializableMapNodeData(node));
            }
            foreach(var edge in mapGraph.Edges) {
                session.MapEdges.Add(new SerializableMapEdgeData(edge));
            }
        }

        private void PushHighwaysIntoSession(BlobHighwayFactoryBase highwayFactory, SerializableSession session) {
            foreach(var highway in highwayFactory.Highways) {
                session.Highways.Add(new SerializableHighwayData(highway));
            }
        }

        private void PushConstructionZonesIntoSession(ConstructionZoneFactoryBase zoneFactory, SerializableSession session) {
            throw new NotImplementedException();
        }

        private void PushHighwayManagersIntoSession(HighwayManagerFactoryBase managerFactory, SerializableSession session) {
            throw new NotImplementedException();
        }

        private void PushResourceDepotsIntoSession(ResourceDepotFactoryBase depotFactory, SerializableSession session) {
            throw new NotImplementedException();
        }

        private void PushSocietiesIntoSession(SocietyFactoryBase societyFactory, SerializableSession session) {
            throw new NotImplementedException();
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
                var successorNode = MapGraph.BuildNode(nodeData.LocalPosition, nodeData.Terrain);
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
                }else {
                    Debug.LogErrorFormat("Failed to create map edge between nodes of ID {0} and {1}: one of the nodes does not exist",
                        edgeData.FirstEndpointID, edgeData.SecondEndpointID);
                }
            }

            return mapEdgeIDMapping;
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

                    foreach(var resourceType in highwayData.UpkeepRequestedForResource.Keys) {
                        successorHighway.SetUpkeepRequestedForResource(resourceType, true);
                    }

                    foreach(var resourceType in highwayData.PullingPermissionForFirstEndpoint.Keys) {
                        successorHighway.SetPullingPermissionForFirstEndpoint(resourceType, true);
                    }

                    foreach(var resourceType in highwayData.PullingPermissionForSecondEndpoint.Keys) {
                        successorHighway.SetPullingPermissionForSecondEndpoint(resourceType, true);
                    }
                }
            }
        }

        #endregion

    }

}
