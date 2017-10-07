using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;

namespace Assets.UI.Session {

    /// <summary>
    /// A class that displays serializable sessions, either maps or saved games.
    /// </summary>
    public class SessionRecord : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The session to display.
        /// </summary>
        public SerializableSession SessionToRecord {
            get { return _sessionToRecord; }
            set {
                _sessionToRecord = value;
                Refresh();
            }
        }
        private SerializableSession _sessionToRecord;

        /// <summary>
        /// The primary button used to select the record.
        /// </summary>
        public Button MainButton {
            get { return _mainButton; }
        }
        [SerializeField] private Button _mainButton;

        [SerializeField] private Text NameField;

        [SerializeField] private Image MainImage;
        [SerializeField] private Color HighlightedColor;
        [SerializeField] private Color UnhighlightedColor;

        #endregion

        #region instance methods

        private void Refresh() {
            if(SessionToRecord != null) {
                NameField.text = SessionToRecord.Name;
            }else {
                NameField.text = "--";
            }
            MainImage.color = UnhighlightedColor;
        }

        /// <summary>
        /// Highlights the record, indicating that it's been selected.
        /// </summary>
        public void Highlight() {
            MainImage.color = HighlightedColor;
        }

        /// <summary>
        /// Unhighlights the record, indicating that it's not being selected.
        /// </summary>
        public void Unhighlight() {
            MainImage.color = UnhighlightedColor;
        }

        #endregion

    }

}
