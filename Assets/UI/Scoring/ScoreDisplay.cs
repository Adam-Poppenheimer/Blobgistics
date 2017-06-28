using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scoring;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Scoring {

    public class ScoreDisplay : PanelBase {

        #region instance fields and properties

        [SerializeField] private PlayerScorerBase Scorer;
        [SerializeField] private VictoryManagerBase VictoryManager;

        [SerializeField] private Text CurrentScoreField;
        [SerializeField] private Text RequiredScoreField;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            Scorer.ScoreChanged += Scorer_ScoreChanged;
        }

        private void OnDestroy() {
            if(Scorer != null) {
                Scorer.ScoreChanged -= Scorer_ScoreChanged;
            }
        }

        #endregion

        protected override void DoOnActivate() {
            CurrentScoreField.text = Scorer.TotalScore.ToString();
            RequiredScoreField.text = VictoryManager.ScoreToWin.ToString();
        }

        private void Scorer_ScoreChanged(object sender, IntEventArgs e) {
            CurrentScoreField.text = e.Value.ToString();
        }

        #endregion

    }

}
