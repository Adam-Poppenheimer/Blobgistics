using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using Assets.Session;
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

        public override int ScoreToWin { get; set; }

        public override bool IsCheckingForVictory { get; set; }

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

        public SessionManagerBase SessionManager {
            get { return _sessionManager; }
            set { _sessionManager= value; }
        }
        [SerializeField] private SessionManagerBase _sessionManager;

        public MapPermissionManagerBase MapPermissionManager {
            get { return _mapPermissionManager; }
            set { _mapPermissionManager= value; }
        }
        [SerializeField] private MapPermissionManagerBase _mapPermissionManager;

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
            IsCheckingForVictory = false;
        }

        public override void TriggerVictory() {
            SimulationControl.PerformVictoryTasks();
            UIControl.PerformVictoryTasks();
            MapPermissionManager.FlagMapAsHavingBeenWon(SessionManager.CurrentSession.Name);
            IsCheckingForVictory = false;
        }

        #endregion

        private void PlayerScorer_ScoreChanged(object sender, IntEventArgs e) {
            if(IsCheckingForVictory && e.Value >= ScoreToWin) {
                TriggerVictory();
            }
        }

        #endregion
        
    }

}
