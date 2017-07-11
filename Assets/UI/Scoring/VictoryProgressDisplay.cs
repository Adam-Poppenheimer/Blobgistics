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

        #region internal types

        private enum VictoryDisplayPanelType {
            VictoryCountdown,
            RequisitesNotAchieved,
            UnstableSociety
        }

        #endregion

        #region instance fields and properties

        [SerializeField] private VictoryManagerBase VictoryManager;

        [SerializeField] private RectTransform TierOneSection;
        [SerializeField] private Text CurrentTierOneSocietiesField;
        [SerializeField] private Text TierOneSocietiesToWinField;

        [SerializeField] private RectTransform TierTwoSection;
        [SerializeField] private Text CurrentTierTwoSocietiesField;
        [SerializeField] private Text TierTwoSocietiesToWinField;

        [SerializeField] private RectTransform TierThreeSection;
        [SerializeField] private Text CurrentTierThreeSocietiesField;
        [SerializeField] private Text TierThreeSocietiesToWinField;

        [SerializeField] private RectTransform TierFourSection;
        [SerializeField] private Text CurrentTierFourSocietiesField;
        [SerializeField] private Text TierFourSocietiesToWinField;

        [SerializeField] private RectTransform CountdownToVictorySection;
        [SerializeField] private Text TimeLeftUntilVictoryField;

        [SerializeField] private RectTransform RequisiteSocietiesNotAchievedSection;

        [SerializeField] private RectTransform UnstableSocietySection;
        [SerializeField] private Text UnstableSocietyNameField;

        private VictoryDisplayPanelType ActivePanel {
            get { return _activePanel; }
            set {
                CountdownToVictorySection.gameObject.SetActive           (false);
                RequisiteSocietiesNotAchievedSection.gameObject.SetActive(false);
                UnstableSocietySection.gameObject.SetActive              (false);
                _activePanel = value;
                switch(_activePanel) {
                    case VictoryDisplayPanelType.VictoryCountdown:      CountdownToVictorySection.gameObject.SetActive           (true); break;
                    case VictoryDisplayPanelType.RequisitesNotAchieved: RequisiteSocietiesNotAchievedSection.gameObject.SetActive(true); break;
                    case VictoryDisplayPanelType.UnstableSociety:       UnstableSocietySection.gameObject.SetActive              (true); break;
                    default: break;
                }
            }
        }
        private VictoryDisplayPanelType _activePanel;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Update() {
            if(VictoryManager.VictoryClockIsTicking) {
                ActivePanel = VictoryDisplayPanelType.VictoryCountdown;
                var timeLeftUntilVictory = VictoryManager.SecondsOfStabilityToWin - VictoryManager.CurrentVictoryClockValue;
                TimeLeftUntilVictoryField.text = timeLeftUntilVictory.ToString("#.0");

            }else if(VictoryManager.HasAllRequisiteSocieties()){
                ActivePanel = VictoryDisplayPanelType.UnstableSociety;
                var mostPressingUnstableSociety = VictoryManager.GetMostPressingUnstableSociety();
                if(mostPressingUnstableSociety != null) {
                    UnstableSocietyNameField.text = mostPressingUnstableSociety.CurrentComplexity.name;
                }

            }else {
                ActivePanel = VictoryDisplayPanelType.RequisitesNotAchieved;
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
                TierOneSection.gameObject.SetActive(true);

                CurrentTierOneSocietiesField.text = VictoryManager.CurrentTierOneSocieties.ToString();
                TierOneSocietiesToWinField.text   = VictoryManager.TierOneSocietiesToWin.ToString();
            }else {
                TierOneSection.gameObject.SetActive(false);
            }

            if(VictoryManager.TierTwoSocietiesToWin > 0) {
                TierTwoSection.gameObject.SetActive(true);

                CurrentTierTwoSocietiesField.text = VictoryManager.CurrentTierTwoSocieties.ToString();
                TierTwoSocietiesToWinField.text   = VictoryManager.TierTwoSocietiesToWin.ToString();
            }else {
                TierTwoSection.gameObject.SetActive(false);
            }

            if(VictoryManager.TierThreeSocietiesToWin > 0) {
                TierThreeSection.gameObject.SetActive(true);

                CurrentTierThreeSocietiesField.text = VictoryManager.CurrentTierThreeSocieties.ToString();
                TierThreeSocietiesToWinField.text   = VictoryManager.TierThreeSocietiesToWin.ToString();
            }else {
                TierThreeSection.gameObject.SetActive(false);
            }

            if(VictoryManager.TierFourSocietiesToWin > 0) {
                TierFourSection.gameObject.SetActive(true);

                CurrentTierFourSocietiesField.text = VictoryManager.CurrentTierFourSocieties.ToString();
                TierFourSocietiesToWinField.text   = VictoryManager.TierFourSocietiesToWin.ToString();
            }else {
                TierFourSection.gameObject.SetActive(false);
            }
        }

        #endregion

        private void VictoryManager_VictoryProgressRefreshed(object sender, EventArgs e) {
            UpdateDisplay();
        }

        #endregion

    }

}
