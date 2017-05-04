using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.ConstructionZones;
using Assets.HighwayManager;
using Assets.Map;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class ConstructionZoneControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnCanCreateConstructionZoneOnNodeIsCalled_ReturnsFalseIfAnyOtherFeatureExistsOnTheNode_AndTrueOtherwise() {
            //Setup
            var controlToTest = BuildConstructionZoneControl();

            var mapGraph = controlToTest.MapGraph;

            var nodeWithConstructionZone = mapGraph.BuildNode(Vector3.zero);
            var nodeWithResourceDepot    = mapGraph.BuildNode(Vector3.zero);
            var nodeWithSociety          = mapGraph.BuildNode(Vector3.zero);
            var nodeWithHighwayManager   = mapGraph.BuildNode(Vector3.zero);
            var emptyNode = mapGraph.BuildNode(Vector3.zero);
            
            controlToTest.ConstructionZoneFactory.BuildConstructionZone(nodeWithConstructionZone,
                controlToTest.ConstructionZoneFactory.GetAvailableProjects().First());
            controlToTest.ResourceDepotFactory.ConstructDepotAt(nodeWithResourceDepot);
            controlToTest.SocietyFactory.ConstructSocietyAt(nodeWithSociety, controlToTest.SocietyFactory.StandardComplexityLadder);
            controlToTest.HighwayManagerFactory.ConstructHighwayManagerAtLocation(nodeWithHighwayManager);

            //Execution and validation
            Assert.IsFalse(controlToTest.CanCreateConstructionZoneOnNode(nodeWithConstructionZone.ID, "Village"),
                "ConstructionZoneControl falsely permits the creation of a ConstructionZone on a MapNode with a ConstructionZone");
            Assert.IsFalse(controlToTest.CanCreateConstructionZoneOnNode(nodeWithResourceDepot.ID, "Village"),
                "ConstructionZoneControl falsely permits the creation of a ConstructionZone on a MapNode with a ResourceDepot");
            Assert.IsFalse(controlToTest.CanCreateConstructionZoneOnNode(nodeWithSociety.ID, "Village"),
                "ConstructionZoneControl falsely permits the creation of a ConstructionZone on a MapNode with a Society");
            Assert.IsFalse(controlToTest.CanCreateConstructionZoneOnNode(nodeWithHighwayManager.ID, "Village"),
                "ConstructionZoneControl falsely permits the creation of a ConstructionZone on a MapNode with a HighwayManager");
            Assert.IsTrue(controlToTest.CanCreateConstructionZoneOnNode(emptyNode.ID, "Village"),
                "ConstructionZoneControl does not permit the creation of a ConstructionZone on a MapNode with nothing on it");
        }

        [Test]
        public void OnCreateConstructionZoneOnNodeIsCalled_AndCanCreateConstructionZoneOnNodeReturnsTrue_ProperRequestSentToConstructionZoneFactory() {
            //Setup
            var controlToTest = BuildConstructionZoneControl();
            var mockZoneFactory = controlToTest.ConstructionZoneFactory as MockConstructionZoneFactory;

            MapNodeBase locationRequestedForZone = null;
            ConstructionProjectBase projectRequestedForZone = null;
            mockZoneFactory.BuildConstructionZoneCalled += delegate(MapNodeBase location, ConstructionProjectBase project) {
                locationRequestedForZone = location;
                projectRequestedForZone = project;
            };

            var emptyNode = controlToTest.MapGraph.BuildNode(Vector3.zero);

            //Execution
            controlToTest.CreateConstructionZoneOnNode(emptyNode.ID, "Village");

            //Validation
            Assert.AreEqual(emptyNode, locationRequestedForZone, "ConstructionZoneFactory was given an incorrect location");
            Assert.AreEqual(mockZoneFactory.VillageProject, projectRequestedForZone, "ConstructionZoneFactory was given an incorrect project");
        }

        [Test]
        public void OnCreateConstructionZoneOnNodeIsCalled_AndCanCreateConstructionZoneOnNodeReturnsFalse_DisplaysError_ButDoesNotThrow() {
            //Setup
            var controlToTest = BuildConstructionZoneControl();
            var zoneFactory = controlToTest.ConstructionZoneFactory;

            var mapGraph = controlToTest.MapGraph;

            var nodeWithConstructionZone = mapGraph.BuildNode(Vector3.zero);
            ConstructionProjectBase project;
            zoneFactory.TryGetProjectOfName("Village", out project);
            zoneFactory.BuildConstructionZone(nodeWithConstructionZone, project);

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution and Validation
            DebugMessageData lastMessage;

            Assert.DoesNotThrow(delegate() {
                controlToTest.CreateConstructionZoneOnNode(nodeWithConstructionZone.ID, "Village");
            }, "CreateConstructionZoneOnNode threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "CreateConstructionZoneOnNode did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        [Test]
        public void OnDestroyConstructionZoneIsCalled_SpecifiedConstructionZoneObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildConstructionZoneControl();

            var mapGraph = controlToTest.MapGraph;
            var constructionZoneFactory = controlToTest.ConstructionZoneFactory;

            var nodeToPlaceUpon = mapGraph.BuildNode(Vector3.zero);

            controlToTest.CreateConstructionZoneOnNode(nodeToPlaceUpon.ID, "Resource Depot");

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
        public void OnMethodsCalledWithInvalidID_AllMethodsDisplayErrorMessage_ButDoNotThrow() {
            //Setup
            var controlToTest = BuildConstructionZoneControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution and Validation
            DebugMessageData lastMessage;

            Assert.DoesNotThrow(delegate() {
                controlToTest.GetAllPermittedConstructionZoneProjectsOnNode(42);
            }, "GetAllPermittedConstructionZoneProjectsOnNode threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "GetAllPermittedConstructionZoneProjectsOnNode did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.CanCreateConstructionZoneOnNode(42, "Village");
            }, "CanCreateConstructionZoneOnNode threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "CanCreateConstructionZoneOnNode did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.CreateConstructionZoneOnNode(42, "Village");
            }, "CreateConstructionZoneOnNode threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "CreateConstructionZoneOnNode did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyConstructionZone(42);
            }, "DestroyConstructionZone threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "DestroyConstructionZone did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        #endregion

        #region utilities

        private ConstructionZoneControl BuildConstructionZoneControl() {
            var hostingObject = new GameObject();
            var newControl = hostingObject.AddComponent<ConstructionZoneControl>();

            newControl.ConstructionZoneFactory = BuildMockConstructionZoneFactory();
            newControl.ResourceDepotFactory = BuildMockResourceDepotFactory();
            newControl.SocietyFactory = BuildMockSocietyFactory();
            newControl.MapGraph = BuildMockMapGraph();
            newControl.HighwayManagerFactory = BuildMockHighwayManagerFactory();

            return newControl;
        }

        private ConstructionZoneFactoryBase BuildMockConstructionZoneFactory() {
            return (new GameObject()).AddComponent<MockConstructionZoneFactory>();
        }

        private ResourceDepotFactoryBase BuildMockResourceDepotFactory() {
            return (new GameObject()).AddComponent<MockResourceDepotFactory>();
        }

        private SocietyFactoryBase BuildMockSocietyFactory() {
            return (new GameObject()).AddComponent<MockSocietyFactory>();
        }

        private MapGraphBase BuildMockMapGraph() {
            return (new GameObject()).AddComponent<MockMapGraph>();
        }

        private HighwayManagerFactoryBase BuildMockHighwayManagerFactory() {
            return (new GameObject()).AddComponent<MockHighwayManagerFactory>();
        }

        #endregion

        #endregion

    }

}
