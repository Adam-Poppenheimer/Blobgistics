using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.ResourceDepots;

namespace Assets.UI.ResourceDepots {

    /// <summary>
    /// The standard implementation of ResourceDepotSummaryDisplayBase, which provides information
    /// and commands involving resource depots to the player.
    /// </summary>
    public class ResourceDepotSummaryDisplay : ResourceDepotSummaryDisplayBase {

        #region instance fields and properties

        #region from ResourceDepotSummaryDisplayBase

        /// <inheritdoc/>
        public override ResourceDepotUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            MovePanelWithCamera = true;
        }

        #endregion

        #region from IntelligentPanel

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            DestroyButton.onClick.AddListener(delegate(){
                RaiseDestructionRequested();
            });
            DesiredWorldPosition = CurrentSummary.Transform.position;
        }

        /// <inheritdoc/>
        protected override void DoOnDeactivate() {
            DestroyButton.onClick.RemoveAllListeners();
        }

        #endregion

        #endregion

    }

}
