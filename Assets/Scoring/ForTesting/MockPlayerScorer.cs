using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scoring.ForTesting {

    public class MockPlayerScorer : PlayerScorerBase {

        #region instance fields and properties

        #region from PlayerScorerBase

        public override int TotalScore {
            get { return _totalScore; }
        }
        public void SetTotalScore(int value) {
            bool scoreHasChanged = _totalScore != value;
            _totalScore = value;
            if(scoreHasChanged) {
                RaiseScoreChanged(_totalScore);
            }
        }
        private int _totalScore;

        #endregion

        #endregion
        
    }

}
