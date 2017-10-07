using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.ConstructionZones;
using Assets.UI.Blobs;

namespace Assets.UI.ConstructionZones {

    /// <summary>
    /// The standard implementation of ConstructionZoneSummaryDisplayBase, which provides information and
    /// commands for a particular construction zone.
    /// </summary>
    public class ConstructionZoneSummaryDisplay : ConstructionZoneSummaryDisplayBase {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        /// <inheritdoc/>
        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Text ProjectNameField;
        [SerializeField] private ResourceDisplayBase CostField;
        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(DestroyButton != null) {
                DestroyButton.onClick.AddListener(delegate() {
                    RaiseDestructionRequested();
                });
            }
            MovePanelWithCamera = true;
        }

        /// <inheritdoc/>
        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.Transform.position);
            }
        }

        #endregion

        #region from IntelligentPanel

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            if(CurrentSummary != null) {
               DesiredWorldPosition = CurrentSummary.Transform.position;
            }
        }

        /// <inheritdoc/>
        public override void ClearDisplay() {
            CurrentSummary = null;
        }

        /// <inheritdoc/>
        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                ProjectNameField.text = CurrentSummary.Project.Name;
                CostField.PushAndDisplayInfo(CurrentSummary.Project.Cost);
            }
        }

        #endregion

        #endregion

    }

}
