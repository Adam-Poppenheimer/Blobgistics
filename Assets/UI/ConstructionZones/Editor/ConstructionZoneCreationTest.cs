using System;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

using NUnit.Framework;

using Assets.Core;
using Assets.Map;
using Assets.UI.ConstructionZones.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.ConstructionZones.Editor {

    public class ConstructionZoneCreationTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedIntoUIControl_ConstructionPanelIsActivated_AndGivenTheClickedMapNodeAsItsLocation() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var locationToConstruct = new MapNodeUISummary();

            //Execution
            controlToTest.PushSelectEvent(locationToConstruct, eventData);

            //Validation
            Assert.That(constructionPanel.IsActivated, "ConstructionPanel was not activated");
            Assert.AreEqual(locationToConstruct, constructionPanel.LocationToConstruct, "ConstructionPanel has the wrong LocationToConstruct");
        }

        [Test]
        public void OnSelectEventPushedIntoUIControl_ConstructionPanelBuildingPermissionsAreSetProperly() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var locationToConstruct = new MapNodeUISummary();

            //Execution
            controlToTest.PushSelectEvent(locationToConstruct, eventData);

            //Validation
            Assert.NotNull(constructionPanel.LastPermissionsSet, "ConstructionPanel received no permissions");
            Assert.That(constructionPanel.LastPermissionsSet.Contains("Resource Depot"), "ConstructionPanel was not given permissions for Resource Depot");
            Assert.That(constructionPanel.LastPermissionsSet.Contains("Farmland"), "ConstructionPanel was not given permissions for Farmland");
            Assert.That(constructionPanel.LastPermissionsSet.Contains("Village"), "ConstructionPanel was not given permissions for Village");
        }

        [Test]
        public void ConstructionPanelRaisesConstructionRequest_ProperRequestIsForwardedToSimulationControl_AndConstructionPanelIsDeactivated() {
            //Setup
            var constructionPanel = BuildMockConstructionPanel();
            var simulationControl = BuildMockSimulationControl();

            string lastConstructionRequested = null;
            simulationControl.ConstructionRequested += delegate(object sender, StringEventArgs e) {
                lastConstructionRequested = e.Value;
            };

            var controlToTest = BuildUIControl();
            controlToTest.ConstructionPanel = constructionPanel;
            controlToTest.SimulationControl = simulationControl;

            var locationToConstruct = new MapNodeUISummary();
            constructionPanel.LocationToConstruct = locationToConstruct;

            //Execution and Validation
            constructionPanel.RaiseConstructionRequest("Resource Depot");
            Assert.AreEqual("Resource Depot", lastConstructionRequested, "Resource Depot construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.IsActivated, "ConstructionPanel was not deactivated after the Resource Depot request was sent");

            constructionPanel.Activate();
            constructionPanel.RaiseConstructionRequest("Farmland");
            Assert.AreEqual("Farmland", lastConstructionRequested, "Farmland construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.IsActivated, "ConstructionPanel was not deactivated after the Farmland request was sent");

            constructionPanel.Activate();
            constructionPanel.RaiseConstructionRequest("Village");
            Assert.AreEqual("Village", lastConstructionRequested, "Village construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.IsActivated, "ConstructionPanel was not deactivated after the Village request was sent");
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


