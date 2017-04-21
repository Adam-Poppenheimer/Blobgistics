using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Blobs;
using Assets.ResourceDepots;
using Assets.HighwayManager;
using Assets.Highways;
using Assets.Core.ForTesting;
using Assets.Map;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.Editor {

    public class UIControlTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnSocietyDisplayRaisesAscensionPermissionChangeRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var summaryDisplay = BuildMockSocietySummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.SocietySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(1);
            var summaryToPush = new SocietyUISummary(mockSociety);

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseAscensionPermissionChangeRequestedEvent(false);

            //Validation
            Assert.AreEqual(mockSociety.ID, simulationControl.LastAscensionPermissionRequest.Key, "The last request was made on an incorrect society");
            Assert.AreEqual(false, simulationControl.LastAscensionPermissionRequest.Value, "The last request was made with an incorrect permission");
        }

        [Test]
        public void OnSocietyDisplayRaisesDestructionRequestedevent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var summaryDisplay = BuildMockSocietySummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.SocietySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(42);
            var summaryToPush = new SocietyUISummary(mockSociety);

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(mockSociety.ID, simulationControl.LastSocietyDestructionRequest);
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
            Assert.That(summaryDisplay.isActiveAndEnabled, "The display was not activated");
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
            Assert.IsFalse(summaryDisplay.isActiveAndEnabled, "SummaryDisplay is still active");
        }

        [Test]
        public void OnSelectEventPushedWithHighwayManagerUISummary_HighwayManagerDisplayIsGivenTheSummary_AndActivated() {
            //Setup
            var summaryDisplay = BuildMockManagerDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwayManagerSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new HighwayManagerUISummary();
            summaryToPush.ID = 1;

            //Execution
            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Validation
            Assert.AreEqual(summaryToPush, summaryDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(summaryDisplay.isActiveAndEnabled, "The display was not activated");
        }

        [Test]
        public void OnHighwayManagerDisplayRaisesDestructionRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var summaryDisplay = BuildMockManagerDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwayManagerSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new HighwayManagerUISummary();
            summaryToPush.ID = 1;

            uiControl.PushSelectEvent(summaryToPush, eventData);

            //Execution
            summaryDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(summaryToPush.ID, simulationControl.IDOfRequestedManager, "SimulationControl did not receive the proper ID");
        }

        [Test]
        public void OnHighwayManagerDisplayRaisesDestructionRequestedEvent_HighwayManagerDisplayIsDeactivated() {
            //Setup
            var summaryDisplay = BuildMockManagerDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwayManagerSummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var eventData = new BaseEventData(eventSystem);

            var summaryToPush = new HighwayManagerUISummary();
            summaryToPush.ID = 1;

            uiControl.PushSelectEvent(summaryToPush, eventData);

            summaryDisplay.Activate();

            //Execution
            summaryDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.IsFalse(summaryDisplay.isActiveAndEnabled, "SummaryDisplay is still active");
        }

        [Test]
        public void OnSelectEventPushedIntoUIControl_SummaryDisplayIsActivated_AndGivenTheProperSummary() {
            //Setup
            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            var simulationControl = BuildMockSimulationControl();
            
            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

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
            Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested, "The SimulationControl was passed the incorrect ID");
            Assert.AreEqual(30, simulationControl.LastHighwayPriorityRequested, "The SimulationControl was passed the incorrect Priority");
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
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.SecondEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsTrue(simulationControl.SecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                summaryDisplay.ChangeSecondEndpointPermission(resourceType, false);
                ++totalChangeRequestCount;
                Assert.AreEqual(totalChangeRequestCount, simulationControl.SecondEndpointPermissionRequestsPassed,
                    "The SimulationControl failed to receive change request " + totalChangeRequestCount);
                Assert.AreEqual(summaryWithin.ID, simulationControl.LastHighwayIDRequested,
                    "The SimulationControl was passed the incorrect ID");
                Assert.AreEqual(resourceType, simulationControl.SecondEndpointResourceModified,
                    "The SimulationControl was passed the incorrect ResourceType");
                Assert.IsFalse(simulationControl.SecondEndpointPermissionRequested,
                    "The SimulationControl was passed the incorrect isPermitted");
            }
        }

        [Test]
        public void OnHighwayDisplayRaisesUpkeepRequestedEvent_ProperCommandIsSentToSimulationControl() {
            //Setup
            var summaryDisplay = BuildMockHighwaySummaryDisplay();
            var simulationControl = BuildMockSimulationControl();

            var uiControl = BuildUIControl();
            uiControl.HighwaySummaryDisplay = summaryDisplay;
            uiControl.SimulationControl = simulationControl;

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
            Assert.IsTrue (simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Food],   
                "simulationControl was not given the proper upkeep request for food"  );

            Assert.IsFalse(simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Yellow], 
                "simulationControl was not given the proper upkeep request for yellow");

            Assert.IsTrue (simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.White],  
                "simulationControl was not given the proper upkeep request for white" );

            Assert.IsFalse(simulationControl.UpkeepRequestForResourceByID[summaryToPush.ID][ResourceType.Blue],   
                "simulationControl was not given the proper upkeep request for blue"  );
        }

        [Test]
        public void OnMapNodeBeginDragEventPushed_HighwayGhostIsActivated_Cleared_AndGivenTheDraggedMapNodeAsItsFirstEndpoint() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var mapNodeSummary = new MapNodeUISummary();

            //Execution
            controlToTest.PushBeginDragEvent(mapNodeSummary, eventData);

            //Validation
            Assert.AreEqual(mapNodeSummary, highwayGhost.FirstEndpoint, "highwayGhost.FirstEndpoint has an incorrect value");
            Assert.That(highwayGhost.WasCleared, "highwayGhost was not cleared");
            Assert.That(highwayGhost.WasActivated, "highwayGhost was not activated");
        }

        [Test]
        public void OnMapNodeDragEventPushed_AndDragHasAlreadyStarted_HighwayGhostUpdatedWithProperPointerPosition() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);
            var dragEventData = new PointerEventData(eventSystem);
            dragEventData.position = new Vector2(2f, 2f);

            var mapNodeSummary = new MapNodeUISummary();

            //Execution
            controlToTest.PushBeginDragEvent(mapNodeSummary, beginDragEventData);
            controlToTest.PushDragEvent(mapNodeSummary, dragEventData);

            //Validation
            Assert.That(highwayGhost.WasUpdatedWithEventData, "HighwayGhost was not updated with some PointerEventData");
            Assert.AreEqual(dragEventData, highwayGhost.EventDataUpdatedWith, "HighwayGhost was updated with the wrong PointerEventData");
        }

        [Test]
        public void OnMapNodeEndDragEventPushed_AndDragHasAlreadyStarted_UIControlAttemptsToBuildHighwayWithEndpointsInGhost() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var mockSimulationControl = BuildMockSimulationControl();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;
            controlToTest.SimulationControl = mockSimulationControl;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);
            var endDragEventData = new PointerEventData(eventSystem);

            var firstEndpointSummary = new MapNodeUISummary();
            firstEndpointSummary.ID = 1;
            var secondEndpointSummary = new MapNodeUISummary();
            secondEndpointSummary.ID = 20;

            //Execution
            controlToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            highwayGhost.SecondEndpoint = secondEndpointSummary;

            mockSimulationControl.EnableHighwayBuilding = true;
            controlToTest.PushEndDragEvent(firstEndpointSummary, endDragEventData);

            highwayGhost.Clear();

            controlToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            highwayGhost.SecondEndpoint = secondEndpointSummary;

            mockSimulationControl.EnableHighwayBuilding = false;
            controlToTest.PushEndDragEvent(firstEndpointSummary, endDragEventData);

            //Validation
            Assert.AreEqual(2, mockSimulationControl.CanBuildHighwayBetweenChecksMade, "SimulationControl did not receive two CanBuildHighwayBetween checks");
            Assert.AreEqual(1, mockSimulationControl.HighwaysAttempted, "SimulationControl did not receive exactly one build request");
            Assert.AreEqual(firstEndpointSummary.ID, mockSimulationControl.FirstEndpointsChecked[0], "First check had invalid first endpoint");
            Assert.AreEqual(secondEndpointSummary.ID, mockSimulationControl.SecondEndpointsChecked[0], "First check had invalid second endpoint");

            Assert.AreEqual(firstEndpointSummary.ID, mockSimulationControl.FirstEndpointsChecked[1], "Second check had invalid first endpoint");
            Assert.AreEqual(secondEndpointSummary.ID, mockSimulationControl.SecondEndpointsChecked[1], "Second check had invalid second endpoint");
        }


        [Test]
        public void OnMapNodeEndDragEventPushed_AndDragHasAlreadyStarted_HighwayGhostIsCleared_AndDeactivated() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var mockSimulationControl = BuildMockSimulationControl();

            var endpointSummary = new MapNodeUISummary();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;
            controlToTest.SimulationControl = mockSimulationControl;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            //Execution
            controlToTest.PushBeginDragEvent(endpointSummary, beginDragEventData);

            highwayGhost.WasCleared = false;

            controlToTest.PushEndDragEvent(endpointSummary, beginDragEventData);

            //Validation
            Assert.That(highwayGhost.WasCleared, "HighwayGhost was not cleared");
            Assert.That(highwayGhost.WasDeactivated, "HighwayGhost was not deactivated");
        }

        [Test]
        public void OnMapNodePointerEnterEventPushed_AndDragHasAlreadyStarted_HighwayGhostGivenEnteredBlobSiteAsSecondEndpoint() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var simulationControl = BuildMockSimulationControl();

            var firstEndpointSummary = new MapNodeUISummary();
            var secondEndpointSummary = new MapNodeUISummary();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;
            controlToTest.SimulationControl = simulationControl;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            //Execution
            controlToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            controlToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);

            //Validation
            Assert.AreEqual(secondEndpointSummary, highwayGhost.SecondEndpoint);
        }

        [Test]
        public void OnMapNodePointerEnterEventPushed_AndDragHasAlreadyStarted_GhostIsBuildablePropertySetToTheHighwaysBuildability() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var mockSimulationControl = BuildMockSimulationControl();

            var firstEndpointSummary = new MapNodeUISummary();
            var secondEndpointSummary = new MapNodeUISummary();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;
            controlToTest.SimulationControl = mockSimulationControl;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            mockSimulationControl.EnableHighwayBuilding = true;

            //Execution and Validation
            controlToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);

            controlToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);
            Assert.That(highwayGhost.GhostIsBuildable, "First attempt does not register buildability");

            mockSimulationControl.EnableHighwayBuilding = false;

            controlToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);
            Assert.IsFalse(highwayGhost.GhostIsBuildable, "Second attempt falsely registers buildability");
        }

        [Test]
        public void OnMapNodePointerExistEventPushed_AndDragHasAlreadyStarted_HighwayGhostSecondEndpointIsSetToNull_AndBuildabilitySetToFalse() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var mockSimulationControl = BuildMockSimulationControl();

            var firstEndpointSummary = new MapNodeUISummary();
            var secondEndpointSummary = new MapNodeUISummary();

            var controlToTest = BuildUIControl();
            controlToTest.HighwayGhost = highwayGhost;
            controlToTest.SimulationControl = mockSimulationControl;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            mockSimulationControl.EnableHighwayBuilding = false;

            //Execution
            controlToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            controlToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);
            controlToTest.PushPointerExitEvent(secondEndpointSummary, beginDragEventData);

            //Validation
            Assert.IsNull(highwayGhost.SecondEndpoint, "highwayGhost.SecondEndpoint is not null");
            Assert.IsFalse(highwayGhost.GhostIsBuildable, "highwayGhost falsely represents its endpoints as a buildable highway");
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

        private MockHighwayManagerUISummaryDisplay BuildMockManagerDisplay() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayManagerUISummaryDisplay>();
        }

        private MockHighwaySummaryDisplay BuildMockHighwaySummaryDisplay() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwaySummaryDisplay>();
        }

        private MockHighwayGhost BuildMockHighwayGhost() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayGhost>();
        }

        private MockSocietyUISummaryDisplay BuildMockSocietySummaryDisplay() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSocietyUISummaryDisplay>();
        }

        private MockSociety BuildMockSociety() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSociety>();
        }

        #endregion

        #endregion

    }

}
