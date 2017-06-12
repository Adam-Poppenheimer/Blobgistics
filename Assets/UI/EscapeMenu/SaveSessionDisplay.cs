using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;
using Assets.UI.Session;

namespace Assets.UI.EscapeMenu {

    public class SaveSessionDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private SessionRecord SessionRecordPrefab;
        [SerializeField] private RectTransform LocationToPlaceRecords;
        [SerializeField] private InputField FilenameInputField;

        [SerializeField] private Button SaveSessionButton;
        [SerializeField] private Button CancelButton;

        [SerializeField] private FileSystemLiaison FileSystemLiaison;
        [SerializeField] private SessionManager SessionManager;

        private bool PerformSaveOnNextUpdate = false;

        private List<SessionRecord> InstantiatedRecords = new List<SessionRecord>();

        #endregion

        #region events

        public event EventHandler<EventArgs> DeactivationRequested;

        protected void RaiseDeactivationRequested() {
            if(DeactivationRequested != null) {
                DeactivationRequested(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            SaveSessionButton.onClick.AddListener(delegate() { PerformSaveOnNextUpdate = true; });
            CancelButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
        }

        private void OnEnable() {
            RefreshSessionList();
        }

        private void Update() {
            if(PerformSaveOnNextUpdate) {
                PerformSave();
                PerformSaveOnNextUpdate = false;
            }
        }

        #endregion

        private void RefreshSessionList() {
            FileSystemLiaison.RefreshLoadedSavedGames();
            for(int i = InstantiatedRecords.Count; i < FileSystemLiaison.LoadedSavedGames.Count; ++i) {
                var newRecord = Instantiate(SessionRecordPrefab.gameObject).GetComponent<SessionRecord>();
                newRecord.transform.SetParent(LocationToPlaceRecords);
                newRecord.MainButton.onClick.AddListener(delegate() {
                    FilenameInputField.text = newRecord.SessionToRecord.Name;
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

        private void PerformSave() {
            var sessionToSave = SessionManager.PullSessionFromRuntime(FilenameInputField.text,
                SessionManager.CurrentSession.Description, SessionManager.CurrentSession.ScoreToWin);
            FileSystemLiaison.WriteSavedGameToFile(sessionToSave);
            RefreshSessionList();
        }

        #endregion

    }

}
