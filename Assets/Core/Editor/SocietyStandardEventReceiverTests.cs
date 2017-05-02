using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Societies;

using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class SocietyStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSocietyDisplayRaisesAscensionPermissionChangeRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var societyDisplay = BuildMockSocietyDisplay();
            var societyControl = BuildMockSocietyControl();

            int idReceivedByControl = -1;
            bool permissionReceivedByControl = true;
            societyControl.OnAscensionPermissionChangeRequested += delegate(int id, bool isPermitted) {
                idReceivedByControl = id;
                permissionReceivedByControl = isPermitted;
            };

            var receiverToTest = BuildSocietyReceiver();
            receiverToTest.SocietySummaryDisplay = societyDisplay;
            receiverToTest.SocietyControl = societyControl;

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(42);
            var currentSummary = new SocietyUISummary(mockSociety);

            societyDisplay.CurrentSummary = currentSummary;

            //Execution
            societyDisplay.RaiseAscensionPermissionChangeRequestedEvent(false);

            //Validation
            Assert.AreEqual(mockSociety.ID, idReceivedByControl, "SocietyControl received an incorrect ID or none at all");
            Assert.AreEqual(false, permissionReceivedByControl, "SocietyControl received an incorrect permission or none at all");
        }

        [Test]
        public void OnSocietyDisplayRaisesDestructionRequestedEvent_RequestIsSentToSimulationControlProperly() {
            //Setup
            var societyDisplay = BuildMockSocietyDisplay();
            var societyControl = BuildMockSocietyControl();

            int idReceivedByControl = -1;
            societyControl.OnSocietyDestructionRequested += delegate(int id) {
                idReceivedByControl = id;
            };

            var receiverToTest = BuildSocietyReceiver();
            receiverToTest.SocietySummaryDisplay = societyDisplay;
            receiverToTest.SocietyControl = societyControl;

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(42);
            var currentSummary = new SocietyUISummary(mockSociety);

            societyDisplay.CurrentSummary = currentSummary;

            //Execution
            societyDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(mockSociety.ID, idReceivedByControl);
        }

        [Test]
        public void OnSelectionEventPushed_DisplayIsGivenTheSource_AndActivated() {
            //Setup
            var societyDisplay = BuildMockSocietyDisplay();
            var societyControl = BuildMockSocietyControl();

            var receiverToTest = BuildSocietyReceiver();
            receiverToTest.SocietySummaryDisplay = societyDisplay;
            receiverToTest.SocietyControl = societyControl;

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(42);
            var currentSummary = new SocietyUISummary(mockSociety);

            societyDisplay.Deactivate();

            //Execution
            receiverToTest.PushSelectEvent(currentSummary, null);

            //Validation
            Assert.AreEqual(currentSummary, societyDisplay.CurrentSummary, "SocietyDisplay was not given the correct summary");
            Assert.That(societyDisplay.isActiveAndEnabled, "SocietyDisplay is not active");
        }

        [Test]
        public void OnDestructionEventPushed_DisplayIsDeactivated() {
            //Setup
            var societyDisplay = BuildMockSocietyDisplay();
            var societyControl = BuildMockSocietyControl();

            var receiverToTest = BuildSocietyReceiver();
            receiverToTest.SocietySummaryDisplay = societyDisplay;
            receiverToTest.SocietyControl = societyControl;

            var mockSociety = BuildMockSociety();
            mockSociety.SetID(42);
            var currentSummary = new SocietyUISummary(mockSociety);

            societyDisplay.CurrentSummary = currentSummary;
            societyDisplay.Activate();

            //Execution
            receiverToTest.PushObjectDestroyedEvent(currentSummary);

            //Validation
            Assert.IsFalse(societyDisplay.isActiveAndEnabled, "SocietyDisplay is still active");
        }

        #endregion

        #region utilities

        private SocietyStandardEventReceiver BuildSocietyReceiver() {
            return (new GameObject()).AddComponent<SocietyStandardEventReceiver>();
        }

        private MockSocietyUISummaryDisplay BuildMockSocietyDisplay() {
            return (new GameObject()).AddComponent<MockSocietyUISummaryDisplay>();
        }

        private MockSocietyControl BuildMockSocietyControl() {
            return (new GameObject()).AddComponent<MockSocietyControl>();
        }

        private MockSociety BuildMockSociety() {
            return (new GameObject()).AddComponent<MockSociety>(); 
        }

        #endregion

        #endregion

    }

}
