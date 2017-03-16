using System;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Core;
using Assets.Highways;

using Assets.UI.Highways.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways.Editor {

    public class HighwayDisplayUITests {

        #region instance methods

        #region tests

        [Test]
        public void OnPointerClickEventWithHighwaySummaryPushedIntoUIControl_SummaryDisplayIsCleared_GivenTheProperSummary_AndUpdated() {
            //Setup
            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            
            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var summaryToPush = new BlobHighwayUISummary();
            summaryToPush.ID = 1;
            summaryToPush.Priority = 15;

            //Execution
            uiControl.PushPointerClickEvent(summaryToPush, eventData);

            //Validation
            Assert.AreEqual(summaryToPush, summaryDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(summaryDisplay.WasCleared, "The display was not cleared");
            Assert.That(summaryDisplay.WasUpdated, "The display was not updated");
        }

        [Test]
        public void OnSummaryRaisesPriorityChangedEvent_ProperPriorityChangeRequestIsSentToSimulationControl() {
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            //Execution
            summaryDisplay.ChangePriority(30);

            //Validation
            Assert.AreEqual(1, simulationControl.PriorityChangeRequestsPassed, "The SimulationControl was never passed a changeRequest");
            Assert.AreEqual(summaryWithin.ID, simulationControl.LastIDRequested, "The SimulationControl was passed the incorrect ID");
            Assert.AreEqual(30, simulationControl.LastPriorityRequested, "The SimulationControl was passed the incorrect Priority");
        }

        [Test]
        public void OnSummaryDisplayRaisesFirstEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            //Execution and Validation
            int totalChangeRequestCount = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeFirstEndpointPermission(resourceType, true);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.FirstEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.LastFirstEndpointResourceTypeChangeRequested,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsTrue(simulationControl.LastFirstEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeFirstEndpointPermission(resourceType, false);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.FirstEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.LastFirstEndpointResourceTypeChangeRequested,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsFalse(simulationControl.LastFirstEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
        }

        [Test]
        public void OnSummaryDisplayRaisesSecondEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            //Execution and Validation
            int totalChangeRequestCount = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeSecondEndpointPermission(resourceType, true);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.SecondEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.LastSecondEndpointResourceTypeChangeRequested,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsTrue(simulationControl.LastSecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeSecondEndpointPermission(resourceType, false);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.SecondEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.LastSecondEndpointResourceTypeChangeRequested,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsFalse(simulationControl.LastSecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
        }

        #endregion

        #region utilities

        private MockBlobHighwaySummaryDisplay BuildMockHighwaySummaryDisplay() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobHighwaySummaryDisplay>();
        }

        private HighwayDisplayMockSimulationControl BuildMockSimulationControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayDisplayMockSimulationControl>();
        }

        private UIControl BuildUIControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<UIControl>();
        }

        private EventSystem BuildEventSystem() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<EventSystem>();
        }

        #endregion

        #endregion

    }

}





