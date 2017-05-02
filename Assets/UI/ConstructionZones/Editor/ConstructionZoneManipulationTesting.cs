using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Core;
using Assets.ConstructionZones;
using Assets.UI.ConstructionZones.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.ConstructionZones.Editor {

    public class ConstructionZoneManipulationTesting {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedIntoUIControl_ConstructionZoneSummaryDisplayIsActivated_AndGivenTheClickedZone() {
            throw new NotImplementedException();
            //Setup
            var zonePanel = BuildMockConstructionZonePanel();
            //var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            //controlToTest.ConstructionZoneSummaryDisplay = zonePanel;
            //controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var zoneToSelect = new ConstructionZoneUISummary();

            //Execution
            controlToTest.PushSelectEvent(zoneToSelect, eventData);

            //Validation
            Assert.That(zonePanel.isActiveAndEnabled, "ConstructionZoneDisplay was not activated");
            Assert.AreEqual(zoneToSelect, zonePanel.CurrentSummary, "ConstructionZoneDisplay has the wrong SummaryToDisplay");
        }

        [Test]
        public void OnDestructionRequestedEventRaised_SimulationControlReceivesRequestToDestroyTheZone() {
            throw new NotImplementedException();
            //Setup
            var zonePanel = BuildMockConstructionZonePanel();
            //var simulationControl = BuildMockSimulationControl();
            int IDOfDestroyedZone = -1;
            /*simulationControl.OnConstructionZoneDestructionRequested += delegate(object sender, IntEventArgs e) {
                IDOfDestroyedZone = e.Value;
            };*/

            var controlToTest = BuildUIControl();
            //controlToTest.ConstructionZoneSummaryDisplay = zonePanel;
            //controlToTest.SimulationControl = simulationControl;

            var zoneToDestroy = new ConstructionZoneUISummary();
            zoneToDestroy.ID = 1000;

            zonePanel.Activate();
            zonePanel.CurrentSummary = zoneToDestroy;

            //Execution
            zonePanel.GenerateDestructionRequest();

            //Validation
            Assert.AreEqual(zoneToDestroy.ID, IDOfDestroyedZone, "SimulationControl was not asked to destroy the correct ID of ConstructionZone");
        }

        [Test]
        public void OnCloseRequestedEventRaised_SummaryDisplayIsCleared_AndDeactivated() {
            throw new NotImplementedException();
            //Setup
            var zonePanel = BuildMockConstructionZonePanel();
            //var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            //controlToTest.ConstructionZoneSummaryDisplay = zonePanel;
            //controlToTest.SimulationControl = simulationControl;

            zonePanel.Activate();

            //Execution
            zonePanel.GenerateDeactivationRequest();

            //Validation
            Assert.That(zonePanel.HasBeenCleared, "ZonePanel was not cleared");
            Assert.IsFalse(zonePanel.isActiveAndEnabled, "ZonePanel is still activated");
        }

        #endregion

        #region utilities

        private EventSystem BuildEventSystem() {
            var hostedObject = new GameObject();
            return hostedObject.AddComponent<EventSystem>();
        }

        private UIControl BuildUIControl() {
            var hostedObject = new GameObject();
            return hostedObject.AddComponent<UIControl>();
        }

        private MockConstructionZoneDisplaySummary BuildMockConstructionZonePanel() {
            var hostedObject = new GameObject();
            return hostedObject.AddComponent<MockConstructionZoneDisplaySummary>();
        }

        #endregion

        #endregion

    }

}
