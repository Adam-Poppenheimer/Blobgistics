using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones {

    public class ConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private Button ConstructResourceDepotButton;
        [SerializeField] private Text   ResourceDepotCostField;

        [SerializeField] private Button ConstructFarmlandButton;
        [SerializeField] private Text   FarmlandCostField;

        [SerializeField] private Button ConstructVillageButton;
        [SerializeField] private Text   VillageCostField;

        [SerializeField] private Button ConstructHighwayManagerButton;
        [SerializeField] private Text   HighwayManagerCostField;

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
            if(ConstructHighwayManagerButton != null) {
                ConstructHighwayManagerButton.onClick.AddListener(delegate() {
                    RaiseConstructionRequested("Highway Manager");
                });
            }
        }

        protected override void DoOnUpdate() {
            if(LocationToConstruct != null){
                transform.position = Camera.main.WorldToScreenPoint(LocationToConstruct.Transform.position);
            }
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            if(LocationToConstruct != null) {
                transform.position = Camera.main.WorldToScreenPoint(LocationToConstruct.Transform.position);
            }
        }

        public override void ClearDisplay() {
            LocationToConstruct = null;
        }

        #endregion

        #region from ConstructionPanelBase

        public override void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects) {
            ConstructResourceDepotButton.interactable = false;
            ConstructFarmlandButton.interactable = false;
            ConstructVillageButton.interactable = false;

            var resourceDepotProject = permittedProjects.Where(
                project => project.Name.Equals("Resource Depot", StringComparison.InvariantCultureIgnoreCase)
            ).FirstOrDefault();

            if(resourceDepotProject != null) {
                ConstructResourceDepotButton.interactable = true;
                ResourceDepotCostField.text = resourceDepotProject.CostSummaryString;
            }
            
            var villageProject = permittedProjects.Where(
                project => project.Name.Equals("Village", StringComparison.InvariantCultureIgnoreCase)
            ).FirstOrDefault();

            if(villageProject != null) {
                ConstructVillageButton.interactable = true;
                VillageCostField.text = villageProject.CostSummaryString;
            }

            var farmlandProject = permittedProjects.Where(
                project => project.Name.Equals("Farmland", StringComparison.InvariantCultureIgnoreCase)
            ).FirstOrDefault();

            if(farmlandProject != null) {
                ConstructFarmlandButton.interactable = true;
                FarmlandCostField.text = farmlandProject.CostSummaryString;
            }

            var highwayManagerProject = permittedProjects.Where(
                project => project.Name.Equals("Highway Manager", StringComparison.InvariantCultureIgnoreCase)
            ).FirstOrDefault();

            if(highwayManagerProject != null) {
                ConstructHighwayManagerButton.interactable = true;
                HighwayManagerCostField.text = highwayManagerProject.CostSummaryString;
            }
        }

        #endregion

        #endregion

    }
}
