using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Scoring {

    public abstract class VictoryManagerBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int TierOneSocietiesToWin   { get; set; }
        public abstract int TierTwoSocietiesToWin   { get; set; }
        public abstract int TierThreeSocietiesToWin { get; set; }
        public abstract int TierFourSocietiesToWin  { get; set; }

        public abstract int CurrentTierOneSocieties   { get; protected set; }
        public abstract int CurrentTierTwoSocieties   { get; protected set; }
        public abstract int CurrentTierThreeSocieties { get; protected set; }
        public abstract int CurrentTierFourSocieties  { get; protected set; }

        public abstract float SecondsOfStabilityToWin { get; set; }

        public abstract bool IsCheckingForVictory { get; set; }

        public abstract bool VictoryClockIsTicking { get; protected set; }
        public abstract float CurrentVictoryClockValue { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> VictoryProgressRefreshed;

        protected void RaiseVictoryProgressRefreshed() {
            if(VictoryProgressRefreshed != null) {
                VictoryProgressRefreshed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        public abstract void TriggerVictory();
        public abstract void TriggerDefeat();

        public abstract void Pause();
        public abstract void Unpause();

        public abstract bool HasAllRequisiteSocieties();
        public abstract SocietyBase GetMostPressingUnstableSociety(); 

        #endregion

    }

}
