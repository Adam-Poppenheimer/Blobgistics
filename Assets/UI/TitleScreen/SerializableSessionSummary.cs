using System;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;

namespace Assets.UI.TitleScreen {

    public class SerializableSessionSummary : MonoBehaviour {

        #region instance fields and properties

        public SerializableSession CurrentSession {
            get { return currentSession; }
        }
        [SerializeField] private SerializableSession currentSession;

        public Button SelectionButton {
            get { return _selectionButton; }
        }
        [SerializeField] private Button _selectionButton;

        [SerializeField] private Text MapNameField;

        #endregion

        #region instance methods

        public void LoadSession(SerializableSession session) {
            currentSession = session;
            MapNameField.text = session.Name;
        }

        #endregion

    }

}