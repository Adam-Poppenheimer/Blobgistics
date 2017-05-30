using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using Assets.Scoring.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Scoring {

    public class VictoryManager : VictoryManagerBase {

        #region instance fields and properties

        public PlayerScorerBase PlayerScorer {
            get { return _playerScorer; }
            set {
                if(_playerScorer != null) {
                    _playerScorer.ScoreChanged -= PlayerScorer_ScoreChanged;
                }
                _playerScorer = value;
                if(_playerScorer != null) {
                    _playerScorer.ScoreChanged += PlayerScorer_ScoreChanged;
                }
            }
        }
        [SerializeField] private PlayerScorerBase _playerScorer;

        public override int ScoreToWin {
            get { return _scoreToWin; }
        }
        public void SetScoreToWin(int value) {
            _scoreToWin = value;
        }
        [SerializeField] private int _scoreToWin;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl= value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public SimulationControlBase SimulationControl {
            get { return _simulationControl; }
            set { _simulationControl= value; }
        }
        [SerializeField] private SimulationControlBase _simulationControl;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            if(PlayerScorer != null) {
                PlayerScorer.ScoreChanged -= PlayerScorer_ScoreChanged;

                PlayerScorer.ScoreChanged += PlayerScorer_ScoreChanged;
            }
        }

        #endregion

        #region from VictoryManagerBase

        public override void TriggerDefeat() {
            SimulationControl.PerformDefeatTasks();
            UIControl.PerformDefeatTasks();
        }

        public override void TriggerVictory() {
            SimulationControl.PerformVictoryTasks();
            UIControl.PerformVictoryTasks();
        }

        #endregion

        private void PlayerScorer_ScoreChanged(object sender, IntEventArgs e) {
            if(e.Value >= ScoreToWin) {
                TriggerVictory();
            }
        }

        #endregion
        
    }

}
