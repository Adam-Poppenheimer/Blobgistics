using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.Blobs;

namespace Assets.Core {

    public abstract class SimulationControlBase : MonoBehaviour {

        #region instance methods

        public abstract void TickSimulation(float secondsPassed);

        public abstract void Pause();
        public abstract void Resume();

        public abstract void PerformVictoryTasks();
        public abstract void PerformDefeatTasks();

        #endregion

    }

}
