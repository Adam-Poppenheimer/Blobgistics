using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.Scoring;

namespace Assets.UI.TitleScreen {

    public class NewGameDisplay : MonoBehaviour {

        #region instance fields and properties

        private SerializableSession SelectedSession;

        [SerializeField] private Text MapNameField;
        [SerializeField] private Text MapDescriptionField;
        [SerializeField] private Text MapScoreToWinField;

        [SerializeField] private Text PrerequisiteMapsField;
        [SerializeField] private string PrerequisitesRequiredMessage;

        [SerializeField] private RectTransform MapScoreToWinSection;

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

        public event EventHandler<EventArgs> MapLoaded;
        public event EventHandler<EventArgs> DeactivationRequested;

        protected void RaiseMapLoaded() {
            if(MapLoaded != null) {
                MapLoaded(this, EventArgs.Empty);
            }
        }

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
                var newSummary = Instantiate(MapSummaryPrefab).GetComponent<SerializableSessionSummary>();
                newSummary.transform.SetParent(AvailableMapsSection);
                newSummary.LoadSession(session);
                newSummary.gameObject.SetActive(true);
                var cachedCurrentSession = session;
                newSummary.GetComponent<Button>().onClick.AddListener(delegate() { SetSelectedSession(cachedCurrentSession); });
            }

            BackButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
            PrerequisiteMapsField.gameObject.SetActive(false);

            SetSelectedSession(null);
        }

        private void OnDisable() {
            SetSelectedSession(null);
        }

        #endregion

        private void SetSelectedSession(SerializableSession newSession) {
            if(newSession != null) {
                SelectedSession = newSession;

                MapNameField.text = SelectedSession.Name;
                MapNameField.gameObject.SetActive(true);

                MapDescriptionField.text = SelectedSession.Description;
                MapDescriptionField.gameObject.SetActive(true);

                MapScoreToWinField.text = SelectedSession.ScoreToWin.ToString();
                MapScoreToWinSection.gameObject.SetActive(true);

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

                MapScoreToWinField.text = "";
                MapScoreToWinSection.gameObject.SetActive(false);

                StartGameButton.interactable = false;
            }
        }

        public void StartGameWithSelectedMap() {
            if(SelectedSession != null) {
                SessionManager.CurrentSession = SelectedSession;
                SessionManager.PullRuntimeFromCurrentSession();
                VictoryManager.IsCheckingForVictory = true;
                RaiseMapLoaded();
            }
        }

        #endregion

    }

}
