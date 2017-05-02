using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Map;
using Assets.Highways;

using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class HighwayControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSetHighwayUpkeepRequestCalled_TheCorrectHighwayHasItsUpkeepRequestsSetProperly() {
            //Setup
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
            var controlToTest = BuildHighwayControl();

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
        public void OnMethodsCalledWithInvalidID_AllMethodsDisplayErrorMessage_ButDoNotThrow() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private HighwayControl BuildHighwayControl() {
            var hostingObject = new GameObject();
            var newControl = hostingObject.AddComponent<HighwayControl>();

            newControl.HighwayFactory = BuildMockHighwayFactory();
            newControl.MapGraph = BuildMockMapGraph();

            return newControl;
        }

        private MockHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayFactory>();
        }

        private MockMapGraph BuildMockMapGraph() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockMapGraph>();
        }

        #endregion

        #endregion

    }

}
