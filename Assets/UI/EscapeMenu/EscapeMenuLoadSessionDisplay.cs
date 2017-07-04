using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.UI.Session;

namespace Assets.UI.EscapeMenu {

    public class EscapeMenuLoadSessionDisplay : PanelBase {

        #region instance fields and properties

        [SerializeField] private SessionRecord SessionRecordPrefab;
        [SerializeField] private RectTransform LocationToPlaceRecords;
        [SerializeField] private Text SelectedSessionNameField;

        [SerializeField] private Button LoadSessionButton;
        [SerializeField] private Button CancelButton;

        [SerializeField] private FileSystemLiaison FileSystemLiaison;
        [SerializeField] private SessionManagerBase SessionManager;

        private SerializableSession SelectedSession {
            get { return _selectedSession; }
            set {
                _selectedSession = value;
                LoadSessionButton.interactable = _selectedSession != null;
            }
        }
        private SerializableSession _selectedSession;

        private bool PerformLoadOnNextUpdate = false;

        private List<SessionRecord> InstantiatedRecords = new List<SessionRecord>();

        #endregion

        #region events

        public event EventHandler<EventArgs> MapLoaded;

        protected void RaiseMapLoaded() {
            if(MapLoaded != null) {
                MapLoaded(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            LoadSessionButton.onClick.AddListener(delegate() { PerformLoadOnNextUpdate = true; });
            CancelButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
        }

        private void Update() {
            if(PerformLoadOnNextUpdate) {
                PerformLoad();
                PerformLoadOnNextUpdate = false;
            }
        }

        #endregion

        #region from PanelBase

        protected override void DoOnActivate() {
            SelectedSession = null;
            RefreshSessionList();
        }

        #endregion

        private void RefreshSessionList() {
            FileSystemLiaison.RefreshLoadedSavedGames();
            for(int i = InstantiatedRecords.Count; i < FileSystemLiaison.LoadedSavedGames.Count; ++i) {
                var newRecord = Instantiate(SessionRecordPrefab.gameObject).GetComponent<SessionRecord>();
                newRecord.transform.SetParent(LocationToPlaceRecords, false);
                newRecord.MainButton.onClick.AddListener(delegate() {
                    SelectedSession = newRecord.SessionToRecord;
                    SelectedSessionNameField.text = newRecord.SessionToRecord.Name;
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
            SessionManager.CurrentSession = SelectedSession;
            SessionManager.PullRuntimeFromCurrentSession();
            RaiseMapLoaded();
        }

        #endregion

    }

}
