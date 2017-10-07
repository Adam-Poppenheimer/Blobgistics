using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.Scoring;

using Assets.UI.Session;

namespace Assets.UI.TitleScreen {

    /// <summary>
    /// A class that controls the new game display within the title screen.
    /// </summary>
    public class NewGameDisplay : MonoBehaviour {

        #region instance fields and properties

        private SerializableSession SelectedSession;

        [SerializeField] private Text MapNameField;
        [SerializeField] private Text MapDescriptionField;
        [SerializeField] private Text MapTierOneSocietiesToWinField;
        [SerializeField] private Text MapTierTwoSocietiesToWinField;
        [SerializeField] private Text MapTierThreeSocietiesToWinField;
        [SerializeField] private Text MapTierFourSocietiesToWinField;

        [SerializeField] private Text PrerequisiteMapsField;
        [SerializeField] private string PrerequisitesRequiredMessage;

        [SerializeField] private RectTransform MapVictoryConditionsSection;

        [SerializeField] private RectTransform  AvailableMapsSection;
        [SerializeField] private GameObject     MapSummaryPrefab;

        [SerializeField] private Button StartGameButton;
        [SerializeField] private Button BackButton;

        [SerializeField] private SessionManagerBase SessionManager;
        [SerializeField] private FileSystemLiaison FileSystemLiason;
        [SerializeField] private VictoryManagerBase VictoryManager;
        [SerializeField] private MapPermissionManagerBase MapPermissionManager;

        #endregion

        #region events

        /// <summary>
        /// Fires when the player has requested to play the loaded map.
        /// </summary>
        public event EventHandler<EventArgs> MapLoaded;

        /// <summary>
        /// Fires when the display requests its own deactivation.
        /// </summary>
        public event EventHandler<EventArgs> DeactivationRequested;

        /// <summary>
        /// Fires the MapLoaded event.
        /// </summary>
        protected void RaiseMapLoaded() {
            if(MapLoaded != null) {
                MapLoaded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the DeactivationRequested event.
        /// </summary>
        protected void RaiseDeactivationRequested() {
            if(DeactivationRequested != null) { }
            DeactivationRequested(this, EventArgs.Empty);
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            FileSystemLiason.RefreshLoadedMaps();
            foreach(var session in FileSystemLiason.LoadedMaps) {
                EstablishRecordForSession(session);
            }

            SortAndEnableMapSummaries();

            BackButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
            PrerequisiteMapsField.gameObject.SetActive(false);

            SetSelectedSession(null);
        }

        private void OnDisable() {
            SetSelectedSession(null);
        }

        private void OnEnable() {
            SortAndEnableMapSummaries();
        }

        #endregion

        /// <summary>
        /// Loads whatever map is in the current session into the runtime
        /// and requests for the game to begin.
        /// </summary>
        public void StartGameWithSelectedMap() {
            if(SelectedSession != null) {
                SessionManager.CurrentSession = SelectedSession;
                SessionManager.PullRuntimeFromCurrentSession();
                VictoryManager.IsCheckingForVictory = true;
                RaiseMapLoaded();
            }
        }

        private void EstablishRecordForSession(SerializableSession session) {
            var newRecord = Instantiate(MapSummaryPrefab).GetComponent<SessionRecord>();
            newRecord.transform.SetParent(AvailableMapsSection, false);
            newRecord.SessionToRecord = session;
            newRecord.gameObject.SetActive(true);
            var cachedCurrentSession = session;

            var summaryButton = newRecord.GetComponent<Button>();
            summaryButton.onClick.AddListener(delegate() { SetSelectedSession(cachedCurrentSession); });            
        }

        private void SetSelectedSession(SerializableSession newSession) {
            if(newSession != null) {
                SelectedSession = newSession;

                MapNameField.text = SelectedSession.Name;
                MapNameField.gameObject.SetActive(true);

                MapDescriptionField.text = SelectedSession.Description;
                MapDescriptionField.gameObject.SetActive(true);

                MapTierOneSocietiesToWinField.text   = SelectedSession.TierOneSocietiesToWin.ToString();
                MapTierTwoSocietiesToWinField.text   = SelectedSession.TierTwoSocietiesToWin.ToString();
                MapTierThreeSocietiesToWinField.text = SelectedSession.TierThreeSocietiesToWin.ToString();
                MapTierFourSocietiesToWinField.text  = SelectedSession.TierFourSocietiesToWin.ToString();

                MapVictoryConditionsSection.gameObject.SetActive(true);

                if(MapPermissionManager.GetMapIsPermittedToBePlayed(SelectedSession.Name)) {
                    PrerequisiteMapsField.gameObject.SetActive(false);
                    StartGameButton.interactable = true;
                }else {
                    StartGameButton.interactable = false;
                    var prerequisiteString = PrerequisitesRequiredMessage;
                    var mapsLeftToWin = MapPermissionManager.GetMapsLeftToWinRequiredToPlayMap(SelectedSession.Name);
                    for(int i = 0; i < mapsLeftToWin.Count() - 1; ++i) {
                        prerequisiteString += string.Format(" {0},", mapsLeftToWin[i]);
                    }
                    prerequisiteString += string.Format(" {0}", mapsLeftToWin.Last());

                    PrerequisiteMapsField.text = prerequisiteString;
                    PrerequisiteMapsField.gameObject.SetActive(true);
                }
            }else {
                SelectedSession = null;

                MapNameField.text = "";
                MapNameField.gameObject.SetActive(false);

                MapDescriptionField.text = "";
                MapDescriptionField.gameObject.SetActive(false);

                MapTierOneSocietiesToWinField.text   = "";
                MapTierTwoSocietiesToWinField.text   = "";
                MapTierThreeSocietiesToWinField.text = "";
                MapTierFourSocietiesToWinField.text  = "";

                MapVictoryConditionsSection.gameObject.SetActive(false);

                StartGameButton.interactable = false;
            }
        }

        private void SortAndEnableMapSummaries() {
            var sessionSummaries = new List<SessionRecord>(GetComponentsInChildren<SessionRecord>());
            sessionSummaries.Sort((summaryOne, summaryTwo) => SessionComparer(summaryOne.SessionToRecord, summaryTwo.SessionToRecord));

            foreach(var sessionSummary in sessionSummaries) {
                sessionSummary.transform.SetSiblingIndex(sessionSummaries.IndexOf(sessionSummary));
                if(MapPermissionManager.GetMapIsPermittedToBePlayed(sessionSummary.SessionToRecord.Name)) {
                    sessionSummary.MainButton.interactable = true;
                }else {
                    sessionSummary.MainButton.interactable = false;
                }
            }
        }

        /// <summary>
        /// A comparer that can help sort our sessions.
        /// </summary>
        /// <remarks>
        ///  With this method, any session with a
        /// prerequisite session appears after its prerequisite session. Currently, this only
        /// works when the prerequisite relationship is defined explicitly. For example, if A
        /// is a prerequisite for B and B is a prerequisite for C, this comparer isn't guaranteed
        /// to place A, B, and C in the right order. It will only work if A and B are both
        /// declared prerequisites to C. This is something of a problem and should be fixed.
        /// </remarks>
        /// <param name="sessionOne">The first session to compare</param>
        /// <param name="sessionTwo">The second session to compare</param>
        /// <returns>A comparison between the two sessions</returns>
        private int SessionComparer(SerializableSession sessionOne, SerializableSession sessionTwo) {
            bool firstIsPermitted = MapPermissionManager.GetMapIsPermittedToBePlayed(sessionOne.Name);
            bool secondIsPermitted = MapPermissionManager.GetMapIsPermittedToBePlayed(sessionTwo.Name);
            if(firstIsPermitted == secondIsPermitted) {
                if(MapPermissionManager.GetAllMapsRequiredToPlayMap(sessionOne.Name).Contains(sessionTwo.Name)) {
                    return 1;
                }else if(MapPermissionManager.GetAllMapsRequiredToPlayMap(sessionTwo.Name).Contains(sessionOne.Name)) {
                    return -1;
                }else {
                    return 0;
                }
            }else if(firstIsPermitted){
                return -1;
            }else {
                return 1;
            }
        }

        #endregion

    }

}
