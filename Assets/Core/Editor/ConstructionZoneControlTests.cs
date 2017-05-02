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
        public void OnGetAllPermittedConstructionZoneProjectsOnNodeCalled_AllProjectsCanBeCreatedUponTheGivenNode() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCanCreateConstructionZoneOnNodeIsCalled_ReturnsFalseIfAnyOtherFeatureExistsOnTheNode() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCreateConstructionZoneOnNodeIsCalled_ProperConstructionZoneIsCreatedOnTheProperNode() {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
