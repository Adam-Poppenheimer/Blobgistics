using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scoring {

    public abstract class VictoryManagerBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ScoreToWin { get; set; }
        public abstract bool IsCheckingForVictory { get; set; }

        #endregion

        #region instance methods

        public abstract void TriggerVictory();
        public abstract void TriggerDefeat();

        #endregion

    }

}
