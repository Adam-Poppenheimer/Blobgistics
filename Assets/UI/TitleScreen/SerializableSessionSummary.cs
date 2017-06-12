using System;

using UnityEngine;
using UnityEngine.UI;

using Assets.Session;

namespace Assets.UI.TitleScreen {

    public class SerializableSessionSummary : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private Text MapNameField;

        #endregion

        #region instance methods

        public void LoadSession(SerializableSession session) {
            MapNameField.text = session.Name;
        }

        #endregion

    }

}