using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using Assets.Session;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Scoring {

    public class VictoryManager : VictoryManagerBase {

        #region instance fields and properties

        #region from VictoryManagerBase

        public override int TierOneSocietiesToWin {
            get { return _tierOneSocietiesToWin; }
            set { _tierOneSocietiesToWin = value; }
        }
        [SerializeField] private int _tierOneSocietiesToWin;

        public override int TierTwoSocietiesToWin {
            get { return _tierTwoSocietiesToWin; }
            set { _tierTwoSocietiesToWin = value; }
        }
        [SerializeField] private int _tierTwoSocietiesToWin;

        public override int TierThreeSocietiesToWin {
            get { return _tierThreeSocietiesToWin; }
            set { _tierThreeSocietiesToWin = value; }
        }
        [SerializeField] private int _tierThreeSocietiesToWin;

        public override int TierFourSocietiesToWin {
            get { return _tierFourSocietiesToWin; }
            set { _tierFourSocietiesToWin = value; }
        }
        [SerializeField] private int _tierFourSocietiesToWin;

        public override int CurrentTierOneSocieties   { get; protected set; }
        public override int CurrentTierTwoSocieties   { get; protected set; }
        public override int CurrentTierThreeSocieties { get; protected set; }
        public override int CurrentTierFourSocieties  { get; protected set; }

        public override float SecondsOfStabilityToWin {
            get { return _secondsOfStabilityToWin; }
            set { _secondsOfStabilityToWin = value; }
        }
        [SerializeField] private float _secondsOfStabilityToWin;

        public override bool IsCheckingForVictory { get; set; }

        public override bool VictoryClockIsTicking { get; protected set; }

        public override float CurrentVictoryClockValue { get; set; }

        private bool IsPaused = false;

        #endregion

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

        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        public ComplexityLadderBase ActiveLadder {
            get { return _activeLadder; }
            set { _activeLadder = value; }
        }
        [SerializeField] private ComplexityLadderBase _activeLadder;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            if(SocietyFactory != null) {
                foreach(var society in SocietyFactory.Societies) {
                    society.NeedsAreSatisfiedChanged -= Society_NeedsAreSatisfiedChanged;
                    society.NeedsAreSatisfiedChanged += Society_NeedsAreSatisfiedChanged;

                    society.CurrentComplexityChanged -= Society_CurrentComplexityChanged;
                    society.CurrentComplexityChanged += Society_CurrentComplexityChanged;
                }
                SocietyFactory.SocietySubscribed += SocietyFactory_SocietySubscribed;
                SocietyFactory.SocietyUnsubscribed += SocietyFactory_SocietyUnsubscribed;
                RefreshVictoryProgress();
            }
            VictoryClockIsTicking = false;
        }

        private void Update() {
            if(IsCheckingForVictory && VictoryClockIsTicking && GetVictoryConditionsAreSatisfied()) {
                if(!IsPaused) {
                    CurrentVictoryClockValue += Time.deltaTime;
                    if(CurrentVictoryClockValue >= SecondsOfStabilityToWin) {
                        TriggerVictory();
                    }
                }
            }else {
                CurrentVictoryClockValue = 0f;
            }
        }

        private void OnDestroy() {
            IsCheckingForVictory = false;
            if(SocietyFactory == null) {
                foreach(var society in SocietyFactory.Societies) {
                    society.NeedsAreSatisfiedChanged -= Society_NeedsAreSatisfiedChanged;
                    society.CurrentComplexityChanged   -= Society_CurrentComplexityChanged;
                }
                SocietyFactory.SocietySubscribed   -= SocietyFactory_SocietySubscribed;
                SocietyFactory.SocietyUnsubscribed -= SocietyFactory_SocietyUnsubscribed;
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
            VictoryClockIsTicking = false;
            CurrentVictoryClockValue = 0f;
            IsCheckingForVictory = false;
        }

        public override void Pause() {
            IsPaused = true;
        }

        public override void Unpause() {
            IsPaused = false;
        }

        public override bool HasAllRequisiteSocieties() {
            return (
                CurrentTierOneSocieties   >= TierOneSocietiesToWin   &&
                CurrentTierTwoSocieties   >= TierTwoSocietiesToWin   &&
                CurrentTierThreeSocieties >= TierThreeSocietiesToWin &&
                CurrentTierFourSocieties  >= TierFourSocietiesToWin
            );
        }

        public override SocietyBase GetMostPressingUnstableSociety() {
            var unstableSocieties = new List<SocietyBase>(SocietyFactory.Societies.Where(society => !society.NeedsAreSatisfied));
            unstableSocieties.Sort(delegate(SocietyBase societyOne, SocietyBase societyTwo) {
                var societyOneTier = societyOne.ActiveComplexityLadder.GetTierOfComplexity(societyOne.CurrentComplexity);
                var societyTwoTier = societyTwo.ActiveComplexityLadder.GetTierOfComplexity(societyTwo.CurrentComplexity);
                return societyTwoTier.CompareTo(societyOneTier);
            });
            return unstableSocieties.FirstOrDefault();
        }

        #endregion

        private void RefreshVictoryProgress() {
            CurrentTierOneSocieties   = 0;
            CurrentTierTwoSocieties   = 0;
            CurrentTierThreeSocieties = 0;
            CurrentTierFourSocieties  = 0;

            var allSocietiesHaveSatisfiedNeeds = true;

            foreach(var society in SocietyFactory.Societies) {
                if(ActiveLadder.TierOneComplexities.Contains(society.CurrentComplexity)) {
                    ++CurrentTierOneSocieties;
                }else if(ActiveLadder.TierTwoComplexities.Contains(society.CurrentComplexity)) {
                    ++CurrentTierTwoSocieties;
                }else if(ActiveLadder.TierThreeComplexities.Contains(society.CurrentComplexity)) {
                    ++CurrentTierThreeSocieties;
                }else if(ActiveLadder.TierFourComplexities.Contains(society.CurrentComplexity)) {
                    ++CurrentTierFourSocieties;
                }
                allSocietiesHaveSatisfiedNeeds &= society.NeedsAreSatisfied;
            }
            if(GetVictoryConditionsAreSatisfied() && allSocietiesHaveSatisfiedNeeds) {
                VictoryClockIsTicking = true;
            }else {
                VictoryClockIsTicking = false;
            }
            RaiseVictoryProgressRefreshed();
        }

        private bool GetVictoryConditionsAreSatisfied() {
            return (
                CurrentTierOneSocieties   >= TierOneSocietiesToWin   &&
                CurrentTierTwoSocieties   >= TierTwoSocietiesToWin   &&
                CurrentTierThreeSocieties >= TierThreeSocietiesToWin &&
                CurrentTierFourSocieties  >= TierFourSocietiesToWin
            );
        }

        private void CheckVictory() {
            if(IsCheckingForVictory) {
                RefreshVictoryProgress();
            }
        }

        private void SocietyFactory_SocietyUnsubscribed(object sender, SocietyEventArgs e) {
            e.Society.CurrentComplexityChanged -= Society_CurrentComplexityChanged;
            e.Society.NeedsAreSatisfiedChanged -= Society_NeedsAreSatisfiedChanged;
            CheckVictory();
        }

        private void SocietyFactory_SocietySubscribed(object sender, SocietyEventArgs e) {
            e.Society.CurrentComplexityChanged += Society_CurrentComplexityChanged;
            e.Society.NeedsAreSatisfiedChanged += Society_NeedsAreSatisfiedChanged;
            CheckVictory();
        }

        private void Society_CurrentComplexityChanged(object sender, ComplexityDefinitionEventArgs e) {
            CheckVictory();
        }

        private void Society_NeedsAreSatisfiedChanged(object sender, BoolEventArgs e) {
            if(!e.Value) {
                VictoryClockIsTicking = false;
            }else {
                RefreshVictoryProgress();
            }
        }

        #endregion

    }

}
