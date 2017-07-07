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

        public override int TierFourSocietiesToWin {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override int TierOneSocietiesToWin {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override int TierThreeSocietiesToWin {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override int TierTwoSocietiesToWin {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override int CurrentTierFourSocieties {
            get {
                throw new NotImplementedException();
            }

            protected set {
                throw new NotImplementedException();
            }
        }

        public override int CurrentTierOneSocieties {
            get {
                throw new NotImplementedException();
            }

            protected set {
                throw new NotImplementedException();
            }
        }

        public override int CurrentTierThreeSocieties {
            get {
                throw new NotImplementedException();
            }

            protected set {
                throw new NotImplementedException();
            }
        }

        public override int CurrentTierTwoSocieties {
            get {
                throw new NotImplementedException();
            }

            protected set {
                throw new NotImplementedException();
            }
        }

        public override float SecondsOfStabilityToWin {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override bool VictoryClockIsTicking {
            get {
                throw new NotImplementedException();
            }

            protected set {
                throw new NotImplementedException();
            }
        }

        public override float CurrentVictoryClockValue {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

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

        public override void Pause() {
            throw new NotImplementedException();
        }

        public override void Unpause() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
