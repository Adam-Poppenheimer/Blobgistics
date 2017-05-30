using System;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.Highways;
using Assets.Session.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Session.Editor {

    public class SessionManagerTests {

        #region instance methods

        #region tests        

        [Test]
        public void OnSessionPushedToRuntime_AllOldObjectsInRuntimeAreDestroyed() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            mapGraph.BuildNode(Vector3.zero);
            mapGraph.BuildNode(Vector3.zero);
            mapGraph.BuildNode(Vector3.zero);
            mapGraph.BuildMapEdge(null, null);
            mapGraph.BuildMapEdge(null, null);
            mapGraph.BuildMapEdge(null, null);

            var highwayFactory = BuildMockHighwayFactory();
            highwayFactory.ConstructHighwayBetween(null, null);
            highwayFactory.ConstructHighwayBetween(null, null);
            highwayFactory.ConstructHighwayBetween(null, null);

            var constructionZoneFactory = BuildMockConstructionZoneFactory();
            constructionZoneFactory.BuildConstructionZone(null, null);
            constructionZoneFactory.BuildConstructionZone(null, null);
            constructionZoneFactory.BuildConstructionZone(null, null);

            var highwayManagerFactory = BuildMockHighwayManagerFactory();
            highwayManagerFactory.ConstructHighwayManagerAtLocation(null);
            highwayManagerFactory.ConstructHighwayManagerAtLocation(null);
            highwayManagerFactory.ConstructHighwayManagerAtLocation(null);

            var resourceDepotFactory = BuildMockResourceDepotFactory();
            resourceDepotFactory.ConstructDepotAt(null);
            resourceDepotFactory.ConstructDepotAt(null);
            resourceDepotFactory.ConstructDepotAt(null);

            var societyFactory = BuildMockSocietyFactory();
            societyFactory.ConstructSocietyAt(null, null, null);
            societyFactory.ConstructSocietyAt(null, null, null);
            societyFactory.ConstructSocietyAt(null, null, null);

            var managerToTest = BuildSessionManager();
            managerToTest.MapGraph = mapGraph;
            managerToTest.HighwayFactory = highwayFactory;
            managerToTest.ConstructionZoneFactory = constructionZoneFactory;
            managerToTest.HighwayManagerFactory = highwayManagerFactory;
            managerToTest.ResourceDepotFactory = resourceDepotFactory;
            managerToTest.SocietyFactory = societyFactory;

            //Execution
            managerToTest.PushSessionIntoRuntime(new SerializableSession("Empty Session"));

            //Validation
            Assert.IsEmpty(mapGraph.Nodes,                            "MapGraph.Nodes still has undestroyed elements");
            Assert.IsEmpty(mapGraph.Edges,                            "MapGraph.Edges still has undestroyed elements");
            Assert.IsEmpty(highwayFactory.Highways,                   "HighwayFactory.Highways still has undestroyed elements");
            Assert.IsEmpty(constructionZoneFactory.ConstructionZones, "ConstructionZoneFactory.ConstructionZones still has undestroyed elements");
            Assert.IsEmpty(highwayManagerFactory.Managers,            "HighwayManagerFactory.HighwayManagers still has undestroyed elements");
            Assert.IsEmpty(resourceDepotFactory.ResourceDepots,       "ResourceDepotFactory.ResourceDepots still has undestroyed elements");
            Assert.IsEmpty(societyFactory.Societies,                  "SocietyFactory.Societies still has undestroyed elements");
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllMapNodesAndEdgesAreConstructedAndInitializedProperly() {
            //Setup
            var mapGraphToPullFrom = BuildMockMapGraph();
            var nodeCenter = mapGraphToPullFrom.BuildNode(Vector3.zero,  TerrainType.Grassland);
            var nodeLeft   = mapGraphToPullFrom.BuildNode(Vector3.left,  TerrainType.Forest   );
            var nodeRight  = mapGraphToPullFrom.BuildNode(Vector3.right, TerrainType.Mountains);
            var nodeUp     = mapGraphToPullFrom.BuildNode(Vector3.up,    TerrainType.Grassland);

            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeLeft );
            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeRight);
            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeUp   );

            var mapGraphToPushTo = BuildMockMapGraph();

            var highwayFactory = BuildMockHighwayFactory();

            var managerToTest = BuildSessionManager();
            managerToTest.MapGraph = mapGraphToPullFrom;
            managerToTest.HighwayFactory = highwayFactory;

            //Execution
            var sessionPulled = managerToTest.PullSessionFromRuntime("sessionPulled");

            managerToTest.MapGraph = mapGraphToPushTo;

            managerToTest.PushSessionIntoRuntime(sessionPulled);

            //Validation
            foreach(var node in mapGraphToPullFrom.Nodes) {
                Assert.That(DoesSomeEquivalentNodeExist(node, mapGraphToPushTo.Nodes),
                    string.Format("Node from mapGraphToPullFrom with ID {0} has no equivalent node in mapGraphToPushTo", node.ID));
            }
            foreach(var edge in mapGraphToPullFrom.Edges) {
                Assert.That(DoesSomeEquivalentEdgeExist(edge, mapGraphToPushTo.Edges),
                    string.Format("Edge from mapGraphToPullFrom with ID {0} has no equivalent edge in mapGraphToPushTo", edge.ID));
            }
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllHighwaysAreConstructedAndInitializedProperly() {
            //Setup
            var mapGraphToPullFrom = BuildMockMapGraph();
            var nodeCenter = mapGraphToPullFrom.BuildNode(Vector3.zero,  TerrainType.Grassland);
            var nodeLeft   = mapGraphToPullFrom.BuildNode(Vector3.left,  TerrainType.Forest   );
            var nodeRight  = mapGraphToPullFrom.BuildNode(Vector3.right, TerrainType.Mountains);
            var nodeUp     = mapGraphToPullFrom.BuildNode(Vector3.up,    TerrainType.Grassland);

            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeLeft );
            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeRight);
            mapGraphToPullFrom.BuildMapEdge(nodeCenter, nodeUp   );

            var mapGraphToPushTo = BuildMockMapGraph();

            var highwayFactoryToPullFrom = BuildMockHighwayFactory();

            var highwayOne   = highwayFactoryToPullFrom.ConstructHighwayBetween(nodeCenter, nodeLeft );
            var highwayTwo   = highwayFactoryToPullFrom.ConstructHighwayBetween(nodeCenter, nodeRight);
            var highwayThree = highwayFactoryToPullFrom.ConstructHighwayBetween(nodeCenter, nodeUp   );

            highwayOne.SetUpkeepRequestedForResource(ResourceType.Food,   true);
            highwayOne.SetUpkeepRequestedForResource(ResourceType.Cotton, true);
            highwayOne.SetPullingPermissionForFirstEndpoint (ResourceType.Food,   true);
            highwayOne.SetPullingPermissionForSecondEndpoint(ResourceType.Cotton, true);
            highwayOne.Priority = 1;
            highwayOne.Efficiency = 1f;

            highwayTwo.SetUpkeepRequestedForResource(ResourceType.Wood, true);
            highwayTwo.SetUpkeepRequestedForResource(ResourceType.Ore,  true);
            highwayTwo.SetPullingPermissionForFirstEndpoint (ResourceType.Wood, true);
            highwayTwo.SetPullingPermissionForSecondEndpoint(ResourceType.Ore,  true);
            highwayTwo.Priority = 2;
            highwayTwo.Efficiency = 2f;

            highwayThree.SetUpkeepRequestedForResource(ResourceType.Steel,  true);
            highwayThree.SetUpkeepRequestedForResource(ResourceType.Lumber, true);
            highwayThree.SetPullingPermissionForFirstEndpoint (ResourceType.Steel,  true);
            highwayThree.SetPullingPermissionForSecondEndpoint(ResourceType.Lumber, true);
            highwayThree.Priority = 3;
            highwayThree.Efficiency = 3f;

            var highwayFactoryToPushTo = BuildMockHighwayFactory();

            var managerToTest = BuildSessionManager();
            managerToTest.MapGraph = mapGraphToPullFrom;
            managerToTest.HighwayFactory = highwayFactoryToPullFrom;

            //Execution
            var sessionPulled = managerToTest.PullSessionFromRuntime("sessionPulled");

            managerToTest.MapGraph = mapGraphToPushTo;
            managerToTest.HighwayFactory = highwayFactoryToPushTo;

            managerToTest.PushSessionIntoRuntime(sessionPulled);

            //Validation
            foreach(var highway in highwayFactoryToPullFrom.Highways) {
                Assert.That(DoesSomeEquivalentHighwayExist(highway, highwayFactoryToPushTo.Highways),
                    string.Format("Highway from highwayFactoryToPullFrom with ID {0} " +
                        "has no equivalent highway in highwayFactoryToPushTo", highway.ID));
            }
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllConstructionZonesAreConstructedAndInitializedProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllHighwayManagersAreConstructedAndInitializedProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllResourceDepotsAreConstructedAndInitializedProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnSessionPulledFromAndPushedToRuntime_AllSocietiesAreConstructedAndInitializedProperly() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private MockMapGraph BuildMockMapGraph() {
            return (new GameObject()).AddComponent<MockMapGraph>();
        }

        private MockHighwayFactory BuildMockHighwayFactory() {
            return (new GameObject()).AddComponent<MockHighwayFactory>();
        }

        private SessionManager BuildSessionManager() {
            return (new GameObject()).AddComponent<SessionManager>();
        }

        private MockConstructionZoneFactory BuildMockConstructionZoneFactory() {
            return (new GameObject()).AddComponent<MockConstructionZoneFactory>();
        }

        private MockHighwayManagerFactory BuildMockHighwayManagerFactory() {
            return (new GameObject()).AddComponent<MockHighwayManagerFactory>();
        }

        private MockResourceDepotFactory BuildMockResourceDepotFactory() {
            return (new GameObject()).AddComponent<MockResourceDepotFactory>();
        }

        private MockSocietyFactory BuildMockSocietyFactory() {
            return (new GameObject()).AddComponent<MockSocietyFactory>();
        }

        private bool DoesSomeEquivalentNodeExist(MapNodeBase node, ReadOnlyCollection<MapNodeBase> candidates) {
            foreach(var candidate in candidates) {
                if(AreNodesEquivalent(node, candidate)){
                    return true;
                }
            }
            return false;
        }

        private bool AreNodesEquivalent(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            return nodeOne.transform.localPosition == nodeTwo.transform.localPosition && nodeOne.Terrain == nodeTwo.Terrain;
        }

        private bool DoesSomeEquivalentEdgeExist(MapEdgeBase edge, ReadOnlyCollection<MapEdgeBase> candidates) {
            foreach(var candidate in candidates) {
                if(AreEdgesEquivalent(edge, candidate)) {
                    return true;
                }
            }
            return false;
        }

        private bool AreEdgesEquivalent(MapEdgeBase edgeOne, MapEdgeBase edgeTwo) {
            return AreNodesEquivalent(edgeOne.FirstNode, edgeTwo.FirstNode) && AreNodesEquivalent(edgeOne.SecondNode, edgeTwo.SecondNode);
        }

        private bool DoesSomeEquivalentHighwayExist(BlobHighwayBase highway, ReadOnlyCollection<BlobHighwayBase> candidates) {
            foreach(var candidate in candidates) {
                if(AreHighwaysEquivalent(highway, candidate)) {
                    return true;
                }
            }
            return false;
        }

        private bool AreHighwaysEquivalent(BlobHighwayBase highwayOne, BlobHighwayBase highwayTwo) {
            var endpointsAreEquivalent =
                AreNodesEquivalent(highwayOne.FirstEndpoint,  highwayTwo.FirstEndpoint) &&
                AreNodesEquivalent(highwayOne.SecondEndpoint, highwayTwo.SecondEndpoint);
            var prioritiesAreEquivalent = highwayOne.Priority == highwayTwo.Priority;
            var efficienciesAreEquivalent = Mathf.Approximately(highwayOne.Efficiency, highwayTwo.Efficiency);

            bool upkeepRequestsAreEquivalent = true;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highwayOne.GetUpkeepRequestedForResource(resourceType) != highwayTwo.GetUpkeepRequestedForResource(resourceType)) {
                    upkeepRequestsAreEquivalent = false;
                }
            }

            bool firstEndpointPullingPermissionsAreEquivalent = true;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highwayOne.GetPullingPermissionForFirstEndpoint(resourceType) != highwayTwo.GetPullingPermissionForFirstEndpoint(resourceType)) {
                    firstEndpointPullingPermissionsAreEquivalent = false;
                }
            }

            bool secondEndpointPullingPermissionsAreEquivalent = true;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highwayOne.GetPullingPermissionForSecondEndpoint(resourceType) != highwayTwo.GetPullingPermissionForSecondEndpoint(resourceType)) {
                    secondEndpointPullingPermissionsAreEquivalent = false;
                }
            }

            return endpointsAreEquivalent && prioritiesAreEquivalent && efficienciesAreEquivalent &&
                upkeepRequestsAreEquivalent && firstEndpointPullingPermissionsAreEquivalent &&
                secondEndpointPullingPermissionsAreEquivalent;
        }

        #endregion

        #endregion

    }

}


