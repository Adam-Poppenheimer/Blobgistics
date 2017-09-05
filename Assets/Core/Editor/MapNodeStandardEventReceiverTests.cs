using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Map;
using Assets.ConstructionZones;
using Assets.Highways;
using Assets.Core.ForTesting;

using Assets.UI.Blobs;

namespace Assets.Core.Editor {

    public class MapNodeStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnBeginDragEventPushed_HighwayGhostIsActivated_Cleared_AndGivenTheDraggedMapNodeAsItsFirstEndpoint() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var eventSystem = BuildEventSystem();
            var eventData = new PointerEventData(eventSystem);

            var mapNodeSummary = new MapNodeUISummary();

            //Execution
            receiverToTest.PushBeginDragEvent(mapNodeSummary, eventData);

            //Validation
            Assert.AreEqual(mapNodeSummary, highwayGhost.FirstEndpoint, "highwayGhost.FirstEndpoint has an incorrect value");
            Assert.That(highwayGhost.WasCleared, "highwayGhost was not cleared");
            Assert.That(highwayGhost.WasActivated, "highwayGhost was not activated");
        }

        [Test]
        public void OnDragEventPushed_AndDragHasAlreadyStarted_HighwayGhostUpdatedWithProperPointerPosition() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);
            var dragEventData = new PointerEventData(eventSystem);
            dragEventData.position = new Vector2(2f, 2f);

            var mapNodeSummary = new MapNodeUISummary();

            //Execution
            receiverToTest.PushBeginDragEvent(mapNodeSummary, beginDragEventData);
            receiverToTest.PushDragEvent(mapNodeSummary, dragEventData);

            //Validation
            Assert.That(highwayGhost.WasUpdatedWithEventData, "HighwayGhost was not updated with some PointerEventData");
            Assert.AreEqual(dragEventData, highwayGhost.EventDataUpdatedWith, "HighwayGhost was updated with the wrong PointerEventData");
        }

        [Test]
        public void OnEndDragEventPushed_AndDragHasAlreadyStarted_UIControlAttemptsToBuildHighwayWithEndpointsInGhost() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            int lastCheckedPermissionNode1ID = -1;
            int lastCheckedPermissionNode2ID = -1;
            highwayControl.CanConnectNodesWithHighwayCalled += delegate(int node1ID, int node2ID) {
                lastCheckedPermissionNode1ID = node1ID;
                lastCheckedPermissionNode2ID = node2ID;
            };

            int lastAttemptMadeNode1ID = -1;
            int lastAttemptMadeNode2ID = -1;
            highwayControl.ConnectNodesWithHighwayCalled += delegate(int node1ID, int node2ID) {
                lastAttemptMadeNode1ID = node1ID;
                lastAttemptMadeNode2ID = node2ID;
            };

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);
            var endDragEventData = new PointerEventData(eventSystem);

            var firstEndpointSummary = new MapNodeUISummary();
            firstEndpointSummary.ID = 1;
            var secondEndpointSummary = new MapNodeUISummary();
            secondEndpointSummary.ID = 20;

            //Execution and validation
            receiverToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            highwayGhost.SecondEndpoint = secondEndpointSummary;

            highwayControl.HighwayBuildingIsPermitted = true;
            receiverToTest.PushEndDragEvent(firstEndpointSummary, endDragEventData);

            Assert.AreEqual(firstEndpointSummary.ID, lastCheckedPermissionNode1ID,
                "HighwayControl.CanConnectNodesWithHighway was called with an incorrect node1ID or not called at all");
            Assert.AreEqual(secondEndpointSummary.ID, lastCheckedPermissionNode2ID,
                "HighwayControl.CanConnectNodesWithHighway was called with an incorrect node2ID or not called at all");

            Assert.AreEqual(firstEndpointSummary.ID, lastAttemptMadeNode1ID,
                "HighwayControl.ConnectNodesWithHighway was called with an incorrect node1ID or not called at all");
            Assert.AreEqual(secondEndpointSummary.ID, lastAttemptMadeNode2ID,
                "HighwayControl.ConnectNodesWithHighway was called with an incorrect node2ID or not called at all");

            lastCheckedPermissionNode1ID = -1;
            lastCheckedPermissionNode2ID = -1;

            lastAttemptMadeNode1ID = -1;
            lastAttemptMadeNode2ID = -1;

            highwayGhost.Clear();

            receiverToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            highwayGhost.SecondEndpoint = secondEndpointSummary;

            highwayControl.HighwayBuildingIsPermitted = false;
            receiverToTest.PushEndDragEvent(firstEndpointSummary, endDragEventData);

            Assert.AreEqual(firstEndpointSummary.ID, lastCheckedPermissionNode1ID,
                "HighwayControl.CanConnectNodesWithHighway was called with an incorrect node1ID or not called at all");
            Assert.AreEqual(secondEndpointSummary.ID, lastCheckedPermissionNode2ID,
                "HighwayControl.CanConnectNodesWithHighway was called with an incorrect node2ID or not called at all");

            Assert.IsFalse(
                firstEndpointSummary.ID == lastAttemptMadeNode1ID || secondEndpointSummary.ID == lastAttemptMadeNode2ID,
                "HighwayControl.ConnectNodesWithHighway was called even when no permission was given"
            );
        }

        [Test]
        public void OnEndDragEventPushed_AndDragHasAlreadyStarted_HighwayGhostIsCleared_AndDeactivated() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var endpointSummary = new MapNodeUISummary();

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            //Execution
            receiverToTest.PushBeginDragEvent(endpointSummary, beginDragEventData);

            highwayGhost.WasCleared = false;

            receiverToTest.PushEndDragEvent(endpointSummary, beginDragEventData);

            //Validation
            Assert.That(highwayGhost.WasCleared, "HighwayGhost was not cleared");
            Assert.That(highwayGhost.WasDeactivated, "HighwayGhost was not deactivated");
        }

        [Test]
        public void OnPointerEnterEventPushed_AndDragHasAlreadyStarted_HighwayGhostGivenEnteredBlobSiteAsSecondEndpoint() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var firstEndpointSummary = new MapNodeUISummary();
            var secondEndpointSummary = new MapNodeUISummary();

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            //Execution
            receiverToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            receiverToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);

            //Validation
            Assert.AreEqual(secondEndpointSummary, highwayGhost.SecondEndpoint);
        }

        [Test]
        public void OnPointerExitEventPushed_AndDragHasAlreadyStarted_HighwayGhostSecondEndpointIsSetToNull_AndBuildabilitySetToFalse() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var firstEndpointSummary = new MapNodeUISummary();
            var secondEndpointSummary = new MapNodeUISummary();

            var eventSystem = BuildEventSystem();
            var beginDragEventData = new PointerEventData(eventSystem);

            //Execution
            receiverToTest.PushBeginDragEvent(firstEndpointSummary, beginDragEventData);
            receiverToTest.PushPointerEnterEvent(secondEndpointSummary, beginDragEventData);
            receiverToTest.PushPointerExitEvent(secondEndpointSummary, beginDragEventData);

            //Validation
            Assert.IsNull(highwayGhost.SecondEndpoint, "highwayGhost.SecondEndpoint is not null");
            Assert.IsFalse(highwayGhost.GhostIsBuildable, "highwayGhost falsely represents its endpoints as a buildable highway");
        }

        [Test]
        public void OnSelectionEventPushed_ConstructionPanelIsGivenTheCorrectLocation_AndActivated() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var locationToConstruct = new MapNodeUISummary();

            //Execution
            receiverToTest.PushSelectEvent(locationToConstruct, null);

            //Validation
            Assert.That(constructionPanel.isActiveAndEnabled, "ConstructionPanel was not activated");
            Assert.AreEqual(locationToConstruct, constructionPanel.LocationToConstruct, "ConstructionPanel has the wrong LocationToConstruct");
        }

        [Test]
        public void OnSelectionEventPushed_ConstructionPanelBuildingPermissionsAreSetProperly() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            constructionZoneControl.PermittedProjects = new List<ConstructionProjectUISummary>() { 
                new ConstructionProjectUISummary("Resource Depot", new ResourceDisplayInfo(
                    IntPerResourceDictionary.BuildSummary(new GameObject(), new Dictionary<ResourceType, int>()))),
                new ConstructionProjectUISummary("Village", new ResourceDisplayInfo(
                    IntPerResourceDictionary.BuildSummary(new GameObject(), new Dictionary<ResourceType, int>()))),
                new ConstructionProjectUISummary("Farmland", new ResourceDisplayInfo(
                    IntPerResourceDictionary.BuildSummary(new GameObject(), new Dictionary<ResourceType, int>())))
            };

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var locationToConstruct = new MapNodeUISummary();

            //Execution
            receiverToTest.PushSelectEvent(locationToConstruct, null);

            //Validation
            Assert.NotNull(constructionPanel.LastPermissionsSet, "ConstructionPanel received no permissions");

            var hasResourceDepot = constructionPanel.LastPermissionsSet.Where(
                summary => summary.Name.Equals("Resource Depot", StringComparison.InvariantCultureIgnoreCase)
            ).Count() == 1;

            var hasVillage = constructionPanel.LastPermissionsSet.Where(
                summary => summary.Name.Equals("Village", StringComparison.InvariantCultureIgnoreCase)
            ).Count() == 1;

            var hasFarmland = constructionPanel.LastPermissionsSet.Where(
                summary => summary.Name.Equals("Farmland", StringComparison.InvariantCultureIgnoreCase)
            ).Count() == 1;

            Assert.That(hasResourceDepot, "ConstructionPanel was not given permissions for Resource Depot");
            Assert.That(hasVillage, "ConstructionPanel was not given permissions for Farmland");
            Assert.That(hasFarmland, "ConstructionPanel was not given permissions for Village");
        }

        [Test]
        public void ConstructionPanelRaisesConstructionRequest_ProperRequestIsForwardedToConstructionZoneControl_AndConstructionPanelIsDeactivated() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            string lastProjectNameRequested = "";
            constructionZoneControl.CreateConstructionZoneOnNodeCalled += delegate(int id, string projectName) {
                lastProjectNameRequested = projectName;
            };

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            var locationToConstruct = new MapNodeUISummary();
            constructionPanel.LocationToConstruct = locationToConstruct;

            //Execution and Validation
            constructionPanel.RaiseDeactivationRequestedEvent("Resource Depot");
            Assert.AreEqual("Resource Depot", lastProjectNameRequested, "Resource Depot construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.isActiveAndEnabled, "ConstructionPanel was not deactivated after the Resource Depot request was sent");

            constructionPanel.Activate();
            constructionPanel.RaiseDeactivationRequestedEvent("Farmland");
            Assert.AreEqual("Farmland", lastProjectNameRequested, "Farmland construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.isActiveAndEnabled, "ConstructionPanel was not deactivated after the Farmland request was sent");

            constructionPanel.Activate();
            constructionPanel.RaiseDeactivationRequestedEvent("Village");
            Assert.AreEqual("Village", lastProjectNameRequested, "Village construction request did not forward to SimulationControl");
            Assert.IsFalse(constructionPanel.isActiveAndEnabled, "ConstructionPanel was not deactivated after the Village request was sent");
        }

        [Test]
        public void ConstructionPanelRaisesCloseRequestedEvent_ConstructionPanelIsDeactivated() {
            //Setup
            var highwayGhost = BuildMockHighwayGhost();
            var highwayControl = BuildMockHighwayControl();
            var constructionZoneControl = BuildMockConstructionZoneControl();
            var constructionPanel = BuildMockConstructionPanel();

            var receiverToTest = BuildMapNodeReceiver();
            receiverToTest.HighwayGhost = highwayGhost;
            receiverToTest.HighwayControl = highwayControl;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;
            receiverToTest.ConstructionPanel = constructionPanel;

            //Execution
            constructionPanel.Activate();
            constructionPanel.RaiseCloseRequestedEvent();

            //Validation
            Assert.IsFalse(constructionPanel.isActiveAndEnabled, "ConstructionPanel is still activated");
        }

        #endregion

        #region utilities

        private EventSystem BuildEventSystem() {
            return (new GameObject()).AddComponent<EventSystem>();
        }

        private MockHighwayGhost BuildMockHighwayGhost() {
            return (new GameObject()).AddComponent<MockHighwayGhost>();
        }

        private MapNodeStandardEventReceiver BuildMapNodeReceiver() {
            return (new GameObject()).AddComponent<MapNodeStandardEventReceiver>();
        }

        private MockHighwayControl BuildMockHighwayControl() {
            return (new GameObject()).AddComponent<MockHighwayControl>();
        }

        private MockConstructionZoneControl BuildMockConstructionZoneControl() {
            return (new GameObject()).AddComponent<MockConstructionZoneControl>();
        }

        private MockConstructionPanel BuildMockConstructionPanel() {
            return (new GameObject()).AddComponent<MockConstructionPanel>();
        }

        #endregion

        #endregion

    }

}
