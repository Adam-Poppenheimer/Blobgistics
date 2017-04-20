using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.ConstructionZones;
using Assets.Highways;
using Assets.ResourceDepots;
using Assets.Societies;
using Assets.HighwayManager;

using Assets.Core.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.Editor {

    public class SimulationControlTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnSetHighwayUpkeepRequestCalled_TheCorrectHighwayHasItsUpkeepRequestsSetProperly() {
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
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.Food,   true);
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.Yellow, true);
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.White,  false);
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.Blue,   false);

            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.Food,   false);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.Yellow, false);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.White,  true);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.Blue,   true);

            //Validation
            Assert.That   (highway1.IsRequestingFood,   "Highway1 is not requesting food");
            Assert.That   (highway1.IsRequestingYellow, "Highway1 is not requesting yellow");
            Assert.IsFalse(highway1.IsRequestingWhite,  "Highway1 is falsely requesting white");
            Assert.IsFalse(highway1.IsRequestingBlue,   "Highway1 is falsely requesting blue");

            Assert.IsFalse(highway2.IsRequestingFood,   "Highway2 is falsely requesting food");
            Assert.IsFalse(highway2.IsRequestingYellow, "Highway2 is falsely requesting yellow");
            Assert.That   (highway2.IsRequestingWhite,  "Highway2 is not requesting white");
            Assert.That   (highway2.IsRequestingBlue,   "Highway2 is not requesting blue");
        }

        [Test]
        public void OnSetHighwayUpkeepRequestCalled_AndHighwayIDIsInvalid_DisplaysErrorMessage_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayUpkeepRequest(42, ResourceType.Food, true);
            });

            //Validation

            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not just one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages.Last().LogType, "The message received was not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

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

            ConstructionProjectBase project;
            constructionZoneFactory.TryGetProjectOfName("Resource Depot", out project);
            constructionZoneFactory.BuildConstructionZone(constructionNode, project);

            //Validation

            //Returns true when the MapNode is empty
            Assert.That(controlToTest.CanCreateConstructionSiteOnNode(freeNode.ID, "Resource Depot"),
                "Was not permitted to construct on freeNode");

            //Return false when there is a society on the MapNode
            Assert.IsFalse(controlToTest.CanCreateConstructionSiteOnNode(societyNode.ID, "Resource Depot"),
                "Falsely permitted to construct on depotNode");

            //Returns false when there is another depot on the MapNode
            Assert.IsFalse(controlToTest.CanCreateConstructionSiteOnNode(depotNode.ID, "Resource Depot"),
                "Falsely permitted to construct on societyNode");

            //Returns false when there is a ConstructionSite on the MapNode
            Assert.IsFalse(controlToTest.CanCreateConstructionSiteOnNode(constructionNode.ID, "Resource Depot"),
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
            controlToTest.CreateConstructionSiteOnNode(nodeToPlaceUpon.ID, "Resource Depot");

            //Validation
            
            Assert.True(constructionZoneFactory.HasConstructionZoneAtLocation(nodeToPlaceUpon),
                "constructionZoneFactory does not register a ConstructionZone at the specified location");

            ConstructionProjectBase project;
            constructionZoneFactory.TryGetProjectOfName("Resource Depot", out project);
            var zoneAtLocation = constructionZoneFactory.GetConstructionZoneAtLocation(nodeToPlaceUpon);

            Assert.AreEqual(project, zoneAtLocation.CurrentProject,
                "The construction zone at the specified location has the wrong CurrentProject");
        }

        [Test]
        public void OnDestroyConstructionZoneIsCalled_SpecifiedConstructionZoneObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var mapGraph = controlToTest.MapGraph;
            var constructionZoneFactory = controlToTest.ConstructionZoneFactory;

            var nodeToPlaceUpon = mapGraph.BuildNode(Vector3.zero);

            controlToTest.CreateConstructionSiteOnNode(nodeToPlaceUpon.ID, "Resource Depot");

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
        public void OnSetAscensionPermissionForSocietyIsCalled_SpecifiedSocietyHasItsAscensionIsPermittedFieldChanged() {
            Assert.Ignore("This test is unimplemented but also not considered critical");
        }

        [Test]
        public void OnTickSimulationIsCalled_AllSimulationTickingIsPerformed() {
            //Setup
            var societyFactory = BuildMockSocietyFactory();
            var blobDistributor = BuildMockBlobDistributor();
            var blobFactory = BuildMockBlobFactory();

            float amountTickedOnSocietyFactory = 0f;
            societyFactory.FactoryTicked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnSocietyFactory = e.Value;
            };

            float amountTickedOnBlobDistributor = 0f;
            blobDistributor.Ticked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnBlobDistributor = e.Value;
            };

            float amountTickedOnBlobFactory = 0f;
            blobFactory.Ticked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnBlobFactory = e.Value;
            };

            var controlToTest = BuildSimulationControl();
            controlToTest.SocietyFactory = societyFactory;
            controlToTest.BlobDistributor = blobDistributor;
            controlToTest.BlobFactory = blobFactory;

            //Execution
            controlToTest.TickSimulation(5f);

            //Validation
            Assert.AreEqual(5f, amountTickedOnSocietyFactory,  "Incorrect amount ticked on SocietyFactory");
            Assert.AreEqual(5f, amountTickedOnBlobDistributor, "Incorrect amount ticked on BlobDistributor");
            Assert.AreEqual(5f, amountTickedOnBlobFactory,     "Incorrect amount ticked on BlobFactory");
        }

        [Test]
        public void OnDestroyResourceDepotIsCalled_SpecifiedDepotIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();
            var nodeToPlaceUpon = controlToTest.MapGraph.BuildNode(Vector3.zero);

            var newDepot = controlToTest.ResourceDepotFactory.ConstructDepotAt(nodeToPlaceUpon);
            var depotName = "SimulationControlTest's Destroyed Depot";
            var depotID = newDepot.ID;
            newDepot.name = depotName;


            //Execution
            controlToTest.DestroyResourceDepotOfID(newDepot.ID);

            //Validation
            Assert.Null(GameObject.Find(depotName), "There still exists a GameObject with the destroyed depot's name");
            Assert.Null(controlToTest.ResourceDepotFactory.GetDepotOfID(depotID), "DepotFactory still recognizes the destroyed depot");
        }

        [Test]
        public void OnDestroyHighwayManagerIsCalled_SpecifiedManagerObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSimulationControl();
            var nodeToPlaceUpon = controlToTest.MapGraph.BuildNode(Vector3.zero);

            var newManager = controlToTest.HighwayManagerFactory.ConstructHighwayManagerAtLocation(nodeToPlaceUpon);
            var managerName = "SimulationControlTest's Destroyed HighwayManager";
            var managerID = newManager.ID;
            newManager.name = managerName;


            //Execution
            controlToTest.DestroyHighwayManagerOfID(newManager.ID);

            //Validation
            Assert.Null(GameObject.Find(managerName), "There still exists a GameObject with the destroyed manager's name");
            Assert.Null(controlToTest.ResourceDepotFactory.GetDepotOfID(managerID), "HighwayManagerFactory still recognizes the destroyed manager");
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
                controlToTest.CanCreateConstructionSiteOnNode(42, "Resource Depot");
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
                controlToTest.CreateConstructionSiteOnNode(42, "Resource Depot");
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
        public void OnDestroyResourceDepotIsCalledOnInvalidID_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyResourceDepotOfID(42);
            });

            //Validation
            Assert.AreEqual(1, insertionHandler.StoredMessages.Count, "There was not one message received");
            Assert.AreEqual(LogType.Error, insertionHandler.StoredMessages[0].LogType, "The message logged is not an error message");

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroyHighwayManagerIsCalledOnAnInvalidID_AnErrorIsLogged_ButNoExceptionIsThrown() {
            //Setup
            var controlToTest = BuildSimulationControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution
            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyHighwayManagerOfID(42);
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

        private MapGraph BuildMapGraph(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();

            var newMapGraph = hostingObject.AddComponent<MapGraph>();
            var newBlobSiteFactory = hostingObject.AddComponent<BlobSiteFactory>();
            var newBlobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();

            newBlobSitePrivateData.SetBlobRealignmentSpeedPerSecond(1f);
            newBlobSitePrivateData.SetAlignmentStrategy(hostingObject.AddComponent<BoxyBlobAlignmentStrategy>());
            newBlobSitePrivateData.SetBlobFactory(blobFactory);

            newBlobSiteFactory.BlobSitePrivateData = newBlobSitePrivateData;

            newMapGraph.BlobSiteFactory = newBlobSiteFactory;

            return newMapGraph;
        }

        private BlobHighwayFactory BuildHighwayFactory(MapGraphBase mapGraph, ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobHighwayFactory>();
            var newBlobTubeFactory = hostingObject.AddComponent<BlobTubeFactory>();

            var newTubePrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            newTubePrivateData.SetBlobFactory(blobFactory);
            newBlobTubeFactory.TubePrivateData = newTubePrivateData;

            newFactory.MapGraph = mapGraph;
            newFactory.BlobTubeFactory = newBlobTubeFactory;
            newFactory.BlobFactory = blobFactory;
            newFactory.StartingProfile = BuildBlobHighwayProfile(1f, 10, 1f);

            return newFactory;
        }

        private ConstructionZoneFactory BuildConstructionZoneFactory(ResourceDepotFactoryBase depotFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ConstructionZoneFactory>();

            var resourceDepotProject = hostingObject.AddComponent<ResourceDepotConstructionProject>();
            resourceDepotProject.Cost = ResourceSummary.BuildResourceSummary(hostingObject);
            resourceDepotProject.name = "Resource Depot";

            newFactory.AvailableProjects = new List<ConstructionProjectBase>() {
                resourceDepotProject
            };
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

        private SimulationControl BuildSimulationControl() {
            var hostingObject = new GameObject();
            var newControl = hostingObject.AddComponent<SimulationControl>();
            var newBlobFactory = BuildResourceBlobFactory();
            var newDepotFactory = BuildDepotFactory();

            newControl.MapGraph = BuildMapGraph(newBlobFactory);
            newControl.HighwayFactory = BuildHighwayFactory(newControl.MapGraph, newBlobFactory);
            newControl.SocietyFactory = BuildSocietyFactory(newBlobFactory);
            newControl.ConstructionZoneFactory = BuildConstructionZoneFactory(newDepotFactory);
            newControl.HighwayManagerFactory = BuildHighwayManagerFactory(newControl.MapGraph, newControl.HighwayFactory);
            
            newControl.ResourceDepotFactory = newDepotFactory;

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

        private ResourceBlobBase BuildResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ResourceBlob>();
        }

        private MockBlobDistributor BuildMockBlobDistributor() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobDistributor>();
        }

        private MockResourceBlobFactory BuildMockBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        private BlobHighwayProfile BuildBlobHighwayProfile(float blobSpeedPerSecond, int capacity, float BlobPullCooldownInSeconds) {
            var hostingObject = new GameObject();
            var newProfile = hostingObject.AddComponent<BlobHighwayProfile>();

            newProfile.SetBlobSpeedPerSecond(blobSpeedPerSecond);
            newProfile.SetCapacity(capacity);
            newProfile.SetBlobPullCooldownInSeconds(BlobPullCooldownInSeconds);

            return newProfile;
        }

        private HighwayManagerFactoryBase BuildHighwayManagerFactory(MapGraphBase mapGraph, BlobHighwayFactoryBase highwayFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<HighwayManagerFactory>();
            var newPrivateData = hostingObject.AddComponent<HighwayManagerPrivateData>();

            newPrivateData.SetNeedStockpileCoefficient(1);
            newPrivateData.SetSecondsToPerformConsumption(10f);

            newFactory.ManagementRadius = 2;
            newFactory.ManagerPrivateData = newPrivateData;
            newFactory.MapGraph = mapGraph;
            newFactory.HighwayFactory = highwayFactory;

            return newFactory;
        }

        #endregion

        #endregion

    }

}


