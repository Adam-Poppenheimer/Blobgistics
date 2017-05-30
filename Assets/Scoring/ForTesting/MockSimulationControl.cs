using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Core;

namespace Assets.Scoring.ForTesting {

    public class MockSimulationControl : SimulationControlBase {

        #region events

        public event EventHandler<EventArgs> PerformVictoryTasksCalled;
        public event EventHandler<EventArgs> PerformDefeatTasksCalled;

        #endregion

        #region instance methods

        #region from SimulationControlBase

        public override void TickSimulation(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void PerformDefeatTasks() {
            if(PerformDefeatTasksCalled != null) {
                PerformDefeatTasksCalled(this, EventArgs.Empty);
            }
        }

        public override void PerformVictoryTasks() {
            if(PerformVictoryTasksCalled != null) {
                PerformVictoryTasksCalled(this, EventArgs.Empty);
            }
        }

        #endregion

        #endregion

    }

}
