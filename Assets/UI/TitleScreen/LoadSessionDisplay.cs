using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.UI.Session;

namespace Assets.UI.TitleScreen {

    /// <summary>
    /// A class that controls the load session panel within the title screen.
    /// </summary>
    public class LoadSessionDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private SessionRecord SessionRecordPrefab;
        [SerializeField] private RectTransform LocationToPlaceRecords;

        [SerializeField] private Button LoadSessionButton;
        [SerializeField] private Button BackButton;

        [SerializeField] private FileSystemLiaison FileSystemLiaison;
        [SerializeField] private SessionManagerBase SessionManager;

        private SessionRecord SelectedRecord {
            get { return _selectedRecord; }
            set {
                if(_selectedRecord != null) {
                    _selectedRecord.Unhighlight();
                }
                _selectedRecord = value;
                if(_selectedRecord != null) {
                    LoadSessionButton.interactable = true;
                    _selectedRecord.Highlight();
                }else {
                    LoadSessionButton.interactable = false;
                }
            }
        }
        private SessionRecord _selectedRecord;

        private bool PerformLoadOnNextUpdate = false;

        private List<SessionRecord> InstantiatedRecords = new List<SessionRecord>();

        #endregion

        #region events

        /// <summary>
        /// Fires when the display requests its own deactivation.
        /// </summary>
        public event EventHandler<EventArgs> DeactivationRequested;

        /// <summary>
        /// Fires when the session has loaded and play should begin.
        /// </summary>
        public event EventHandler<EventArgs> SessionLoaded;

        /// <summary>
        /// Fires the DeactivationRequested event.
        /// </summary>
        protected void RaiseDeactivationRequested() {
            if(DeactivationRequested != null) {
                DeactivationRequested(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the SessionLoaded event.
        /// </summary>
        protected void RaiseSessionLoaded() {
            if(SessionLoaded != null) {
                SessionLoaded(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            LoadSessionButton.onClick.AddListener(delegate() { PerformLoadOnNextUpdate = true; });
            BackButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
        }

        private void OnEnable() {
            RefreshSessionList();
        }

        private void Update() {
            if(PerformLoadOnNextUpdate) {
                PerformLoad();
                PerformLoadOnNextUpdate = false;
            }
        }

        #endregion

        private void RefreshSessionList() {
            FileSystemLiaison.RefreshLoadedSavedGames();
            for(int i = InstantiatedRecords.Count; i < FileSystemLiaison.LoadedSavedGames.Count; ++i) {
                var newRecord = Instantiate(SessionRecordPrefab.gameObject).GetComponent<SessionRecord>();
                newRecord.transform.SetParent(LocationToPlaceRecords, false);
                newRecord.MainButton.onClick.AddListener(delegate() {
                    SelectedRecord = newRecord;
                });

                InstantiatedRecords.Add(newRecord);
            }

            int recordIndex = 0;
            for(; recordIndex < FileSystemLiaison.LoadedSavedGames.Count; ++recordIndex) {
                var currentRecord = InstantiatedRecords[recordIndex];
                currentRecord.SessionToRecord = FileSystemLiaison.LoadedSavedGames[recordIndex];
                currentRecord.gameObject.SetActive(true);
            }
            for(; recordIndex < InstantiatedRecords.Count; ++recordIndex) {
                InstantiatedRecords[recordIndex].gameObject.SetActive(true);
            }
        }

        private void PerformLoad() {
            SessionManager.CurrentSession = SelectedRecord.SessionToRecord;
            SessionManager.PullRuntimeFromCurrentSession();
            RaiseSessionLoaded();
        }

        #endregion

    }

}
