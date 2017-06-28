using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;

namespace Assets.UI.Session {

    public class SessionRecord : MonoBehaviour {

        #region instance fields and properties

        public SerializableSession SessionToRecord {
            get { return _sessionToRecord; }
            set {
                _sessionToRecord = value;
                Refresh();
            }
        }
        private SerializableSession _sessionToRecord;

        public Button MainButton {
            get { return _mainButton; }
        }
        [SerializeField] private Button _mainButton;

        [SerializeField] private Text NameField;

        #endregion

        #region instance methods

        private void Refresh() {
            if(SessionToRecord != null) {
                NameField.text = SessionToRecord.Name;
            }else {
                NameField.text = "--";
            }
        }

        public void Highlight() {
            
        }

        public void Unhighlight() {

        }

        #endregion

    }

}
