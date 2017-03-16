using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI.ConstructionZones {

    public class ConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override bool HasPermissionForResourceDepot { get; set; }

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private Button ConstructResourceDepotButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ConstructResourceDepotButton != null) {
                ConstructResourceDepotButton.onClick.AddListener(delegate() {
                    RaiseDepotConstructionRequested();
                });
            }
        }

        #endregion

        #region from ConstructionPanelBase

        public override void Activate() {
            gameObject.SetActive(true);
        }

        public override void Clear() {
            LocationToConstruct = null;
        }

        public override void Deactivate() {
            gameObject.SetActive(false);
        }

        #endregion

        #endregion

    }
}
