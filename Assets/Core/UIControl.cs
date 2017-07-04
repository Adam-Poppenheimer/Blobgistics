using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

using Assets.Highways;
using Assets.ConstructionZones;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.HighwayManager;
using Assets.UI;
using Assets.UI.TitleScreen;
using Assets.UI.EscapeMenu;
using Assets.UI.Scoring;

namespace Assets.Core {

    public class UIControl : UIControlBase {

        #region instance fields and properties

        public List<TargetedEventReceiverBase> MapNodeEventReceivers {
            get { return _mapNodeEventReceivers; }
            set { _mapNodeEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _mapNodeEventReceivers;

        public List<TargetedEventReceiverBase> HighwayEventReceivers {
            get { return _highwayEventReceivers; }
            set { _highwayEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayEventReceivers;

        public List<TargetedEventReceiverBase> SocietyEventReceivers {
            get { return _societyEventReceivers; }
            set { _societyEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _societyEventReceivers;

        public List<TargetedEventReceiverBase> ConstructionZoneEventReceivers {
            get { return _constructionZoneEventReceivers; }
            set { _constructionZoneEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _constructionZoneEventReceivers;

        public List<TargetedEventReceiverBase> ResourceDepotEventReceivers {
            get { return _resourceDepotEventReceivers; }
            set { _resourceDepotEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _resourceDepotEventReceivers;

        public List<TargetedEventReceiverBase> HighwayManagerEventReceivers {
            get { return _highwayManagerEventReceivers; }
            set { _highwayManagerEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayManagerEventReceivers;

        [SerializeField] private SimulationControlBase SimulationControl;

        [SerializeField] private TitleScreenUI TitleScreenUI;
        [SerializeField] private EscapeMenuUI EscapeMenuUI;
        [SerializeField] private VictorySplashScreen VictorySplashScreen;

        [SerializeField] private List<PanelBase> HUDElements = new List<PanelBase>();

        private Dictionary<Type, List<TargetedEventReceiverBase>> EventReceiversByType =
            new Dictionary<Type, List<TargetedEventReceiverBase>>();

        #endregion

        #region instance methods

        #region Unity message methods

        private void Awake() {
            EventReceiversByType[typeof(MapNodeUISummary         )] = MapNodeEventReceivers;
            EventReceiversByType[typeof(BlobHighwayUISummary     )] = HighwayEventReceivers;
            EventReceiversByType[typeof(SocietyUISummary         )] = SocietyEventReceivers;
            EventReceiversByType[typeof(ConstructionZoneUISummary)] = ConstructionZoneEventReceivers;
            EventReceiversByType[typeof(ResourceDepotUISummary   )] = ResourceDepotEventReceivers;
            EventReceiversByType[typeof(HighwayManagerUISummary  )] = HighwayManagerEventReceivers;

            TitleScreenUI.GameStartRequested += TitleScreenUI_GameStartRequested;
            TitleScreenUI.GameExitRequested += TitleScreenUI_GameExitRequested;

            EscapeMenuUI.GameResumeRequested += EscapeMenuUI_GameResumeRequested;
            EscapeMenuUI.GameExitRequested += EscapeMenuUI_GameExitRequested;
            EscapeMenuUI.ReturnToMainMenuRequested += EscapeMenuUI_ReturnToMainMenuRequested;

            VictorySplashScreen.ReturnToTitleScreenRequested += VictorySplashScreen_ReturnToTitleScreenRequested;
        }

        private void Start() {
            SimulationControl.Pause();
            TitleScreenUI.gameObject.SetActive(true);
        }

        private void Update() {
            if(Input.GetButtonDown("Cancel")) {
                if(!EscapeMenuUI.gameObject.activeInHierarchy) {
                    SimulationControl.Pause();
                    EscapeMenuUI.gameObject.SetActive(true);
                }
            }
        }

        #endregion

        #region from UIControlBase

        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushBeginDragEvent(source, eventData);
                }
            }
        }

        public override void PushDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDragEvent(source, eventData);
                }
            }
        }

        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushEndDragEvent(source, eventData);
                }
            }
        }

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerClickEvent(source, eventData);
                }
            }
        }

        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerEnterEvent(source, eventData);
                }
            }
        }

        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerExitEvent(source, eventData);
                }
            }
        }

        public override void PushSelectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushSelectEvent(source, eventData);
                }
            }
        }

        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushUpdateSelectedEvent(source, eventData);
                }
            }
        }

        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDeselectEvent(source, eventData);
                }
            }
        }

        public override void PushObjectDestroyedEvent<T>(T source) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushObjectDestroyedEvent(source);
                }
            }
        }

        public override void PerformVictoryTasks() {
            Debug.Log("Victory!");
            foreach(var hudElement in HUDElements) {
                hudElement.Deactivate();
            }
            VictorySplashScreen.Activate();
        }

        public override void PerformDefeatTasks() {
            Debug.Log("Defeat!");
        }

        #endregion

        private void ExitGame() {
            Application.Quit();
        }

        private void TitleScreenUI_GameStartRequested(object sender, EventArgs e) {
            TitleScreenUI.Deactivate();
            foreach(var hudElement in HUDElements) {
                hudElement.Activate();
            }
            SimulationControl.Resume();
        }

        private void TitleScreenUI_GameExitRequested(object sender, EventArgs e) {
            ExitGame();
        }

        private void EscapeMenuUI_GameResumeRequested(object sender, EventArgs e) {
            EscapeMenuUI.gameObject.SetActive(false);
            SimulationControl.Resume();
        }

        private void EscapeMenuUI_GameExitRequested(object sender, EventArgs e) {
            ExitGame();
        }

        private void EscapeMenuUI_ReturnToMainMenuRequested(object sender, EventArgs e) {
            EscapeMenuUI.gameObject.SetActive(false);
            foreach(var hudElement in HUDElements) {
                hudElement.Deactivate();
            }
            SimulationControl.Pause();
            TitleScreenUI.Activate();
            TitleScreenUI.ActivateOptionsDisplay();
        }

        private void VictorySplashScreen_ReturnToTitleScreenRequested(object sender, EventArgs e) {
            VictorySplashScreen.Deactivate();
            TitleScreenUI.Activate();
            TitleScreenUI.ActivateOptionsDisplay();
        }

        #endregion

    }

}
