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
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.Textiles, true);
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.ServiceGoods,  false);
            controlToTest.SetHighwayUpkeepRequest(highway1.ID, ResourceType.HiTechGoods,   false);

            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.Food,   false);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.Textiles, false);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.ServiceGoods,  true);
            controlToTest.SetHighwayUpkeepRequest(highway2.ID, ResourceType.HiTechGoods,   true);

            //Validation
            Assert.That   (highway1.GetUpkeepRequestedForResource(ResourceType.Food        ), "Highway1 is not requesting food"            );
            Assert.That   (highway1.GetUpkeepRequestedForResource(ResourceType.Textiles    ), "Highway1 is not requesting Textiles"        );
            Assert.IsFalse(highway1.GetUpkeepRequestedForResource(ResourceType.ServiceGoods), "Highway1 is falsely requesting ServiceGoods");
            Assert.IsFalse(highway1.GetUpkeepRequestedForResource(ResourceType.HiTechGoods ), "Highway1 is falsely requesting HiTechGoods" );

            Assert.IsFalse(highway2.GetUpkeepRequestedForResource(ResourceType.Food        ), "Highway2 is falsely requesting food"    );
            Assert.IsFalse(highway2.GetUpkeepRequestedForResource(ResourceType.Textiles    ), "Highway2 is falsely requesting Textiles");
            Assert.That   (highway2.GetUpkeepRequestedForResource(ResourceType.ServiceGoods), "Highway2 is not requesting ServiceGoods");
            Assert.That   (highway2.GetUpkeepRequestedForResource(ResourceType.HiTechGoods ), "Highway2 is not requesting HiTechGoods" );
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
            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway1.ID, ResourceType.Textiles, true);

            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway1.ID, ResourceType.Food, false);
            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway1.ID, ResourceType.Textiles, true);

            //Highway 2

            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway2.ID, ResourceType.Food, true);
            controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(highway2.ID, ResourceType.ServiceGoods, true);

            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway2.ID, ResourceType.Food, false);
            controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(highway2.ID, ResourceType.ServiceGoods, true);

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
                highway1.GetPullingPermissionForFirstEndpoint(ResourceType.Textiles),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "1", "First", ResourceType.Textiles
                )
            );
            Assert.IsFalse(
                highway1.GetPullingPermissionForFirstEndpoint(ResourceType.ServiceGoods),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "1", "First", ResourceType.ServiceGoods
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
                highway1.GetPullingPermissionForSecondEndpoint(ResourceType.Textiles),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "1", "Second", ResourceType.Textiles
                )
            );
            Assert.IsFalse(
                highway1.GetPullingPermissionForSecondEndpoint(ResourceType.ServiceGoods),
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "1", "Second", ResourceType.ServiceGoods
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
                highway2.GetPullingPermissionForFirstEndpoint(ResourceType.Textiles), 
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "2", "First", ResourceType.Textiles
                )
            );
            Assert.That(
                highway2.GetPullingPermissionForFirstEndpoint(ResourceType.ServiceGoods), 
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "2", "First", ResourceType.ServiceGoods
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
                highway2.GetPullingPermissionForSecondEndpoint(ResourceType.Textiles), 
                string.Format(
                    "Highway {0} falsely has pulling permission on {1} endpoint for resource {2}",
                    "2", "Second", ResourceType.Textiles
                )
            );
            Assert.That(
                highway2.GetPullingPermissionForSecondEndpoint(ResourceType.ServiceGoods),
                string.Format(
                    "Highway {0} lacks pulling permission on {1} endpoint for resource {2}",
                    "2", "Second", ResourceType.ServiceGoods
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
            //Setup
            var controlToTest = BuildHighwayControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution and Validation
            DebugMessageData lastMessage;

            Assert.DoesNotThrow(delegate() {
                controlToTest.CanConnectNodesWithHighway(42, 42);
            }, "CanConnectNodesWithHighway threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "CanConnectNodesWithHighway did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.ConnectNodesWithHighway(42, 42);
            }, "ConnectNodesWithHighway threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "ConnectNodesWithHighway did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayPullingPermissionOnFirstEndpointForResource(42, ResourceType.Food, true);
            }, "SetHighwayPullingPermissionOnFirstEndpointForResource threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "SetHighwayPullingPermissionOnFirstEndpointForResource did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayPullingPermissionOnSecondEndpointForResource(42, ResourceType.Food, true);
            }, "SetHighwayPullingPermissionOnSecondEndpointForResource threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "SetHighwayPullingPermissionOnSecondEndpointForResource did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayPriority(42, 5);
            }, "SetHighwayPriority threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "SetHighwayPriority did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.SetHighwayUpkeepRequest(42, ResourceType.Food, true);
            }, "SetHighwayUpkeepRequest threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "SetHighwayUpkeepRequest did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyHighway(42);
            }, "DestroyHighway threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "DestroyHighway did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
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
