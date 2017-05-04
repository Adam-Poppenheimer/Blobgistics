using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.ResourceDepots;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class ResourceDepotStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectionEventPushed_DisplayIsGivenTheSource_AndActivated() {
            //Setup
            var depotDisplay = BuildMockDepotDisplay();
            var depotControl = BuildMockResourceDepotControl();

            var receiverToTest = BuildDepotReceiver();
            receiverToTest.DepotSummaryDisplay = depotDisplay;
            receiverToTest.ResourceDepotControl = depotControl;

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 42;

            //Execution
            receiverToTest.PushSelectEvent(summaryToPush, null);

            //Validation
            Assert.AreEqual(summaryToPush, depotDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(depotDisplay.isActiveAndEnabled, "The display was not activated");
        }

        [Test]
        public void OnDisplayRaisesDestructionRequestedEvent_RequestIsSentToControlProperly() {
            //Setup
            var depotDisplay = BuildMockDepotDisplay();
            var depotControl = BuildMockResourceDepotControl();

            int lastIDRequested = -1;
            depotControl.DestroyResourceDepotOfIDCalled += delegate(int id) {
                lastIDRequested = id;
            };

            var receiverToTest = BuildDepotReceiver();
            receiverToTest.DepotSummaryDisplay = depotDisplay;
            receiverToTest.ResourceDepotControl = depotControl;

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 42;

            depotDisplay.CurrentSummary = summaryToPush;

            //Execution
            depotDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(summaryToPush.ID, lastIDRequested, "ResourceDepotControl received an incorrect ID");
        }

        [Test]
        public void OnDisplayRaisesDestructionRequestedEvent_DisplayIsDeactivated() {
            //Setup
            var depotDisplay = BuildMockDepotDisplay();
            var depotControl = BuildMockResourceDepotControl();

            int lastIDRequested = -1;
            depotControl.DestroyResourceDepotOfIDCalled += delegate(int id) {
                lastIDRequested = id;
            };

            var receiverToTest = BuildDepotReceiver();
            receiverToTest.DepotSummaryDisplay = depotDisplay;
            receiverToTest.ResourceDepotControl = depotControl;

            var summaryToPush = new ResourceDepotUISummary();
            summaryToPush.ID = 42;

            depotDisplay.CurrentSummary = summaryToPush;

            //Execution
            depotDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.IsFalse(depotDisplay.isActiveAndEnabled);
        }

        #endregion

        #region utilities

        private MockResourceDepotSummaryDisplay BuildMockDepotDisplay() {
            return (new GameObject()).AddComponent<MockResourceDepotSummaryDisplay>();
        }

        private MockResourceDepotControl BuildMockResourceDepotControl() {
            return (new GameObject()).AddComponent<MockResourceDepotControl>();
        }

        private ResourceDepotStandardEventReceiver BuildDepotReceiver() {
            return (new GameObject()).AddComponent<ResourceDepotStandardEventReceiver>();
        }

        #endregion

        #endregion

    }

}
