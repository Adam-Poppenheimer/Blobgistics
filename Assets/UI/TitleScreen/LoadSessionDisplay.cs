using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.UI.Session;

namespace Assets.UI.TitleScreen {

    public class LoadSessionDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private SessionRecord SessionRecordPrefab;
        [SerializeField] private RectTransform LocationToPlaceRecords;

        [SerializeField] private Button LoadSessionButton;
        [SerializeField] private Button BackButton;

        [SerializeField] private FileSystemLiaison FileSystemLiaison;
        [SerializeField] private SessionManager SessionManager;

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

        public event EventHandler<EventArgs> DeactivationRequested;
        public event EventHandler<EventArgs> SessionLoaded;

        protected void RaiseDeactivationRequested() {
            if(DeactivationRequested != null) {
                DeactivationRequested(this, EventArgs.Empty);
            }
        }

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
            FileSystemLiaison.RefreshLoadedSessions();
            for(int i = InstantiatedRecords.Count; i < FileSystemLiaison.LoadedSessions.Count; ++i) {
                var newRecord = Instantiate(SessionRecordPrefab.gameObject).GetComponent<SessionRecord>();
                newRecord.transform.SetParent(LocationToPlaceRecords);
                newRecord.MainButton.onClick.AddListener(delegate() {
                    SelectedRecord = newRecord;
                });

                InstantiatedRecords.Add(newRecord);
            }

            int recordIndex = 0;
            for(; recordIndex < FileSystemLiaison.LoadedSessions.Count; ++recordIndex) {
                var currentRecord = InstantiatedRecords[recordIndex];
                currentRecord.SessionToRecord = FileSystemLiaison.LoadedSessions[recordIndex];
                currentRecord.gameObject.SetActive(true);
            }
            for(; recordIndex < InstantiatedRecords.Count; ++recordIndex) {
                InstantiatedRecords[recordIndex].gameObject.SetActive(true);
            }
        }

        private void PerformLoad() {
            var sessionToLoad = SelectedRecord.SessionToRecord;
            SessionManager.PushSessionIntoRuntime(sessionToLoad);
            RaiseSessionLoaded();
        }

        #endregion

    }

}
