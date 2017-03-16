using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConstructionZones {

    public class ConstructionZoneSummaryDisplay : ConstructionZoneSummaryDisplayBase {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override ConstructionZoneUISummary SummaryToDisplay { get; set; }

        #endregion

        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(DestroyButton != null) {
                DestroyButton.onClick.AddListener(delegate() {
                    RaiseConstructionZoneDestructionRequested();
                });
            }
        }

        #endregion

        #region from ConstructionZoneSummaryDisplayBase

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
