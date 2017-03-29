using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.ConstructionZones;
using Assets.Highways;
using Assets.HighwayUpgraders;
using Assets.Depots;
using Assets.Societies;

using Assets.Core.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.Editor {

    public class SimulationControlTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnCanConnectNodesWithHighwayIsCalled_ReturnValueProperlyRepresentsInternalState() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            //Execution

            //Validation

            //Returns true when there exists an edge between the two nodes, and that edge is empty
            Assert.That(controlToTest.CanConnectNodesWithHighway(middleNode.ID, rightNode.ID),
                "Does not permit valid placement location between middleNode and rightNode");
            Assert.That(controlToTest.CanConnectNodesWithHighway(middleNode.ID, leftNode.ID),
                "Does not permit valid placement location between middleNode and leftNode");
            Assert.That(controlToTest.CanConnectNodesWithHighway(middleNode.ID, upNode.ID),
                "Does not permit valid placement location between middleNode and upNode");

            //Returns false when there exists no edge between the two nodes
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(rightNode.ID, leftNode.ID),
                "Falsely permits invalid placement location between rightNode and leftNode");
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(rightNode.ID, upNode.ID),
                "Falsely permits invalid placement location between rightNode and upNode");
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(leftNode.ID,  upNode.ID),
                "Falsely permits invalid placement location between leftNode and upNode");

            //Returns false when the nodes are the same
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(rightNode.ID, rightNode.ID),
                "Falsely permits invalid placement location between rightNode and rightNode");
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(leftNode.ID,  leftNode.ID),
                "Falsely permits invalid placement location between leftNode and leftNode");
            Assert.IsFalse(controlToTest.CanConnectNodesWithHighway(upNode.ID,    upNode.ID),
                "Falsely permits invalid placement location between upNode and upNode");
        }       

        [Test]
        public void OnConnectNodeWithHighwayIsCalled_HighwayIsConstructedBetweenTheProperNodes() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayfactory = controlToTest.HighwayFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            //Execution
            controlToTest.ConnectNodesWithHighway(middleNode.ID, rightNode.ID);
            controlToTest.ConnectNodesWithHighway(middleNode.ID, leftNode.ID );
            controlToTest.ConnectNodesWithHighway(middleNode.ID, upNode.ID   );

            //Validation
            Assert.That(highwayfactory.HasHighwayBetween(middleNode, rightNode),
                "Expected highway between middleNode and rightNode, but none was created");
            Assert.That(highwayfactory.HasHighwayBetween(middleNode, leftNode),
                "Expected highway between middleNode and leftNode, but none was created");
            Assert.That(highwayfactory.HasHighwayBetween(middleNode, upNode),
                "Expected highway between middleNode and upNode, but none was created");
        }

        [Test]
        public void OnSetHighwayPermissionForResourceIsCalled_SpecifiedHighwayReceivesTheSpecifiedChanges() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayfactory = controlToTest.HighwayFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);

            var highway1 = highwayfactory.ConstructHighwayBetween(middleNode, rightNode);
            var highway2 = highwayfactory.ConstructHighwayBetween(middleNode, leftNode);

            //Execution

            //Highway 1
            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway1.ID, ResourceType.Food, true);
            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway1.ID, ResourceType.Yellow, true);

            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway1.ID, ResourceType.Food, false);
            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway1.ID, ResourceType.Yellow, true);

            //Highway 2

            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway2.ID, ResourceType.Food, true);
            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway2.ID, ResourceType.White, true);

            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway2.ID, ResourceType.Food, false);
            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway2.ID, ResourceType.White, true);

            //Validation

            //Highway 1

            Assert.That(
                highway1.GetPullingPermissionForFirstEndpoint(ResourceType.Food),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "1", "First", ResourceType.Food
                )
            );
            Assert.That(
                highway1.GetPullingPermissionForFirstEndpoint(ResourceType.Yellow),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "1", "First", ResourceType.Yellow
                )
            );
            Assert.IsFalse(
                highway1.GetPullingPermissionForFirstEndpoint(ResourceType.White),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "1", "First", ResourceType.White
                )
            );

            Assert.IsFalse(
                highway1.GetPullingPermissionForSecondEndpoint(ResourceType.Food),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "1", "Second", ResourceType.Food
                )
            );
            Assert.That(
                highway1.GetPullingPermissionForSecondEndpoint(ResourceType.Yellow),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "1", "Second", ResourceType.Yellow
                )
            );
            Assert.IsFalse(
                highway1.GetPullingPermissionForSecondEndpoint(ResourceType.White),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "1", "Second", ResourceType.White
                )
            );

            //Highway 2

            Assert.That(
                highway2.GetPullingPermissionForFirstEndpoint(ResourceType.Food), 
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "2", "First", ResourceType.Food
                )
            );
            Assert.IsFalse(
                highway2.GetPullingPermissionForFirstEndpoint(ResourceType.Yellow), 
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "2", "First", ResourceType.Yellow
                )
            );
            Assert.That(
                highway2.GetPullingPermissionForFirstEndpoint(ResourceType.White), 
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "2", "First", ResourceType.White
                )
            );

            Assert.IsFalse(
                highway2.GetPullingPermissionForSecondEndpoint(ResourceType.Food),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "2", "Second", ResourceType.Food
                )
            );
            Assert.IsFalse(
                highway2.GetPullingPermissionForSecondEndpoint(ResourceType.Yellow), 
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "2", "Second", ResourceType.Yellow
                )
            );
            Assert.That(
                highway2.GetPullingPermissionForSecondEndpoint(ResourceType.White),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "2", "Second", ResourceType.White
                )
            );
        }

        [Test]
        public void OnSetHighwayPriorityIsCalled_SpecifiedHighwayReceivesTheSpecifiedPriority() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayfactory = controlToTest.HighwayFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            var highway1 = highwayfactory.ConstructHighwayBetween(middleNode, rightNode);
            var highway2 = highwayfactory.ConstructHighwayBetween(middleNode, leftNode);
            var highway3 = highwayfactory.ConstructHighwayBetween(middleNode, upNode);

            //Execution
            controlToTest.SetHighwayPriority(highway1.ID, 11);
            controlToTest.SetHighwayPriority(highway2.ID, 22);
            controlToTest.SetHighwayPriority(highway3.ID, 33);

            //Validation
            Assert.AreEqual(11, highway1.Priority, "Highway1 has an incorrect priority");
            Assert.AreEqual(22, highway2.Priority, "Highway2 has an incorrect priority");
            Assert.AreEqual(33, highway3.Priority, "Highway3 has an incorrect priority");
        }

        [Test]
        public void OnDestroyHighwayIsCalled_SpecifiedHighwayObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayFactory = controlToTest.HighwayFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            var highway1 = highwayFactory.ConstructHighwayBetween(middleNode, rightNode);
            var highway2 = highwayFactory.ConstructHighwayBetween(middleNode, leftNode);
            var highway3 = highwayFactory.ConstructHighwayBetween(middleNode, upNode);

            highway1.name = "SimulationControlUnitTestsHighway1";
            highway2.name = "SimulationControlUnitTestsHighway2";
            highway3.name = "SimulationControlUnitTestsHighway3";

            //Execution
            controlToTest.DestroyHighway(highway1.ID);
            controlToTest.DestroyHighway(highway2.ID);
            controlToTest.DestroyHighway(highway3.ID);

            //Validation
            Assert.IsFalse(highwayFactory.HasHighwayBetween(middleNode, rightNode),
                "highwayFactory still registers the existence of highway1");
            Assert.IsFalse(highwayFactory.HasHighwayBetween(middleNode, leftNode),
                "highwayFactory still registers the existence of highway2");
            Assert.IsFalse(highwayFactory.HasHighwayBetween(middleNode, upNode),
                "highwayFactory still registers the existence of highway3");

            Assert.IsNull(GameObject.Find("SimulationControlUnitTestsHighway1"), "The GameObject highway1 was attached to still exists");
            Assert.IsNull(GameObject.Find("SimulationControlUnitTestsHighway2"), "The GameObject highway2 was attached to still exists");
            Assert.IsNull(GameObject.Find("SimulationControlUnitTestsHighway3"), "The GameObject highway3 was attached to still exists");
        }

        [Test]
        public void OnCanCreateResourceDepotConstructionSiteOnNodeIsCalled_ReturnValueProperlyRepresentsInternalState() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var depotFactory = controlToTest.ResourceDepotFactory;
            var societyFactory = controlToTest.SocietyFactory;
            var constructionZoneFactory = controlToTest.ConstructionZoneFactory;

            var freeNode         = mapGraph.BuildNode(Vector3.zero);
            var depotNode        = mapGraph.BuildNode(Vector3.right);
            var societyNode      = mapGraph.BuildNode(Vector3.left);
            var constructionNode = mapGraph.BuildNode(Vector3.up);

            depotFactory.ConstructDepotAt(depotNode);
            societyFactory.ConstructSocietyAt(societyNode, societyFactory.StandardComplexityLadder);
            constructionZoneFactory.BuildConstructionZone(constructionNode, constructionZoneFactory.ResourceDepotProject);

            //Validation

            //Returns true when the MapNode is empty
            Assert.That(controlToTest.CanCreateResourceDepotConstructionSiteOnNode(freeNode.ID),
                "Was not permitted to construct on freeNode");

            //Return false when there is a society on the MapNode
            Assert.IsFalse(controlToTest.CanCreateResourceDepotConstructionSiteOnNode(depotNode.ID),
                "Falsely permitted to construct on depotNode");

            //Returns false when there is another depot on the MapNode
            Assert.IsFalse(controlToTest.CanCreateResourceDepotConstructionSiteOnNode(societyNode.ID),
                "Falsely permitted to construct on societyNode");

            //Returns false when there is a ConstructionSite on the MapNode
            Assert.IsFalse(controlToTest.CanCreateResourceDepotConstructionSiteOnNode(constructionNode.ID),
                "Falsely permitted to construct on constructionNode");
        }

        [Test]
        public void OnCreateResourceDepotConstructionSiteOnNodeIsCalled_ProperConstructionSiteIsCreatedOnTheProperNode() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var constructionZoneFactory = controlToTest.ConstructionZoneFactory;

            var nodeToPlaceUpon = mapGraph.BuildNode(Vector3.zero);

            //Execution
            controlToTest.CreateResourceDepotConstructionSiteOnNode(nodeToPlaceUpon.ID);

            //Validation
            
            Assert.NotNull(constructionZoneFactory.HasConstructionZoneAtLocation(nodeToPlaceUpon),
                "constructionZoneFactory does not register a ConstructionZone at the specified location");

            var zoneAtLocation = constructionZoneFactory.GetConstructionZoneAtLocation(nodeToPlaceUpon);
            Assert.AreEqual(constructionZoneFactory.ResourceDepotProject, zoneAtLocation.CurrentProject,
                "The construction zone at the specified location has the wrong CurrentProject");
        }

        [Test]
        public void OnDestroyConstructionZoneIsCalled_SpecifiedConstructionZoneObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var constructionZoneFactory = controlToTest.ConstructionZoneFactory;

            var nodeToPlaceUpon = mapGraph.BuildNode(Vector3.zero);

            controlToTest.CreateResourceDepotConstructionSiteOnNode(nodeToPlaceUpon.ID);

            var zoneAtLocation = constructionZoneFactory.GetConstructionZoneAtLocation(nodeToPlaceUpon);
            zoneAtLocation.name = "SimulationControl Integration Tests' ConstructionZone";

            //Execution
            controlToTest.DestroyConstructionZone(zoneAtLocation.ID);

            //Validation
            Assert.IsFalse(constructionZoneFactory.HasConstructionZoneAtLocation(nodeToPlaceUpon),
                "ConstructionZoneFactory still registers the removed ConstructionZone");

            Assert.IsNull(GameObject.Find("SimulationControl Integration Tests' ConstructionZone"), 
                "The removed ConstructionZone is still within the GameObject hierarchy");
        }

        [Test]
        public void OnCanDestroySocietyIsCalled_ReturnValueProperlyRepresentsInternalState() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var societyFactory = controlToTest.SocietyFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);

            var simpleSociety = societyFactory.ConstructSocietyAt(middleNode, societyFactory.StandardComplexityLadder);
            var complexifiedSociety = societyFactory.ConstructSocietyAt(rightNode, societyFactory.StandardComplexityLadder);

            CauseSocietyToAscend(complexifiedSociety);

            //Execution

            //Validation
            Assert.IsTrue(controlToTest.CanDestroySociety(simpleSociety.ID),
                "Is not permitted to destroy the simple society");

            Assert.IsFalse(controlToTest.CanDestroySociety(complexifiedSociety.ID), 
                "Is falsely permitted to destroy the complexified society");
        }

        [Test]
        public void OnDestroySocietyIsCalled_SpecifiedSocietyObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var societyFactory = controlToTest.SocietyFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);

            var societyToRemove = societyFactory.ConstructSocietyAt(middleNode, societyFactory.StandardComplexityLadder);
            societyToRemove.name = "SimulationControl Integration Tests' Society";

            //Execution
            controlToTest.DestroySociety(societyToRemove.ID);

            //Validation
            Assert.IsFalse(societyFactory.HasSocietyAtLocation(middleNode),
                "SocietyFactory still registers the removed middleNode");
            Assert.IsNull(GameObject.Find("SimulationControl Integration Tests' Society"),
                "Destroyed Society's GameObject still exists in the GameObject hierarchy");
        }

        [Test]
        public void OnCanCreateHighwayUpgraderOnHighwayIsCalled_ReturnValueRepresentsInternalState() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayFactory = controlToTest.HighwayFactory;
            var upgraderFactory = controlToTest.HighwayUpgraderFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            var highway1 = highwayFactory.ConstructHighwayBetween(middleNode, rightNode);
            var highway2 = highwayFactory.ConstructHighwayBetween(middleNode, leftNode);
            var highway3 = highwayFactory.ConstructHighwayBetween(middleNode, upNode);

            highway1.Profile = controlToTest.UpgradedHighwayProfile;

            upgraderFactory.BuildHighwayUpgrader(highway3, mapGraph.GetEdge(middleNode, upNode).BlobSite, controlToTest.UpgradedHighwayProfile);

            //Execution

            //Validation
            Assert.IsFalse(controlToTest.CanCreateHighwayUpgraderOnHighway(highway1.ID), 
                "Falsely permits a highwayUpgrader on highway 1");
            Assert.IsTrue(controlToTest.CanCreateHighwayUpgraderOnHighway(highway2.ID), 
                "Does not permit a highwayUpgrader on highway 2");
            Assert.IsFalse(controlToTest.CanCreateHighwayUpgraderOnHighway(highway3.ID), 
                "Falsely permits a highwayUpgrader on highway 3");
        }

        [Test]
        public void OnCreateHighwayUpgraderOnHighwayIsCalled_ProperHighwayUpgraderIsCreatedWithTheProperState() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayFactory = controlToTest.HighwayFactory;
            var upgraderFactory = controlToTest.HighwayUpgraderFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);

            var highwayToUpgrade = highwayFactory.ConstructHighwayBetween(middleNode, rightNode);

            //Execution
            controlToTest.CreateHighwayUpgraderOnHighway(highwayToUpgrade.ID);

            //Validation
            Assert.That(upgraderFactory.HasUpgraderTargetingHighway(highwayToUpgrade), 
                "UpgraderFactory fails to register a HighwayUpgrader on highwayToUpgrade");

            var upgraderOnHighway = upgraderFactory.GetUpgraderTargetingHighway(highwayToUpgrade);
            Assert.AreEqual(controlToTest.UpgradedHighwayProfile, upgraderOnHighway.ProfileToInsert,
                "UpgraderOnHighway has an incorrect ProfileToInsert");
        }

        [Test]
        public void OnDestroyHighwayUpgraderIsCalled_SpecifiedHighwayUpgraderObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var highwayFactory = controlToTest.HighwayFactory;
            var upgraderFactory = controlToTest.HighwayUpgraderFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);

            mapGraph.AddUndirectedEdge(middleNode, rightNode);

            var highwayToUpgrade = highwayFactory.ConstructHighwayBetween(middleNode, rightNode);

            controlToTest.CreateHighwayUpgraderOnHighway(highwayToUpgrade.ID);
            var upgraderToDestroy = upgraderFactory.GetUpgraderTargetingHighway(highwayToUpgrade);
            upgraderToDestroy.name = "SimulationControl Integration Tests' HighwayUpgrader";

            //Execution
            controlToTest.DestroyHighwayUpgrader(upgraderToDestroy.ID);

            //Validation
            Assert.IsFalse(upgraderFactory.HasUpgraderTargetingHighway(highwayToUpgrade),
                "UpgraderFactory still registers the removed HighwayUpgrader");
            Assert.IsNull(GameObject.Find("SimulationControl Integration Tests' HighwayUpgrader"),
                "Destroyed HighwayUpgrader's GameObject still exists in the GameObject hierarchy");
        }

        [Test]
        public void OnTickSimulationIsCalled_AllSimulationTickingIsPerformed() {
            //Setup
            var societyFactory = BuildMockSocietyFactory();
            var highwayFactory = BuildMockHighwayFactory();
            var blobDistributor = BuildMockBlobDistributor();

            float amountTickedOnSociety = 0f;
            societyFactory.FactoryTicked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnSociety = e.Value;
            };

            float amountTickedOnHighway = 0f;
            highwayFactory.FactoryTicked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnHighway = e.Value;
            };

            float amountTickedOnDistributor = 0f;
            blobDistributor.Ticked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnDistributor = e.Value;
            };

            var controlToTest = BuildSimulationControl();
            controlToTest.SocietyFactory = societyFactory;
            controlToTest.HighwayFactory = highwayFactory;
            controlToTest.BlobDistributor = blobDistributor;

            //Execution
            controlToTest.TickSimulation(5f);

            //Validation
            Assert.AreEqual(5f, amountTickedOnSociety, "Incorrect amount ticked on Society");
            Assert.AreEqual(5f, amountTickedOnHighway, "Incorrect amount ticked on Highway");
            Assert.AreEqual(5f, amountTickedOnDistributor, "Incorrect amount ticked on Distributor");
        }

        #endregion

        #region error handling 

        [Test]
        public void OnCanConnectNodesWithHighwayIsCalledOnInvalidIDs_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;

            mapGraph.BuildNode(Vector3.zero);
            mapGraph.BuildNode(Vector3.left);

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CanConnectNodesWithHighway(-1000, 42);
            });

            //Validation

            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not just one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages.Last().LogType, "The message received was not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnConnectNodeWithHighwayIsCalled_WhileCanConnectNodesWithHighwayIsFalse_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode = mapGraph.BuildNode(Vector3.left);

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.ConnectNodesWithHighway(middleNode.ID, leftNode.ID);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not just one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages.Last().LogType, "The message received was not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnConnectNodesWithHighwayIsCalledOnInvalidIDs_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;

            mapGraph.BuildNode(Vector3.zero);
            mapGraph.BuildNode(Vector3.left);            

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.ConnectNodesWithHighway(-1000, 42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not just a single message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages.Last().LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnSetHighwayPermissionForResourceIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(42, ResourceType.Food, true);
                controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(42, ResourceType.Food, true);
            });

            //Validation
            Assert.AreEqual(2, insertionHandler.StoredMessages.Count, "There were not two messages received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The first message logged is not an error message");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[1].LogType, "The second message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnSetHighwayPriorityIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayPriority(42, 0);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroyHighwayIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyHighway(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnCanCreateResourceDepotConstructionSiteOnNodeIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CanCreateResourceDepotConstructionSiteOnNode(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnCreateResourceDepotConstructionSiteOnNodeIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CreateResourceDepotConstructionSiteOnNode(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroyConstructionZoneIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyConstructionZone(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnCanDestroySocietyIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CanDestroySociety(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroySocietyIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroySociety(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnCanCreateHighwayUpgraderOnHighwayIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CanCreateHighwayUpgraderOnHighway(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnCreateHighwayUpgraderOnHighwayIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.CreateHighwayUpgraderOnHighway(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroyHighwayUpgraderIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyHighwayUpgrader(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        #endregion

        #endregion

        #region utilities

        private MapGraph BuildMapGraph() {
            var hostingObject = new GameObject();

            var newMapGraph = hostingObject.AddComponent<MapGraph>();
            var newBlobSiteFactory = hostingObject.AddComponent<BlobSiteFactory>();
            var newBlobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();

            newBlobSiteFactory.BlobSitePrivateData = newBlobSitePrivateData;

            newMapGraph.BlobSiteFactory = newBlobSiteFactory;

            return newMapGraph;
        }

        private BlobHighwayFactory BuildHighwayFactory(MapGraphBase mapGraph, ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobHighwayFactory>();
            var newBlobTubeFactory = hostingObject.AddComponent<BlobTubeFactory>();
            var newHighwayProfile = new BlobHighwayProfile(1f, 10,
                ResourceSummary.BuildResourceSummary(
                    newFactory.gameObject,
                    new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
                )
            );

            newBlobTubeFactory.BlobFactory = blobFactory;

            newFactory.MapGraph = mapGraph;
            newFactory.BlobTubeFactory = newBlobTubeFactory;
            newFactory.StartingProfile = newHighwayProfile;

            return newFactory;
        }

        private ConstructionZoneFactory BuildConstructionZoneFactory(ResourceDepotFactoryBase depotFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ConstructionZoneFactory>();
            newFactory.ResourceDepotFactory = depotFactory;
            return newFactory;
        }

        private SocietyFactory BuildSocietyFactory(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<SocietyFactory>();
            
            newFactory.BlobFactory = blobFactory;
            newFactory.SetStandardComplexityLadder(BuildComplexityLadder());

            return newFactory;
        }

        private ResourceDepotFactory BuildDepotFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ResourceDepotFactory>();
        }

        private HighwayUpgraderFactory BuildHighwayUpgraderFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayUpgraderFactory>();
        }

        private SimulationControl BuildSimulationControl() {
            var hostingObject = new GameObject();
            var newControl = hostingObject.AddComponent<SimulationControl>();
            var newBlobFactory = BuildResourceBlobFactory();
            var newDepotFactory = BuildDepotFactory();

            newControl.MapGraph = BuildMapGraph();
            newControl.HighwayFactory = BuildHighwayFactory(newControl.MapGraph, newBlobFactory);
            newControl.SocietyFactory = BuildSocietyFactory(newBlobFactory);
            newControl.ConstructionZoneFactory = BuildConstructionZoneFactory(newDepotFactory);
            newControl.HighwayUpgraderFactory = BuildHighwayUpgraderFactory();
            newControl.ResourceDepotFactory = newDepotFactory;
            newControl.UpgradedHighwayProfile = new BlobHighwayProfile(2, 20, ResourceSummary.BuildResourceSummary(
                newControl.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 5)
            ));

            return newControl;
        }

        private void CauseSocietyToAscend(SocietyBase society) {
            var currentComplexity = society.CurrentComplexity;

            foreach(var needType in currentComplexity.Needs) {
                for(int i = 0; i < currentComplexity.Needs[needType]; ++i) {
                    society.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(needType));
                }
            }

            foreach(var ascentType in currentComplexity.CostOfAscent) {
                for(int i = 0; i < currentComplexity.CostOfAscent[ascentType]; ++i) {
                    if(society.Location.BlobSite.CanPlaceBlobOfTypeInto(ascentType)) {
                        society.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ascentType));
                    }
                }
            }
            society.TickConsumption(currentComplexity.SecondsToFullyConsumeNeeds);
        }

        private ResourceBlobFactoryBase BuildResourceBlobFactory() {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ResourceBlobFactory>();
            return newFactory;
        }

        private ComplexityLadderBase BuildComplexityLadder() {
            var hostingObject = new GameObject();

            var newLadder = hostingObject.AddComponent<ComplexityLadder>();
            var complexity1 = hostingObject.AddComponent<ComplexityDefinition>();
            var complexity2 = hostingObject.AddComponent<ComplexityDefinition>();

            complexity1.SetName("Complexity1");
            complexity1.SetProduction(ResourceSummary.BuildResourceSummary(
                complexity1.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 2)
            ));

            complexity1.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(complexity1.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)),
                ResourceSummary.BuildResourceSummary(complexity1.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.White, 1))
            });
            complexity1.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                complexity1.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            complexity2.SetName("Complexity2");
            complexity2.SetProduction(ResourceSummary.BuildResourceSummary(
                complexity2.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 2)
            ));
            complexity2.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(complexity2.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.White, 1))
            });
            complexity2.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                complexity2.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            newLadder.ComplexityHierarchy = new List<ComplexityDefinitionBase>() {
                complexity1, 
                complexity2
            };

            return newLadder;
        }

        private MockSocietyFactory BuildMockSocietyFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSocietyFactory>();
        }

        private MockHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayFactory>();
        }

        private ResourceBlob BuildResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ResourceBlob>();
        }

        private MockBlobDistributor BuildMockBlobDistributor() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobDistributor>();
        }

        #endregion

        #endregion

    }

}


