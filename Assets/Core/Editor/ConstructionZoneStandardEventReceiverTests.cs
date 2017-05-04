using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.ConstructionZones;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class ConstructionZoneStandardEventReceiverTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSelectEventPushedIntoUIControl_ConstructionZoneSummaryDisplayIsActivated_AndGivenTheClickedZone() {
            //Setup
            var constructionZoneDisplay = MockConstructionZoneSummaryDisplay();
            var constructionZoneControl = BuildMockConstructionZoneControl();

            var receiverToTest = BuildConstructionZoneReceiver();
            receiverToTest.ConstructionZoneSummaryDisplay = constructionZoneDisplay;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;

            var zoneToSelect = new ConstructionZoneUISummary();
            zoneToSelect.ID = 42;

            //Execution
            receiverToTest.PushSelectEvent(zoneToSelect, null);

            //Validation
            Assert.That(constructionZoneDisplay.isActiveAndEnabled, "ConstructionZoneDisplay was not activated");
            Assert.AreEqual(zoneToSelect, constructionZoneDisplay.CurrentSummary, "ConstructionZoneDisplay has the wrong SummaryToDisplay");
        }

        [Test]
        public void OnDestructionRequestedEventRaised_SimulationControlReceivesRequestToDestroyTheZone() {
            //Setup
            var constructionZoneDisplay = MockConstructionZoneSummaryDisplay();
            var constructionZoneControl = BuildMockConstructionZoneControl();

            int lastIDRequestedForDestruction = -1;
            constructionZoneControl.DestroyConstructionZoneCalled += delegate(int id) {
                lastIDRequestedForDestruction = id;
            };

            var receiverToTest = BuildConstructionZoneReceiver();
            receiverToTest.ConstructionZoneSummaryDisplay = constructionZoneDisplay;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;

            var zoneToSelect = new ConstructionZoneUISummary();
            zoneToSelect.ID = 42;

            constructionZoneDisplay.Activate();
            constructionZoneDisplay.CurrentSummary = zoneToSelect;

            //Execution
            constructionZoneDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.AreEqual(zoneToSelect.ID, lastIDRequestedForDestruction, "ConstructionZoneControl received an incorrect ID to destroy");
        }

        [Test]
        public void OnCloseRequestedEventRaised_ConstructionZoneDisplayIsDeactivated() {
            //Setup
            var constructionZoneDisplay = MockConstructionZoneSummaryDisplay();
            var constructionZoneControl = BuildMockConstructionZoneControl();

            int lastIDRequestedForDestruction = -1;
            constructionZoneControl.DestroyConstructionZoneCalled += delegate(int id) {
                lastIDRequestedForDestruction = id;
            };

            var receiverToTest = BuildConstructionZoneReceiver();
            receiverToTest.ConstructionZoneSummaryDisplay = constructionZoneDisplay;
            receiverToTest.ConstructionZoneControl = constructionZoneControl;

            var zoneToSelect = new ConstructionZoneUISummary();
            zoneToSelect.ID = 42;

            constructionZoneDisplay.Activate();
            constructionZoneDisplay.CurrentSummary = zoneToSelect;

            //Execution
            constructionZoneDisplay.RaiseDestructionRequestedEvent();

            //Validation
            Assert.IsFalse(constructionZoneDisplay.isActiveAndEnabled);
        }

        #endregion

        #region utilities

        private MockConstructionZoneSummaryDisplay MockConstructionZoneSummaryDisplay() {
            return (new GameObject()).AddComponent<MockConstructionZoneSummaryDisplay>();
        }

        private MockConstructionZoneControl BuildMockConstructionZoneControl() {
            return (new GameObject()).AddComponent<MockConstructionZoneControl>();
        }

        private ConstructionZoneStandardEventReceiver BuildConstructionZoneReceiver() {
            return (new GameObject()).AddComponent<ConstructionZoneStandardEventReceiver>();
        }

        #endregion

        #endregion

    }

}
