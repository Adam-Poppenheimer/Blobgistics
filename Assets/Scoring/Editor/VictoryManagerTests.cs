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
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();
            var societyFactory    = BuildMockSocietyFactory();

            sessionManager.CurrentSession.Name = "Test Session Name";

            var tierOneComplexity = BuildMockComplexity();
            var tierTwoComplexity = BuildMockComplexity();
            var tierThreeComplexity = BuildMockComplexity();

            var activeLadder = BuildMockComplexityLadder();
            activeLadder.tierOneComplexities.Add  (tierOneComplexity);
            activeLadder.tierTwoComplexities.Add  (tierTwoComplexity);
            activeLadder.tierThreeComplexities.Add(tierThreeComplexity);

            var managerToTest = BuildVictoryManager();
            managerToTest.IsCheckingForVictory = true;
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;
            managerToTest.SocietyFactory       = societyFactory;
            managerToTest.ActiveLadder         = activeLadder;

            var societyOne = BuildMonoBehaviour<MockSociety>();
            societyOne.currentComplexity = tierOneComplexity;
            societyOne.activeComplexityLadder = activeLadder;

            var societyTwo = BuildMonoBehaviour<MockSociety>();
            societyTwo.currentComplexity = tierTwoComplexity;
            societyTwo.activeComplexityLadder = activeLadder;

            var societyThree = BuildMonoBehaviour<MockSociety>();
            societyThree.currentComplexity = tierThreeComplexity;
            societyThree.activeComplexityLadder = activeLadder;

            //Execution
            societyFactory.SubscribeSociety(societyOne);
            societyFactory.SubscribeSociety(societyTwo);
            societyFactory.SubscribeSociety(societyThree);

            //Validation
            Assert.AreEqual(1, managerToTest.CurrentTierOneSocieties,   "Incorrect number of Tier One Societies"  );
            Assert.AreEqual(1, managerToTest.CurrentTierTwoSocieties,   "Incorrect number of Tier Two Societies"  );
            Assert.AreEqual(1, managerToTest.CurrentTierThreeSocieties, "Incorrect number of Tier Three Societies");
            Assert.AreEqual(0, managerToTest.CurrentTierFourSocieties,  "Incorrect number of Tier Four Societies" );
        }

        [Test]
        public void OnSocietyFactoryUnsubscribesSociety_ManagerUpdatesAccordingly() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();
            var societyFactory    = BuildMockSocietyFactory();

            sessionManager.CurrentSession.Name = "Test Session Name";

            var tierOneComplexity = BuildMockComplexity();
            var tierTwoComplexity = BuildMockComplexity();
            var tierThreeComplexity = BuildMockComplexity();

            var activeLadder = BuildMockComplexityLadder();
            activeLadder.tierOneComplexities.Add  (tierOneComplexity);
            activeLadder.tierTwoComplexities.Add  (tierTwoComplexity);
            activeLadder.tierThreeComplexities.Add(tierThreeComplexity);

            var managerToTest = BuildVictoryManager();
            managerToTest.IsCheckingForVictory = true;
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;
            managerToTest.SocietyFactory       = societyFactory;
            managerToTest.ActiveLadder         = activeLadder;

            var societyOne = BuildMonoBehaviour<MockSociety>();
            societyOne.currentComplexity = tierOneComplexity;
            societyOne.activeComplexityLadder = activeLadder;

            var societyTwo = BuildMonoBehaviour<MockSociety>();
            societyTwo.currentComplexity = tierTwoComplexity;
            societyTwo.activeComplexityLadder = activeLadder;

            var societyThree = BuildMonoBehaviour<MockSociety>();
            societyThree.currentComplexity = tierThreeComplexity;
            societyThree.activeComplexityLadder = activeLadder;

            //Execution
            societyFactory.SubscribeSociety(societyOne);
            societyFactory.SubscribeSociety(societyTwo);
            societyFactory.SubscribeSociety(societyThree);

            societyFactory.UnsubscribeSociety(societyOne);
            societyFactory.UnsubscribeSociety(societyTwo);

            //Validation
            Assert.AreEqual(0, managerToTest.CurrentTierOneSocieties,   "Incorrect number of Tier One Societies"  );
            Assert.AreEqual(0, managerToTest.CurrentTierTwoSocieties,   "Incorrect number of Tier Two Societies"  );
            Assert.AreEqual(1, managerToTest.CurrentTierThreeSocieties, "Incorrect number of Tier Three Societies");
            Assert.AreEqual(0, managerToTest.CurrentTierFourSocieties,  "Incorrect number of Tier Four Societies" );
        }

        [Test]
        public void OnSocietyChangesComplexity_ManagerUpdatesAccordingly() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();
            var societyFactory    = BuildMockSocietyFactory();

            sessionManager.CurrentSession.Name = "Test Session Name";

            var tierOneComplexity = BuildMockComplexity();
            var tierTwoComplexity = BuildMockComplexity();
            var tierThreeComplexity = BuildMockComplexity();

            var activeLadder = BuildMockComplexityLadder();
            activeLadder.tierOneComplexities.Add  (tierOneComplexity);
            activeLadder.tierTwoComplexities.Add  (tierTwoComplexity);
            activeLadder.tierThreeComplexities.Add(tierThreeComplexity);

            var managerToTest = BuildVictoryManager();
            managerToTest.IsCheckingForVictory = true;
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;
            managerToTest.SocietyFactory       = societyFactory;
            managerToTest.ActiveLadder         = activeLadder;

            var societyOne = BuildMonoBehaviour<MockSociety>();
            societyOne.currentComplexity = tierOneComplexity;
            societyOne.activeComplexityLadder = activeLadder;

            societyFactory.SubscribeSociety(societyOne);

            //Execution
            societyOne.SetCurrentComplexity(tierTwoComplexity);

            //Validation
            Assert.AreEqual(0, managerToTest.CurrentTierOneSocieties,   "Incorrect number of Tier One Societies"  );
            Assert.AreEqual(1, managerToTest.CurrentTierTwoSocieties,   "Incorrect number of Tier Two Societies"  );
            Assert.AreEqual(0, managerToTest.CurrentTierThreeSocieties, "Incorrect number of Tier Three Societies");
            Assert.AreEqual(0, managerToTest.CurrentTierFourSocieties,  "Incorrect number of Tier Four Societies" );
        }

        [Test]
        public void OnVictoryConditionsMet_AndIsCheckingForVictory_ManagerTriggersVictoryActionsOnUIAndSimulationControl() {
            //Setup
            var uiControl         = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();
            var sessionManager    = BuildMockSessionManager();
            var permissionManager = BuildMockMapPermissionManager();
            var societyFactory    = BuildMockSocietyFactory();

            sessionManager.CurrentSession.Name = "Test Session Name";

            var tierOneComplexity = BuildMockComplexity();
            var tierTwoComplexity = BuildMockComplexity();
            var tierThreeComplexity = BuildMockComplexity();

            var activeLadder = BuildMockComplexityLadder();
            activeLadder.tierOneComplexities.Add  (tierOneComplexity);
            activeLadder.tierTwoComplexities.Add  (tierTwoComplexity);
            activeLadder.tierThreeComplexities.Add(tierThreeComplexity);

            var managerToTest = BuildVictoryManager();
            managerToTest.IsCheckingForVictory = true;
            managerToTest.UIControl            = uiControl;
            managerToTest.SimulationControl    = simulationControl;
            managerToTest.SessionManager       = sessionManager;
            managerToTest.MapPermissionManager = permissionManager;
            managerToTest.SocietyFactory       = societyFactory;
            managerToTest.ActiveLadder         = activeLadder;

            var societyOne = BuildMonoBehaviour<MockSociety>();
            societyOne.currentComplexity = tierOneComplexity;
            societyOne.activeComplexityLadder = activeLadder;
            societyOne.needsAreSatisfied = true;

            bool uiControlVictoryTasksCalled = false;
            uiControl.PerformVictoryTasksCalled += delegate(object sender, EventArgs e) { uiControlVictoryTasksCalled = true; };

            bool simulationControlVictoryTasksCalled = false;
            simulationControl.PerformVictoryTasksCalled += delegate(object sender, EventArgs e) { simulationControlVictoryTasksCalled = true; };

            managerToTest.IsCheckingForVictory = true;
            managerToTest.TierOneSocietiesToWin = 1;
            managerToTest.TierTwoSocietiesToWin = 0;
            managerToTest.TierThreeSocietiesToWin = 0;
            managerToTest.TierFourSocietiesToWin = 0;
            managerToTest.SecondsOfStabilityToWin = 1f;

            //Execution
            societyFactory.SubscribeSociety(societyOne);

            managerToTest.Tick(2f);

            //Validation
            Assert.That(uiControlVictoryTasksCalled, "UIControl's victory tasks were not called");
            Assert.That(simulationControlVictoryTasksCalled, "SimulationControl's victory tasks were not called");
        }

        #endregion

        #region utilities

        private T BuildMonoBehaviour<T>() where T : MonoBehaviour {
            return (new GameObject()).AddComponent<T>();
        }

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

        private MockSocietyFactory BuildMockSocietyFactory() {
            return (new GameObject()).AddComponent<MockSocietyFactory>();
        }

        private MockComplexityDefinition BuildMockComplexity() {
            return (new GameObject()).AddComponent<MockComplexityDefinition>();
        }

        private MockComplexityLadder BuildMockComplexityLadder() {
            return (new GameObject()).AddComponent<MockComplexityLadder>();
        }

        #endregion

        #endregion

    }

}
