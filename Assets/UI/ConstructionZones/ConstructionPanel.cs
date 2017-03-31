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

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private Button ConstructResourceDepotButton;
        [SerializeField] private Button ConstructFarmlandButton;
        [SerializeField] private Button ConstructVillageButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ConstructResourceDepotButton != null) {
                ConstructResourceDepotButton.onClick.AddListener(delegate() {
                    RaiseConstructionRequested("Resource Depot");
                });
            }
            if(ConstructFarmlandButton != null) {
                ConstructFarmlandButton.onClick.AddListener(delegate() {
                    RaiseConstructionRequested("Farmland");
                });
            }
            if(ConstructVillageButton != null) {
                ConstructVillageButton.onClick.AddListener(delegate() {
                    RaiseConstructionRequested("Village");
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

        public override void SetPermissions(IEnumerable<string> permittedProjects) {
            ConstructResourceDepotButton.interactable = false;
            ConstructFarmlandButton.interactable = false;
            ConstructVillageButton.interactable = false;

            if(permittedProjects.Contains("Resource Depot", StringComparer.InvariantCultureIgnoreCase)) {
                ConstructResourceDepotButton.interactable = true;
            }
            if(permittedProjects.Contains("Farmland", StringComparer.InvariantCultureIgnoreCase)) {
                ConstructFarmlandButton.interactable = true;
            }
            if(permittedProjects.Contains("Village", StringComparer.InvariantCultureIgnoreCase)) {
                ConstructVillageButton.interactable = true;
            }
        }

        #endregion

        #endregion

    }
}
