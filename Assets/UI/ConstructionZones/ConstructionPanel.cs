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

    public class ConstructionPanel : ConstructionPanelBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private Button ConstructResourceDepotButton;
        [SerializeField] private Text   ResourceDepotCostField;

        [SerializeField] private Button ConstructFarmlandButton;
        [SerializeField] private Text   FarmlandCostField;

        [SerializeField] private Button ConstructVillageButton;
        [SerializeField] private Text   VillageCostField;

        private bool DeactivateOnNextUpdate = false;

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

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
            }else if(LocationToConstruct != null){
                transform.position = Camera.main.WorldToScreenPoint(LocationToConstruct.Transform.position);
            }
        }

        #endregion

        #region Unity EventSystem interfaces

        public void OnSelect(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void OnDeselect(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        #endregion

        #region from ConstructionPanelBase

        public override void Activate() {
            gameObject.SetActive(true);
            if(LocationToConstruct != null) {
                transform.position = Camera.main.WorldToScreenPoint(LocationToConstruct.Transform.position);
            }
            StartCoroutine(ReselectToThis());
        }

        public override void Clear() {
            LocationToConstruct = null;
        }

        public override void Deactivate() {
            Clear();
            gameObject.SetActive(false);
        }

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
        }

        #endregion

        public void DoOnChildSelected(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void DoOnChildDeselected(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        private IEnumerator ReselectToThis() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        #endregion

    }
}
