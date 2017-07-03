using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Scoring.ForTesting;

namespace Assets.Scoring.Editor {

    public class VictoryManagerTests {

        #region instance methods

        #region tests

        [Test]
        public void OnTriggerVictoryCalled_PerformVictoryTasksIsCalledOnSimulationControl_AndUIControl() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();

            bool performVictoryTasksWasCalled = false;
            uiControl.PerformVictoryTasksCalled += delegate(object sender, EventArgs e) {
                performVictoryTasksWasCalled = true;
            };

            var managerToTest = BuildVictoryManager();
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;

            //Execution
            managerToTest.TriggerVictory();

            //Validation
            Assert.IsTrue(performVictoryTasksWasCalled);
        }

        [Test]
        public void OnTriggerDefeatCalled_PerformDefeatTasksIsCalledOnSimulationControl_AndUIControl() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();

            bool performDefeatTasksWasCalled = false;
            uiControl.PerformDefeatTasksCalled += delegate(object sender, EventArgs e) {
                performDefeatTasksWasCalled = true;
            };

            var managerToTest = BuildVictoryManager();
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;


            //Execution
            managerToTest.TriggerDefeat();

            //Validation
            Assert.IsTrue(performDefeatTasksWasCalled);
        }

        [Test]
        public void WhenVictoryIsAchieved_MapPermissionManagers_FlagMapAsHavingBeenWonIsCalled_OnTheNameOfTheCurrentSession() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();

            sessionManager.CurrentSession.Name = "Test Session Name";

            var managerToTest = BuildVictoryManager();
            managerToTest.IsCheckingForVictory = true;
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;

            //Execution
            managerToTest.TriggerVictory();

            //Validation
            Assert.AreEqual(sessionManager.CurrentSession.Name, permissionManager.LastMapFlaggedAsHavingBeenWon,
                "PermissionManager has been given an incorrect map name, or no name at all");
        }

        [Test]
        public void OnSocietyFactorySubscribesNewSociety_ManagerUpdatesAccordingly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnSocietyFactoryUnsubscribesSociety_ManagerUpdatesAccordingly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnSocietyChangesComplexity_ManagerUpdatesAccordingly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnVictoryConditionsMet_AndIsCheckingForVictory_ManagerTriggersVictory() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private VictoryManager BuildVictoryManager() {
            return (new GameObject()).AddComponent<VictoryManager>();
        }

        private MockUIControl BuildMockUIControl() {
            return (new GameObject()).AddComponent<MockUIControl>();
        }

        private MockSimulationControl BuildMockSimulationControl() {
            return (new GameObject()).AddComponent<MockSimulationControl>();
        }

        private MockMapPermissionManager BuildMockMapPermissionManager() {
            return (new GameObject()).AddComponent<MockMapPermissionManager>();
        }

        private MockSessionManager BuildMockSessionManager() {
            return (new GameObject()).AddComponent<MockSessionManager>();
        }

        #endregion

        #endregion

    }

}
