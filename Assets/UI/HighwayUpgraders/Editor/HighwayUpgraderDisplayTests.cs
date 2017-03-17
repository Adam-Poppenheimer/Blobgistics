using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

using NUnit.Framework;

using Assets.HighwayUpgraders;
using Assets.Core;
using Assets.UI.HighwayUpgraders.ForTesting;

using UnityCustomUtilities.Extensions;


namespace Assets.UI.HighwayUpgraders.Editor {

    public class HighwayUpgraderDisplayTests {

        #region instance methods

        #region tests

        [Test]
        public void OnHighwayUpgraderPointerClickEventPushedIntoUIControl_SummaryIsCleared_Activated_AndGivenTheClickedUpgraderSummary() {
            //Setup
            var summaryDisplay = BuildMockUpgraderSummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayUpgraderSummaryDisplay = summaryDisplay;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var summaryToPush = new HighwayUpgraderUISummary();
            
            //Execution
            controlToTest.PushPointerClickEvent(summaryToPush, eventData);

            //Validation
            Assert.That(summaryDisplay.HasBeenCleared, "SummaryDisplay was not cleared");
            Assert.That(summaryDisplay.IsActivated, "SummaryDisplay was not activated");
            Assert.AreEqual(summaryToPush, summaryDisplay.SummaryToDisplay, "SummaryDisplay has the wrong SummaryToDisplay");
        }

        [Test]
        public void OnHighwayUpgraderSummaryDisplayRaisesDestructionRequestedEvent_SimulationControlReceivesRequestToDestroyTheZone() {
            //Setup
            var summaryDisplay = BuildMockUpgraderSummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            int IDOfDestroyedZone = -1;
            simulationControl.OnHighwayUpgraderDestructionRequested += delegate(object sender, IntEventArgs e) {
                IDOfDestroyedZone = e.Value;
            };

            var controlToTest = BuildUIControl();
            controlToTest.HighwayUpgraderSummaryDisplay = summaryDisplay;
            controlToTest.SimulationControl = simulationControl;

            var summaryToDestroy = new HighwayUpgraderUISummary();
            summaryToDestroy.ID = 1000;

            summaryDisplay.Activate();
            summaryDisplay.SummaryToDisplay = summaryToDestroy;

            //Execution
            summaryDisplay.GenerateDestructionRequest();

            //Validation
            Assert.AreEqual(summaryToDestroy.ID, IDOfDestroyedZone, "SimulationControl was not asked to destroy the correct ID");
        }

        [Test]
        public void OnHighwayUpgraderSummaryDisplayRaisesCloseRequestedEvent_SummaryDisplayIsCleared_AndDeactivated() {
            //Setup
            var summaryDisplay = BuildMockUpgraderSummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayUpgraderSummaryDisplay = summaryDisplay;
            controlToTest.SimulationControl = simulationControl;

            summaryDisplay.Activate();

            //Execution
            summaryDisplay.GenerateCloseRequest();

            //Validation
            Assert.That(summaryDisplay.HasBeenCleared, "SummaryDisplay was not cleared");
            Assert.IsFalse(summaryDisplay.IsActivated, "SummaryDisplay is still activated");
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

        private MockSimulationControl BuildMockSimulationControl() {
            var hostedObject = new GameObject();
            return hostedObject.AddComponent<MockSimulationControl>();
        }

        private MockHighwayUpgraderSummaryDisplay BuildMockUpgraderSummaryDisplay() {
            var hostedObject = new GameObject();
            return hostedObject.AddComponent<MockHighwayUpgraderSummaryDisplay>();
        }

        #endregion

        #endregion

    }

}


