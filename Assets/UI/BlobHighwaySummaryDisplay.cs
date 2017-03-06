using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Highways;
using Assets.Core;

namespace Assets.UI {

    public class BlobHighwaySummaryDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private SimulationControl SimulationControl;

        [SerializeField] private InputField PriorityInput;

        private BlobHighwayUISummary CurrentSummary;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            PriorityInput.onEndEdit.AddListener(delegate(string textInInput) {
                int newPriority;
                Int32.TryParse(textInInput, out newPriority);
                if(newPriority != CurrentSummary.Priority) {
                    SimulationControl.SetHighwayPriority(CurrentSummary.ID, newPriority);
                }
            });
        }

        #endregion

        public void UpdateDisplay(BlobHighwayUISummary summary) {
            CurrentSummary = summary;
            PriorityInput.text = summary.Priority.ToString();

            transform.position = summary.Transform.position;
            gameObject.SetActive(true);
        }

        public void ClearDisplay() {
            CurrentSummary = null;
            gameObject.SetActive(false);
        }

        #endregion

    }
}
