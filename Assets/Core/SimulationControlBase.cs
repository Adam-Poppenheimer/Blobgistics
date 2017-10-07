using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.Blobs;

namespace Assets.Core {

    /// <summary>
    /// The abstract base class for the main control module of the simulation. This class exists to create a layer-like
    /// boundary between the UI and the simulation code and thus decrease coupling. It also exists to enable pause and
    /// resume functionality from a single location.
    /// </summary>
    public abstract class SimulationControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Ticks the whole simulation.
        /// </summary>
        /// <param name="secondsPassed">The time that has passed since the last tick</param>
        public abstract void TickSimulation(float secondsPassed);

        /// <summary>
        /// Pauses the simulation.
        /// </summary>
        /// <remarks>
        /// This method has possible advantages over the traditional way of pausing the game (by setting Time.timeScale
        /// to zero) in that it allows us to selectively cancel update loops of some aspects of the game and not others.
        /// It's not clear if this is a useful distinction in the current implementation.
        /// </remarks>
        public abstract void Pause();
        
        /// <summary>
        /// Resumes the simulation if paused.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Performs all tasks in the simulation that must occur after victory has been achieved.
        /// </summary>
        public abstract void PerformVictoryTasks();

        /// <summary>
        /// Performs all tasks in the simulation that must occur after the player has been defeated.
        /// </summary>
        /// <remarks>
        /// There are not currently any ways for the player to lose the game, so this method is largely
        /// redundant and may be subject to removal during refactoring.
        /// </remarks>
        public abstract void PerformDefeatTasks();

        #endregion

    }

}
