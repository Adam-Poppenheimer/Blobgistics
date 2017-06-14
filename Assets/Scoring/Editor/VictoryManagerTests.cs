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
            var playerScorer = BuildMockPlayerScorer();
            var uiControl = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();

            bool performVictoryTasksWasCalled = false;
            uiControl.PerformVictoryTasksCalled += delegate(object sender, EventArgs e) {
                performVictoryTasksWasCalled = true;
            };

            var managerToTest = BuildVictoryManager();
            managerToTest.PlayerScorer = playerScorer;
            managerToTest.UIControl = uiControl;
            managerToTest.SimulationControl = simulationControl;

            //Execution
            managerToTest.TriggerVictory();

            //Validation
            Assert.IsTrue(performVictoryTasksWasCalled);
        }

        [Test]
        public void OnTriggerDefeatCalled_PerformDefeatTasksIsCalledOnSimulationControl_AndUIControl() {
            //Setup
            var playerScorer = BuildMockPlayerScorer();
            var uiControl = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();

            bool performDefeatTasksWasCalled = false;
            uiControl.PerformDefeatTasksCalled += delegate(object sender, EventArgs e) {
                performDefeatTasksWasCalled = true;
            };

            var managerToTest = BuildVictoryManager();
            managerToTest.PlayerScorer = playerScorer;
            managerToTest.UIControl = uiControl;
            managerToTest.SimulationControl = simulationControl;


            //Execution
            managerToTest.TriggerDefeat();

            //Validation
            Assert.IsTrue(performDefeatTasksWasCalled);
        }

        [Test]
        public void OnPlayerScorerGeneratesAWinningScore_AndManagerIsCheckingForVictory_TriggerVictoryIsCalled() {
            //Setup
            var playerScorer = BuildMockPlayerScorer();
            var uiControl = BuildMockUIControl();
            var simulationControl = BuildMockSimulationControl();

            bool performVictoryTasksWasCalled = false;
            uiControl.PerformVictoryTasksCalled += delegate(object sender, EventArgs e) {
                performVictoryTasksWasCalled = true;
            };

            var managerToTest = BuildVictoryManager();
            managerToTest.ScoreToWin = 50;
            managerToTest.IsCheckingForVictory = true;
            managerToTest.PlayerScorer = playerScorer;
            managerToTest.UIControl = uiControl;
            managerToTest.SimulationControl = simulationControl;

            //Execution
            playerScorer.SetTotalScore(managerToTest.ScoreToWin);

            //Validation
            Assert.IsTrue(performVictoryTasksWasCalled);
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

        private MockPlayerScorer BuildMockPlayerScorer() {
            return (new GameObject()).AddComponent<MockPlayerScorer>();
        }

        #endregion

        #endregion

    }

}
