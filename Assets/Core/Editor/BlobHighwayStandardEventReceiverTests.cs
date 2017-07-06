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

using UnityCustomUtilities.Extensions;

namespace Assets.Core.Editor {

    public class BlobHighwayStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedIntoUIControl_DisplayIsActivated_AndGivenTheProperSummary() {
            //Setup
            var highwayDisplay = BuildMockHighwaySummaryDisplay();
            //var simulationControl = BuildMockSimulationControl();
            
            var receiverToTest = BuildHighwayReceiver();
            receiverToTest.HighwaySummaryDisplay = highwayDisplay;
            receiverToTest.HighwayControl = BuildMockHighwayControl();

            var summaryToPush = new BlobHighwayUISummary();
            summaryToPush.ID = 1;
            summaryToPush.Priority = 15;

            //Execution
            receiverToTest.PushSelectEvent(summaryToPush, null);

            //Validation
            Assert.AreEqual(summaryToPush, highwayDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(highwayDisplay.isActiveAndEnabled, "The display was not activated");
        }

        [Test]
        public void OnSummaryDisplayRaisesFirstEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var highwayDisplay = BuildMockHighwaySummaryDisplay();
            highwayDisplay.CurrentSummary = summaryWithin;

            var highwayControl = BuildMockHighwayControl();

            int lastIDPassed = -1;
            ResourceType lastResourceTypeChanged = ResourceType.HiTechGoods;
            bool lastPermissionGiven = false;
            highwayControl.SetHighwayPullingPermissionOnFirstEndpointForResourceCalled += delegate (int id, ResourceType typeChanged, bool newPermission) {
                lastIDPassed = id;
                lastResourceTypeChanged = typeChanged;
                lastPermissionGiven = newPermission;
            };

            var receiverToTest = BuildHighwayReceiver();
            receiverToTest.HighwaySummaryDisplay = highwayDisplay;
            receiverToTest.HighwayControl = highwayControl;

            //Execution and Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                highwayDisplay.ChangeFirstEndpointPermission(resourceType, true);
                Assert.AreEqual(summaryWithin.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
                Assert.AreEqual(resourceType, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
                Assert.That(lastPermissionGiven, "HighwayControl was passed an incorrect permission");

                highwayDisplay.ChangeFirstEndpointPermission(resourceType, false);
                Assert.AreEqual(summaryWithin.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
                Assert.AreEqual(resourceType, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
                Assert.IsFalse(lastPermissionGiven, "HighwayControl was passed an incorrect permission");
            }
        }

        [Test]
        public void OnSummaryDisplayRaisesSecondEndpointPermissionChangedEvent_ProperPermissionChangeRequestSentToSimulationControl() {
            //Setup
            var summaryWithin = new BlobHighwayUISummary();
            summaryWithin.ID = 14;
            summaryWithin.Priority = 15;

            var highwayDisplay = BuildMockHighwaySummaryDisplay();
            highwayDisplay.CurrentSummary = summaryWithin;

            var highwayControl = BuildMockHighwayControl();

            int lastIDPassed = -1;
            ResourceType lastResourceTypeChanged = ResourceType.HiTechGoods;
            bool lastPermissionGiven = false;
            highwayControl.SetHighwayPullingPermissionOnSecondEndpointForResourceCalled += delegate (int id, ResourceType typeChanged, bool newPermission) {
                lastIDPassed = id;
                lastResourceTypeChanged = typeChanged;
                lastPermissionGiven = newPermission;
            };

            var receiverToTest = BuildHighwayReceiver();
            receiverToTest.HighwaySummaryDisplay = highwayDisplay;
            receiverToTest.HighwayControl = highwayControl;

            //Execution and Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                highwayDisplay.ChangeSecondEndpointPermission(resourceType, true);
                Assert.AreEqual(summaryWithin.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
                Assert.AreEqual(resourceType, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
                Assert.That(lastPermissionGiven, "HighwayControl was passed an incorrect permission");

                highwayDisplay.ChangeSecondEndpointPermission(resourceType, false);
                Assert.AreEqual(summaryWithin.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
                Assert.AreEqual(resourceType, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
                Assert.IsFalse(lastPermissionGiven, "HighwayControl was passed an incorrect permission");
            }
        }

        [Test]
        public void OnHighwayDisplayRaisesUpkeepRequestedEvent_ProperCommandIsSentToSimulationControl() {
            //Setup
            var highwayDisplay = BuildMockHighwaySummaryDisplay();
            var highwayControl = BuildMockHighwayControl();

            int lastIDPassed = -1;
            ResourceType lastResourceTypeChanged = ResourceType.HiTechGoods;
            bool lastRequestMade = false;
            highwayControl.SetHighwayUpkeepRequestCalled += delegate(int id, ResourceType type, bool isRequested) {
                lastIDPassed = id;
                lastResourceTypeChanged = type;
                lastRequestMade = isRequested;
            };

            var receiverToTest = BuildHighwayReceiver();
            receiverToTest.HighwaySummaryDisplay = highwayDisplay;
            receiverToTest.HighwayControl = highwayControl;

            var summaryToPush = new BlobHighwayUISummary();
            summaryToPush.ID = 1;

            highwayDisplay.CurrentSummary = summaryToPush;
            highwayDisplay.Activate();

            //Execution and Validation
            highwayDisplay.RaiseUpkeepRequestedEvent(ResourceType.Food, true);

            Assert.AreEqual(summaryToPush.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
            Assert.AreEqual(ResourceType.Food, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
            Assert.IsTrue(lastRequestMade, "HighwayControl was passed an incorrect isBeingRequested");

            lastIDPassed = -1;
            lastResourceTypeChanged = ResourceType.HiTechGoods;

            highwayDisplay.RaiseUpkeepRequestedEvent(ResourceType.Textiles, false);

            Assert.AreEqual(summaryToPush.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
            Assert.AreEqual(ResourceType.Textiles, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
            Assert.IsFalse(lastRequestMade, "HighwayControl was passed an incorrect isBeingRequested");

            lastIDPassed = -1;
            lastResourceTypeChanged = ResourceType.HiTechGoods;

            highwayDisplay.RaiseUpkeepRequestedEvent(ResourceType.ServiceGoods, true);

            Assert.AreEqual(summaryToPush.ID, lastIDPassed, "HighwayControl was passed an incorrect ID");
            Assert.AreEqual(ResourceType.ServiceGoods, lastResourceTypeChanged, "HighwayControl was passed an incorrect ResourceType");
            Assert.IsTrue(lastRequestMade, "HighwayControl was passed an incorrect isBeingRequested");

            lastIDPassed = -1;
            lastResourceTypeChanged = ResourceType.HiTechGoods;
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

        private MockHighwayControl BuildMockHighwayControl() {
            return (new GameObject()).AddComponent<MockHighwayControl>();
        }

        #endregion

        #endregion

    }

}
