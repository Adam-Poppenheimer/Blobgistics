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
            throw new NotImplementedException();
        }

        [Test]
        public void OnConstructionPanelRaisesDepotConstructionRequestedEvent_ConstructResourceDepotRequestIsSentToSimulationControl_AndConstructionPanelIsCleareadAndDeactivated() {
            throw new NotImplementedException();
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


