using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.HighwayManager;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class HighwayManagerStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedWithHighwayManagerUISummary_HighwayManagerDisplayIsGivenTheSummary_AndActivated() {
            //Setup
            var managerDisplay = BuildMockHighwayManagerDisplay();
            var managerControl = BuildMockManagerControl();

            var receiverToTest = BuildHighwayManagerReceiver();
            receiverToTest.HighwayManagerDisplay = managerDisplay;
            receiverToTest.HighwayManagerControl = managerControl;

            var mockManager = BuildMockHighwayManager();
            mockManager.SetID(42);
            var summaryToPush = new HighwayManagerUISummary(mockManager);

            //Execution
            receiverToTest.PushSelectEvent(summaryToPush, null);

            //Validation
            Assert.AreEqual(summaryToPush, managerDisplay.CurrentSummary, "The wrong summary is in the display");
            Assert.That(managerDisplay.isActiveAndEnabled, "The display was not activated");
        }

        [Test]
        public void OnHighwayManagerDisplayRaisesDestructionRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var managerDisplay = BuildMockHighwayManagerDisplay();
            var managerControl = BuildMockManagerControl();

            int lastIDRequested = -1;
            managerControl.DestroyHighwayManagerOfIDCalled += delegate(int id) {
                lastIDRequested = id;
            };

            var receiverToTest = BuildHighwayManagerReceiver();
            receiverToTest.HighwayManagerDisplay = managerDisplay;
            receiverToTest.HighwayManagerControl = managerControl;

            var mockManager = BuildMockHighwayManager();
            mockManager.SetID(42);
            var summaryToPush = new HighwayManagerUISummary(mockManager);

            managerDisplay.CurrentSummary = summaryToPush;
            managerDisplay.Activate();

            //Execution
            managerDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(summaryToPush.ID, lastIDRequested, "ManagerControl received an incorrect ID or none at all");
        }

        [Test]
        public void OnHighwayManagerDisplayRaisesDestructionRequestedEvent_HighwayManagerDisplayIsDeactivated() {
            //Setup
            var managerDisplay = BuildMockHighwayManagerDisplay();
            var managerControl = BuildMockManagerControl();

            int lastIDRequested = -1;
            managerControl.DestroyHighwayManagerOfIDCalled += delegate(int id) {
                lastIDRequested = id;
            };

            var receiverToTest = BuildHighwayManagerReceiver();
            receiverToTest.HighwayManagerDisplay = managerDisplay;
            receiverToTest.HighwayManagerControl = managerControl;

            var mockManager = BuildMockHighwayManager();
            mockManager.SetID(42);
            var summaryToPush = new HighwayManagerUISummary(mockManager);

            managerDisplay.CurrentSummary = summaryToPush;
            managerDisplay.Activate();

            //Execution
            managerDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.IsFalse(managerDisplay.isActiveAndEnabled, "ManagerDisplay is still active");
        }

        #endregion

        #region utilities

        private MockHighwayManagerSummaryDisplay BuildMockHighwayManagerDisplay() {
            return (new GameObject()).AddComponent<MockHighwayManagerSummaryDisplay>();
        }

        private MockHighwayManagerControl BuildMockManagerControl() {
            return (new GameObject()).AddComponent<MockHighwayManagerControl>();
        }

        private HighwayManagerStandardEventReceiver BuildHighwayManagerReceiver() {
            return (new GameObject()).AddComponent<HighwayManagerStandardEventReceiver>();
        }

        private MockHighwayManager BuildMockHighwayManager() {
            return (new GameObject()).AddComponent<MockHighwayManager>();
        }

        #endregion

        #endregion

    }

}
