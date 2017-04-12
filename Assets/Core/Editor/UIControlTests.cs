using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Core;
using Assets.ResourceDepots;
using Assets.Core.ForTesting;
using Assets.UI.ResourceDepots;

namespace Assets.Core.Editor {

    
    public class UIControlTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnSocietyDisplayRaisesAscensionPermissionChangeRequestedEvent_RequestIsSentToSimulationControlProperly() {
            Assert.Ignore("This test is unimplemented but not considered critical");
        }

        [Test]
        public void OnSelectEventPushedWithResourceDepotUISummary_DepotDisplayIsGivenTheSummary_AndActivated() {
            //Setup
            var summaryDisplay = BuildMockDepotDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.DepotSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 1;

            //Execution
            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Validation
            Assert.AreEqual(summaryToPush, summaryDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(summaryDisplay.IsActive, "The display was not activated");
        }

        [Test]
        public void OnResourceDepotDisplayRaisesDestructionRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var summaryDisplay = BuildMockDepotDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.DepotSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 1;

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(summaryToPush.ID, simulationControl.IDOfRequestedDepot, "SimulationControl did not receive the proper ID");
        }

        [Test]
        public void OnResourceDepotDisplayRaisesDestructionRequestedEvent_DepotDisplayIsDeactivated() {
            //Setup
            var summaryDisplay = BuildMockDepotDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.DepotSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 1;

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.IsFalse(summaryDisplay.IsActive, "SummaryDisplay is still active");
        }

        #endregion

        #endregion

        #region utilities

        private UIControl BuildUIControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<UIControl>();
        }

        private MockSimulationControl BuildMockSimulationControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSimulationControl>();
        }

        private MockResourceDepotSummaryDisplay BuildMockDepotDisplay() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceDepotSummaryDisplay>();
        }

        private EventSystem BuildEventSystem() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<EventSystem>();
        }

        #endregion

        #endregion

    }

}
