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

    /// <summary>
    /// The standard implementation for UIControlBase. This class exists to create a layer-like
    /// boundary between the UI and the simulation code and thus decrease coupling.
    /// </summary>
    /// <remarks>
    /// This class makes use of some FSM-like event and control flow programming that should probably be considered an
    /// anti-pattern. Refactors of the UI module might consider using an FSM to switch between title screen, escape 
    /// screen, and the main gameplay stance.
    /// 
    /// It's also worth noting that the various EventReceivers dodge around the type system. There's nothing to
    /// guarantee that a MapNodeEventReceiver can even accept a MapNodeUISummary. This result followed from the difficulty
    /// of generic MonoBehaviours, and may also have been indicative of fundamental structural problems  with this
    /// architecture. More iteration may be needed.
    /// </remarks>
    public class UIControl : UIControlBase {

        #region instance fields and properties

        /// <summary>
        /// A list of all the event receivers that are interested in map node events.
        /// </summary>
        public List<TargetedEventReceiverBase> MapNodeEventReceivers {
            get { return _mapNodeEventReceivers; }
            set { _mapNodeEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _mapNodeEventReceivers;

        /// <summary>
        /// A list of all the event receivers that are interested in highway events.
        /// </summary>
        public List<TargetedEventReceiverBase> HighwayEventReceivers {
            get { return _highwayEventReceivers; }
            set { _highwayEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayEventReceivers;

        /// <summary>
        /// A list of all the event receivers that are interested in society events.
        /// </summary>
        public List<TargetedEventReceiverBase> SocietyEventReceivers {
            get { return _societyEventReceivers; }
            set { _societyEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _societyEventReceivers;

        /// <summary>
        /// A list of all the event receivers that are interested in construction zone events.
        /// </summary>
        public List<TargetedEventReceiverBase> ConstructionZoneEventReceivers {
            get { return _constructionZoneEventReceivers; }
            set { _constructionZoneEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _constructionZoneEventReceivers;

        /// <summary>
        /// A list of all the event receivers that are interested in resource depot events.
        /// </summary>
        public List<TargetedEventReceiverBase> ResourceDepotEventReceivers {
            get { return _resourceDepotEventReceivers; }
            set { _resourceDepotEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _resourceDepotEventReceivers;

        /// <summary>
        /// A list of all the event receivers that are interested in highway manager events.
        /// </summary>
        public List<TargetedEventReceiverBase> HighwayManagerEventReceivers {
            get { return _highwayManagerEventReceivers; }
            set { _highwayManagerEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayManagerEventReceivers;

        [SerializeField] private SimulationControlBase SimulationControl;

        [SerializeField] private TitleScreenUI TitleScreenUI;
        [SerializeField] private EscapeMenuUI EscapeMenuUI;
        [SerializeField] private VictorySplashScreen VictorySplashScreen;

        [SerializeField] private PanningZoomingCameraLogic CameraLogic;
        [SerializeField] private TerrainGridBase TerrainGrid;

        [SerializeField] private List<PanelBase> HUDElements = new List<PanelBase>();

        private Dictionary<Type, List<TargetedEventReceiverBase>> EventReceiversByType =
            new Dictionary<Type, List<TargetedEventReceiverBase>>();

        #endregion

        #region instance methods

        #region Unity message methods

        //It's easier to send events to the right receivers by populating a dictionary of types and then
        //using the type parameter of the generic methods below as a key into that dictionary.
        private void Start() {
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

            SimulationControl.Pause();
            TitleScreenUI.gameObject.SetActive(true);
        }

        private void Update() {
            if(Input.GetButtonDown("Cancel")) {
                if(!TryCloseAllOpenDisplays() && !EscapeMenuUI.gameObject.activeInHierarchy) {
                    SimulationControl.Pause();
                    EscapeMenuUI.gameObject.SetActive(true);
                    CameraLogic.enabled = false;
                }
            }
            CameraLogic.Bounds = TerrainGrid.Bounds;
        }

        #endregion

        #region from UIControlBase

        /// <inheritdoc/>
        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushBeginDragEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDragEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushEndDragEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerClickEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerEnterEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerExitEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushSelectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushSelectEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushUpdateSelectedEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDeselectEvent(source, eventData);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushObjectDestroyedEvent<T>(T source) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushObjectDestroyedEvent(source);
                }
            }
        }

        /// <inheritdoc/>
        public override void PerformVictoryTasks() {
            Debug.Log("Victory!");
            foreach(var hudElement in HUDElements) {
                hudElement.Deactivate();
            }
            VictorySplashScreen.Activate();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Defeat conditions are not currently implemented in or used by the game.
        /// </remarks>
        public override void PerformDefeatTasks() {
            Debug.Log("Defeat!");
        }

        #endregion

        private bool TryCloseAllOpenDisplays() {
            bool anyDisplaysWereClosed = false;

            foreach(var eventReceiverList in EventReceiversByType.Values) {
                foreach(var eventReceiver in eventReceiverList) {
                    anyDisplaysWereClosed |= eventReceiver.TryCloseAllOpenDisplays();
                }
            }

            return anyDisplaysWereClosed;
        }

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
            CameraLogic.enabled = true;
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
            TitleScreenUI.CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        private void VictorySplashScreen_ReturnToTitleScreenRequested(object sender, EventArgs e) {
            VictorySplashScreen.Deactivate();
            TitleScreenUI.Activate();
            TitleScreenUI.CurrentActiveDisplay = TitleScreenActiveDisplayType.Options;
        }

        #endregion

    }

}
