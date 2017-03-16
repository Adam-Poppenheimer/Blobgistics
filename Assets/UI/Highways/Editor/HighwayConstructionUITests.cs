using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using NUnit.Framework;

using Assets.Map;
using Assets.Core;
using Assets.UI.Highways.ForTesting;

namespace Assets.UI.Highways.Editor {

    public class HighwayConstructionUITests {

        #region instance methods

        #region tests

        //All pushed events are assumed to be pushed with a MapNodeUISummary

        [Test]
        public void OnBeginDragEventPushedIntoUIControl_HighwayGhostIsActivated_Cleared_AndGivenTheDraggedMapNodeAsItsFirstEndpoint() {
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
        public void OnDragEventPushedIntoUIControl_AndDragHasAlreadyStarted_HighwayGhostUpdatedWithProperPointerPosition() {
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
        public void OnEndDragEventPushedIntoUIControl_AndDragHasAlreadyStarted_UIControlAttemptsToBuildHighwayWithEndpointsInGhost() {
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
            Assert.AreEqual(2, mockSimulationControl.ChecksMade, "SimulationControl did not receive two CanBuildHighwayBetween checks");
            Assert.AreEqual(1, mockSimulationControl.HighwaysAttempted, "SimulationControl did not receive exactly one build request");
            Assert.AreEqual(firstEndpointSummary.ID, mockSimulationControl.FirstEndpointsChecked[0], "First check had invalid first endpoint");
            Assert.AreEqual(secondEndpointSummary.ID, mockSimulationControl.SecondEndpointsChecked[0], "First check had invalid second endpoint");

            Assert.AreEqual(firstEndpointSummary.ID, mockSimulationControl.FirstEndpointsChecked[1], "Second check had invalid first endpoint");
            Assert.AreEqual(secondEndpointSummary.ID, mockSimulationControl.SecondEndpointsChecked[1], "Second check had invalid second endpoint");
        }


        [Test]
        public void OnEndDragEventPushedIntoUIControl_AndDragHasAlreadyStarted_HighwayGhostIsCleared_AndDeactivated() {
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
        public void OnPointerEnterEventPushedIntoUIControl_AndDragHasAlreadyStarted_HighwayGhostGivenEnteredBlobSiteAsSecondEndpoint() {
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
        public void OnPointerEnterEventPushedIntoUIControl_AndDragHasAlreadyStarted_GhostIsBuildablePropertySetToTheHighwaysBuildability() {
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
        public void OnPointerExitEventPushedIntoUIControl_AndDragHasAlreadyStarted_HighwayGhostSecondEndpointIsSetToNull_AndBuildabilitySetToFalse() {
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

        #region utilities

        private MockHighwayGhost BuildMockHighwayGhost() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayGhost>();
        }

        private HighwayConstructionMockSimulationControl BuildMockSimulationControl() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayConstructionMockSimulationControl>();
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
