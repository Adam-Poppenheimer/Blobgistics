using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Scoring;

namespace Assets.Session.ForTesting {

    public class MockVictoryManager : VictoryManagerBase {

        #region instance fields and properties

        #region from VictoryManagerBase

        public override bool IsCheckingForVictory {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override int ScoreToWin { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from VictoryManagerBase

        public override void TriggerDefeat() {
            throw new NotImplementedException();
        }

        public override void TriggerVictory() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
