using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Scoring {

    /// <summary>
    /// The abstract base class for determining victory conditions,
    /// defeat conditions, and triggering victory or defeat.
    /// </summary>
    public abstract class VictoryManagerBase : MonoBehaviour {

        #region instance fields and properties

        ///<summary>
        /// The number of tier 1 societies required to win.
        /// </summary>
        public abstract int TierOneSocietiesToWin   { get; set; }

        ///<summary>
        /// The number of tier 2 societies required to win.
        /// </summary>
        public abstract int TierTwoSocietiesToWin   { get; set; }

        ///<summary>
        /// The number of tier 3 societies required to win.
        /// </summary>
        public abstract int TierThreeSocietiesToWin { get; set; }
        
        ///<summary>
        /// The number of tier 4 societies required to win.
        /// </summary>
        public abstract int TierFourSocietiesToWin  { get; set; }

        ///<summary>
        /// The number of tier 1 societies the victory manager thinks are in the session.
        /// </summary>
        public abstract int CurrentTierOneSocieties   { get; protected set; }
        
        ///<summary>
        /// The number of tier 2 societies the victory manager thinks are in the session.
        /// </summary>
        public abstract int CurrentTierTwoSocieties   { get; protected set; }
        
        ///<summary>
        /// The number of tier 3 societies the victory manager thinks are in the session.
        /// </summary>
        public abstract int CurrentTierThreeSocieties { get; protected set; }
        
        ///<summary>
        /// The number of tier 4 societies the victory manager thinks are in the session.
        /// </summary>
        public abstract int CurrentTierFourSocieties  { get; protected set; }

        /// <summary>
        /// The number of seconds the current configuration needs to have no societies
        /// with unsatisfied needs in order for satisfied victory conditions to lead to
        /// a victory.
        /// </summary>
        public abstract float SecondsOfStabilityToWin { get; set; }

        /// <summary>
        /// Determines whether the VictoryManager is actively checking for victory.
        /// </summary>
        public abstract bool IsCheckingForVictory { get; set; }

        /// <summary>
        /// Whether the victory clock currently ticking or not.
        /// </summary>
        public abstract bool VictoryClockIsTicking { get; protected set; }

        /// <summary>
        /// The current value of the victory clock, which ticks up until it reaches
        /// SecondsOfStabilityToWin.
        /// </summary>
        public abstract float CurrentVictoryClockValue { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever the victory manager has reconsidered whether the player is
        /// winning or has won.
        /// </summary>
        public event EventHandler<EventArgs> VictoryProgressRefreshed;

        /// <summary>
        /// Fires the VictoryProgressRefreshed event.
        /// </summary>
        protected void RaiseVictoryProgressRefreshed() {
            if(VictoryProgressRefreshed != null) {
                VictoryProgressRefreshed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Takes the various actions that must occur when the player has beaten the current map.
        /// </summary>
        public abstract void TriggerVictory();

        /// <summary>
        /// Takes the various actions that must occur when the player has lost the current map.
        /// </summary>
        public abstract void TriggerDefeat();

        /// <summary>
        /// Pauses the victory clock.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Unpauses the victory clock.
        /// </summary>
        public abstract void Unpause();

        /// <summary>
        /// Determines whether the current session has all the societies necessary for victory.
        /// </summary>
        /// <returns>Whether the current session has all the societies necessary for victory</returns>
        public abstract bool HasAllRequisiteSocieties();

        /// <summary>
        /// Returns the unstable society within the current session whose instability is most
        /// important.
        /// </summary>
        /// <returns>The most important unstable society, or null if none exists</returns>
        public abstract SocietyBase GetMostPressingUnstableSociety(); 

        #endregion

    }

}
