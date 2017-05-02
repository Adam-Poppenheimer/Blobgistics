using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Highways;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class BlobHighwayStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedIntoUIControl_SummaryDisplayIsActivated_AndGivenTheProperSummary() {
            throw new NotImplementedException();
            //Setup
            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            //var simulationControl = BuildMockSimulationControl();
            
            var uiControl = BuildUIControl();
            //uiControl.HighwaySummaryDisplay = summaryDisplay;
            //uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new BlobHighwayUISummary();
            summaryToPush.ID = 1;
            summaryToPush.Priority = 15;

            //Execution
            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Validation
            Assert.AreEqual(summaryToPush, summaryDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(summaryDisplay.isActiveAndEnabled, "The display was not activated");
        }

        [Test]
        public void OnSummaryRaisesPriorityChangedEvent_ProperPriorityChangeRequestIsSentToSimulationControl() {
            throw new NotImplementedException();
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            //var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            //uiControl.HighwaySummaryDisplay = summaryDisplay;
            //uiControl.SimulationControl = simulationControl;

            //Execution
            summaryDisplay.ChangePriority(30);

            //Validation
            /*Assert.AreEqual(1, simulationControl.PriorityChangeRequestsPassed, "The SimulationControl was never passed a changeRequest");
            Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested, "The SimulationControl was passed the incorrect ID");
            Assert.AreEqual(30, simulationControl.LastHighwayPriorityRequested, "The SimulationControl was passed the incorrect Priority");*/
        }

        [Test]
        public void OnSummaryDisplayRaisesFirstEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            throw new NotImplementedException();
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            //var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            //uiControl.HighwaySummaryDisplay = summaryDisplay;
            //uiControl.SimulationControl = simulationControl;

            //Execution and Validation
            int totalChangeRequestCount = 0;
            /*foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeFirstEndpointPermission(resourceType, true);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.FirstEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.FirstEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsTrue(simulationControl.FirstEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeFirstEndpointPermission(resourceType, false);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.FirstEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.FirstEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsFalse(simulationControl.FirstEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }*/
        }

        [Test]
        public void OnSummaryDisplayRaisesSecondEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            throw new NotImplementedException();
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            summaryDisplay.CurrentSummary = summaryWithin;

            //var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            //uiControl.HighwaySummaryDisplay = summaryDisplay;
            //uiControl.SimulationControl = simulationControl;

            //Execution and Validation
            int totalChangeRequestCount = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeSecondEndpointPermission(resourceType, true);
                ++totalChangeRequestCount;
                /*Assert.AreEqual(totalChangeRequestCount, simulationControl.SecondEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.SecondEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsTrue(simulationControl.SecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");*/
            }
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeSecondEndpointPermission(resourceType, false);
                ++totalChangeRequestCount;
                /*Assert.AreEqual(totalChangeRequestCount, simulationControl.SecondEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.SecondEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsFalse(simulationControl.SecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");*/
            }
        }

        [Test]
        public void OnHighwayDisplayRaisesUpkeepRequestedEvent_ProperCommandIsSentToSimulationControl() {
            throw new NotImplementedException();
            //Setup
            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            //var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            //uiControl.HighwaySummaryDisplay = summaryDisplay;
            //uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new BlobHighwayUISummary();
            summaryToPush.ID = 1;

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseUpkeepRequestedEvent(ResourceType.Food, true);
            summaryDisplay.RaiseUpkeepRequestedEvent(ResourceType.Yellow, false);
            summaryDisplay.RaiseUpkeepRequestedEvent(ResourceType.White, true);
            summaryDisplay.RaiseUpkeepRequestedEvent(ResourceType.Blue, false);

            //Validation
            /*Assert.IsTrue (simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Food],   
                "simulationControl was not given the proper upkeep request for food"  );

            Assert.IsFalse(simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Yellow], 
                "simulationControl was not given the proper upkeep request for yellow");

            Assert.IsTrue (simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.White],  
                "simulationControl was not given the proper upkeep request for white" );

            Assert.IsFalse(simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Blue],   
                "simulationControl was not given the proper upkeep request for blue"  );*/
        }

        #endregion

        #region utilities

        private EventSystem BuildEventSystem() {
            return (new GameObject()).AddComponent<EventSystem>();
        }

        private BlobHighwayStandardEventReceiver BuildHighwayReceiver() {
            return (new GameObject()).AddComponent<BlobHighwayStandardEventReceiver>();
        }

        private MockHighwaySummaryDisplay BuildMockHighwaySummaryDisplay() {
            return (new GameObject()).AddComponent<MockHighwaySummaryDisplay>();
        }

        #endregion

        #endregion

    }

}
