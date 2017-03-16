using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

using NUnit.Framework;

using Assets.Core;
using Assets.Map;
using Assets.UI.ConstructionZones.ForTesting;

namespace Assets.UI.ConstructionZones.Editor {

    public class ConstructionZoneCreationTests {

        #region instance methods

        #region tests

        [Test]
        public void OnPointerClickEventPushedIntoUIControl_ConstructionPanelIsActivated_Cleared_AndGivenTheClickedMapNodeAsItsLocation() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var locationToConstruct = new MapNodeUISummary();

            //Execution
            controlToTest.PushPointerClickEvent(locationToConstruct, eventData);

            //Validation
            Assert.That(constructionPanel.HasBeenCleared, "ConstructionPanel was not cleared");
            Assert.That(constructionPanel.IsActivated, "ConstructionPanel was not activated");
            Assert.AreEqual(locationToConstruct, constructionPanel.LocationToConstruct, "ConstructionPanel has the wrong LocationToConstruct");
        }

        [Test]
        public void OnPointerClickEventPushedIntoUIControl_ConstructionPanelBuildingPermissionsAreSetProperly() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var locationToConstruct = new MapNodeUISummary();

            //Execution and Validation
            simulationControl.PermitResourceDepots = true;
            controlToTest.PushPointerClickEvent(locationToConstruct, eventData);

            Assert.That(constructionPanel.HasPermissionForResourceDepot, "ConstructionPanel lacks resourceDepot permission in check 1");

            simulationControl.PermitResourceDepots = false;
            controlToTest.PushPointerClickEvent(locationToConstruct, eventData);

            Assert.IsFalse(constructionPanel.HasPermissionForResourceDepot, "ConstructionPanel falsey permits resourceDepot in check 2");
        }

        [Test]
        public void OnConstructionPanelRaisesDepotConstructionRequestedEvent_ConstructResourceDepotRequestIsSentToSimulationControl_AndConstructionPanelIsCleareadAndDeactivated() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            bool receivedConstructionRequest = false;
            simulationControl.ResourceDepotConstructionRequested += delegate(object sender, EventArgs e) {
                receivedConstructionRequest = true;
            };
            simulationControl.PermitResourceDepots = true;

            var locationToConstruct = new MapNodeUISummary();
            locationToConstruct.ID = 50;
            constructionPanel.LocationToConstruct = locationToConstruct;

            //Execution
            constructionPanel.RaiseResourceDepotConstructionRequest();

            //Validation
            Assert.That(receivedConstructionRequest, "SimulationControl did not receive the construction request");
            Assert.IsTrue(constructionPanel.HasBeenCleared, "ConstructionPanel has not been cleared");
            Assert.IsFalse(constructionPanel.IsActivated, "ConstructionPanel is still activated");
        }

        [Test]
        public void ConstructionPanelRaisesCloseRequestedEvent_ConstructionPanelIsClearedAndDeactivated() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            //Execution
            constructionPanel.Activate();
            constructionPanel.RaiseCloseRequestedEvent();

            //Validation
            Assert.That(constructionPanel.HasBeenCleared, "ConstructionPanel has not been cleared");
            Assert.IsFalse(constructionPanel.IsActivated, "ConstructionPanel is still activated");
        }

        #endregion

        #region utilities

        private EventSystem BuildEventSystem() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<EventSystem>();
        }

        private UIControl BuildUIControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<UIControl>();
        }

        private MockZoneConstructionSimulationControl BuildMockSimulationControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockZoneConstructionSimulationControl>();
        }

        private MockConstructionPanel BuildMockConstructionPanel() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockConstructionPanel>();
        }

        #endregion

        #endregion

    }

}


