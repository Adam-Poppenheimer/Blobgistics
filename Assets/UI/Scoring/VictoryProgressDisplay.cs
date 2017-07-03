using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scoring;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Scoring {

    public class VictoryProgressDisplay : PanelBase {

        #region instance fields and properties

        [SerializeField] private VictoryManagerBase VictoryManager;

        [SerializeField] private Text CurrentTierOneSocietiesField;
        [SerializeField] private Text TierOneSocietiesToWinField;

        [SerializeField] private Text CurrentTierTwoSocietiesField;
        [SerializeField] private Text TierTwoSocietiesToWinField;

        [SerializeField] private Text CurrentTierThreeSocietiesField;
        [SerializeField] private Text TierThreeSocietiesToWinField;

        [SerializeField] private Text CurrentTierFourSocietiesField;
        [SerializeField] private Text TierFourSocietiesToWinField;

        [SerializeField] private RectTransform CountdownToVictorySection;
        [SerializeField] private Text TimeLeftUntilVictoryField;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Update() {
            if(VictoryManager.VictoryClockIsTicking) {
                CountdownToVictorySection.gameObject.SetActive(true);
                var timeLeftUntilVictory = VictoryManager.SecondsOfStabilityToWin - VictoryManager.CurrentVictoryClockValue;
                TimeLeftUntilVictoryField.text = timeLeftUntilVictory.ToString("#.0");
            }else {
                CountdownToVictorySection.gameObject.SetActive(false);
            }
        }

        private void OnDestroy() {
            VictoryManager.VictoryProgressRefreshed -= VictoryManager_VictoryProgressRefreshed;
        }

        #endregion

        #region from PanelBase

        protected override void DoOnActivate() {
            VictoryManager.VictoryProgressRefreshed += VictoryManager_VictoryProgressRefreshed;
        }

        protected override void DoOnDeactivate() {
            VictoryManager.VictoryProgressRefreshed -= VictoryManager_VictoryProgressRefreshed;
        }

        public override void UpdateDisplay() {
            if(VictoryManager.TierOneSocietiesToWin > 0) {
                CurrentTierOneSocietiesField.gameObject.SetActive(true);
                TierOneSocietiesToWinField.gameObject.SetActive(true);

                CurrentTierOneSocietiesField.text = VictoryManager.CurrentTierOneSocieties.ToString();
                TierOneSocietiesToWinField.text   = VictoryManager.TierOneSocietiesToWin.ToString();
            }else {
                CurrentTierOneSocietiesField.gameObject.SetActive(false);
                TierOneSocietiesToWinField.gameObject.SetActive(false);
            }

            if(VictoryManager.TierTwoSocietiesToWin > 0) {
                CurrentTierTwoSocietiesField.gameObject.SetActive(true);
                TierTwoSocietiesToWinField.gameObject.SetActive(true);

                CurrentTierTwoSocietiesField.text = VictoryManager.CurrentTierTwoSocieties.ToString();
                TierTwoSocietiesToWinField.text   = VictoryManager.TierTwoSocietiesToWin.ToString();
            }else {
                CurrentTierTwoSocietiesField.gameObject.SetActive(false);
                TierTwoSocietiesToWinField.gameObject.SetActive(false);
            }

            if(VictoryManager.TierThreeSocietiesToWin > 0) {
                CurrentTierThreeSocietiesField.gameObject.SetActive(true);
                TierThreeSocietiesToWinField.gameObject.SetActive(true);

                CurrentTierThreeSocietiesField.text = VictoryManager.CurrentTierThreeSocieties.ToString();
                TierThreeSocietiesToWinField.text   = VictoryManager.TierThreeSocietiesToWin.ToString();
            }else {
                CurrentTierThreeSocietiesField.gameObject.SetActive(false);
                TierThreeSocietiesToWinField.gameObject.SetActive(false);
            }

            if(VictoryManager.TierFourSocietiesToWin > 0) {
                CurrentTierFourSocietiesField.gameObject.SetActive(true);
                TierFourSocietiesToWinField.gameObject.SetActive(true);

                CurrentTierFourSocietiesField.text = VictoryManager.CurrentTierFourSocieties.ToString();
                TierFourSocietiesToWinField.text   = VictoryManager.TierFourSocietiesToWin.ToString();
            }else {
                CurrentTierFourSocietiesField.gameObject.SetActive(false);
                TierFourSocietiesToWinField.gameObject.SetActive(false);
            }
        }

        #endregion

        private void VictoryManager_VictoryProgressRefreshed(object sender, EventArgs e) {
            UpdateDisplay();
        }

        #endregion

    }

}
