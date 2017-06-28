using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Scoring {

    public class PlayerScorer : PlayerScorerBase {

        #region instance fields and properties

        #region from PlayerScorerBase

        public override int TotalScore {
            get { return totalScore; }
        }
        private int totalScore;

        #endregion

        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set {
                DisconnectSocietyFactoryCallbacks();
                _societyFactory = value;
                ConnectSocietyFactoryCallbacks();
                RefreshTotalScore();
            }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            DisconnectSocietyFactoryCallbacks();
            ConnectSocietyFactoryCallbacks();
            RefreshTotalScore();
        }

        #endregion

        private void DisconnectSocietyFactoryCallbacks() {
            if(SocietyFactory != null) {
                SocietyFactory.SocietySubscribed   -= SocietyFactory_SocietySubscribed;
                SocietyFactory.SocietyUnsubscribed -= SocietyFactory_SocietyUnsubscribed;
                
                foreach(var society in SocietyFactory.Societies) {
                    society.CurrentComplexityChanged -= Society_CurrentComplexityChanged;
                }
            }
        }

        private void ConnectSocietyFactoryCallbacks() {
            if(SocietyFactory != null) {
                SocietyFactory.SocietySubscribed   += SocietyFactory_SocietySubscribed;
                SocietyFactory.SocietyUnsubscribed += SocietyFactory_SocietyUnsubscribed;

                foreach(var society in SocietyFactory.Societies) {
                    society.CurrentComplexityChanged += Society_CurrentComplexityChanged;
                }
            }
        }

        private void RefreshTotalScore() {
            totalScore = 0;

            if(SocietyFactory == null) {
                return;
            }

            foreach(var society in SocietyFactory.Societies) {
                totalScore += society.CurrentComplexity.Score;
            }

            RaiseScoreChanged(totalScore);
        }

        private void SocietyFactory_SocietyUnsubscribed(object sender, SocietyEventArgs e) {
            RefreshTotalScore();
        }

        private void SocietyFactory_SocietySubscribed(object sender, SocietyEventArgs e) {
            RefreshTotalScore();
        }

        private void Society_CurrentComplexityChanged(object sender, ComplexityDefinitionEventArgs e) {
            RefreshTotalScore();
        }

        #endregion

    }

}
