using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.HighwayUpgraders;

namespace Assets.UI.HighwayUpgraders {

    public class HighwayUpgraderSummaryDisplay : HighwayUpgraderSummaryDisplayBase {

        #region instance fields and properties

        #region from HighwayUpgraderSummaryDisplayBase

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override HighwayUpgraderUISummary SummaryToDisplay { get; set; }

        #endregion

        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(DestroyButton != null) {
                DestroyButton.onClick.AddListener(delegate() {
                    RaiseUpgraderDestructionRequested();
                });
            }
        }

        #endregion

        #region from HighwayUpgraderSummaryDisplayBase

        public override void Activate() {
            gameObject.SetActive(true);
        }

        public override void Clear() {
            SummaryToDisplay = null;
        }

        public override void Deactivate() {
            gameObject.SetActive(false);
        }

        #endregion

        #endregion
        
    }

}
