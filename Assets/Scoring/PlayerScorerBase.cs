using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Scoring {

    public abstract class PlayerScorerBase : MonoBehaviour {

        #region events

        public event EventHandler<IntEventArgs> ScoreChanged;

        protected void RaiseScoreChanged(int newScore) {
            if(ScoreChanged != null) {
                ScoreChanged(this, new IntEventArgs(newScore));
            }
        }

        #endregion

        #region instance fields and properties

        public abstract int TotalScore { get; }

        #endregion

    }

}
