using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Map;
using Assets.Highways;
using Assets.Core.ForTesting;

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
