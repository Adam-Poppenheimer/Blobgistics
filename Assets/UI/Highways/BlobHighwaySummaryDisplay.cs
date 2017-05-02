using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Highways;
using Assets.Core;

using UnityCustomUtilities.UI;

namespace Assets.UI.Highways {

    public class BlobHighwaySummaryDisplay : BlobHighwaySummaryDisplayBase {

        #region instance fields and properties

        #region from BlobHighwaySummaryDisplayBase

        public override BlobHighwayUISummary CurrentSummary {
            get { return _currentSummary; }
            set { _currentSummary = value; }
        }
        private BlobHighwayUISummary _currentSummary;

        #endregion

        [SerializeField] private InputField PriorityInput;
        [SerializeField] private Text EfficiencyField;

        [SerializeField] private RectTransform CommonActionsPane;
        [SerializeField] private RectTransform FirstEndpointPane;
        [SerializeField] private RectTransform SecondEndpointPane;

        [SerializeField] private Toggle FirstEndpointFoodPermissionToggle;
        [SerializeField] private Toggle FirstEndpointYellowPermissionToggle;
        [SerializeField] private Toggle FirstEndpointWhitePermissionToggle;
        [SerializeField] private Toggle FirstEndpointBluePermissionToggle;
        
        [SerializeField] private Toggle SecondEndpointFoodPermissionToggle;
        [SerializeField] private Toggle SecondEndpointYellowPermissionToggle;
        [SerializeField] private Toggle SecondEndpointWhitePermissionToggle;
        [SerializeField] private Toggle SecondEndpointBluePermissionToggle;

        [SerializeField] private Toggle IsRequestingFoodToggle;
        [SerializeField] private Toggle IsRequestingYellowToggle;
        [SerializeField] private Toggle IsRequestingWhiteToggle;
        [SerializeField] private Toggle IsRequestingBlueToggle;

        #endregion

        #region instance methods

        #region Unity event methods

        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                FirstEndpointPane.transform.position  = Camera.main.WorldToScreenPoint(CurrentSummary.FirstEndpoint);
                SecondEndpointPane.transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.SecondEndpoint);
            }
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            AlignPermissionPanes();
            
            PriorityInput.onEndEdit.AddListener(delegate(string value) {
                int newPriority;
                if(Int32.TryParse(value, out newPriority)) {
                    RaisePriorityChanged(newPriority);
                }else {
                    PriorityInput.text = CurrentSummary.ToString();
                }
            });

            FirstEndpointFoodPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Food, newPermission);
            });
            FirstEndpointYellowPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Yellow, newPermission);
            });
            FirstEndpointWhitePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.White, newPermission);
            });
            FirstEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Blue, newPermission);
            });

            SecondEndpointFoodPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Food, newPermission);
            });
            SecondEndpointYellowPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Yellow, newPermission);
            });
            SecondEndpointWhitePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.White, newPermission);
            });
            SecondEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Blue, newPermission);
            });

            IsRequestingFoodToggle.onValueChanged.AddListener(delegate(bool isRequesting){
                RaiseResourceRequestedForUpkeep(ResourceType.Food, isRequesting);
            });

            IsRequestingYellowToggle.onValueChanged.AddListener(delegate(bool isRequesting){
                RaiseResourceRequestedForUpkeep(ResourceType.Yellow, isRequesting);
            });

            IsRequestingWhiteToggle.onValueChanged.AddListener(delegate(bool isRequesting){
                RaiseResourceRequestedForUpkeep(ResourceType.White, isRequesting);
            });

            IsRequestingBlueToggle.onValueChanged.AddListener(delegate(bool isRequesting){
                RaiseResourceRequestedForUpkeep(ResourceType.Blue, isRequesting);
            });
        }

        protected override void DoOnDeactivate() {
            gameObject.SetActive(false);

            PriorityInput.onEndEdit.RemoveAllListeners();

            FirstEndpointFoodPermissionToggle.onValueChanged.RemoveAllListeners();
            FirstEndpointYellowPermissionToggle.onValueChanged.RemoveAllListeners();
            FirstEndpointWhitePermissionToggle.onValueChanged.RemoveAllListeners();
            FirstEndpointBluePermissionToggle.onValueChanged.RemoveAllListeners();

            SecondEndpointFoodPermissionToggle.onValueChanged.RemoveAllListeners();
            SecondEndpointYellowPermissionToggle.onValueChanged.RemoveAllListeners();
            SecondEndpointWhitePermissionToggle.onValueChanged.RemoveAllListeners();
            SecondEndpointBluePermissionToggle.onValueChanged.RemoveAllListeners();

            IsRequestingFoodToggle.onValueChanged.RemoveAllListeners();
            IsRequestingYellowToggle.onValueChanged.RemoveAllListeners();
            IsRequestingWhiteToggle.onValueChanged.RemoveAllListeners();
            IsRequestingBlueToggle.onValueChanged.RemoveAllListeners();
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                PriorityInput.text = CurrentSummary.Priority.ToString();
                EfficiencyField.text = CurrentSummary.Efficiency.ToString();

                FirstEndpointFoodPermissionToggle.isOn   = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Food];
                FirstEndpointYellowPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Yellow];
                FirstEndpointWhitePermissionToggle.isOn  = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.White];
                FirstEndpointBluePermissionToggle.isOn   = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Blue];

                SecondEndpointFoodPermissionToggle.isOn   = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Food];
                SecondEndpointYellowPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Yellow];
                SecondEndpointWhitePermissionToggle.isOn  = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.White];
                SecondEndpointBluePermissionToggle.isOn   = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Blue];

                IsRequestingFoodToggle.isOn   = CurrentSummary.IsRequestingFoodUpkeep;
                IsRequestingYellowToggle.isOn = CurrentSummary.IsRequestingYellowUpkeep;
                IsRequestingWhiteToggle.isOn  = CurrentSummary.IsRequestingWhiteUpkeep;
                IsRequestingBlueToggle.isOn   = CurrentSummary.IsRequestingBlueUpkeep;
            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            PriorityInput.text = "0";
            EfficiencyField.text = "0";

            FirstEndpointFoodPermissionToggle.isOn   = false;
            FirstEndpointYellowPermissionToggle.isOn = false;
            FirstEndpointWhitePermissionToggle.isOn  = false;
            FirstEndpointBluePermissionToggle.isOn   = false;

            SecondEndpointFoodPermissionToggle.isOn   = false;            
            SecondEndpointYellowPermissionToggle.isOn = false;            
            SecondEndpointWhitePermissionToggle.isOn  = false;
            SecondEndpointBluePermissionToggle.isOn   = false;

            IsRequestingFoodToggle.isOn   = false;
            IsRequestingYellowToggle.isOn = false;
            IsRequestingWhiteToggle.isOn  = false;
            IsRequestingBlueToggle.isOn   = false;
        }

        #endregion

        private void AlignPermissionPanes() {
            if(CurrentSummary == null) {
                return;
            }

            bool firstEndpointIsToRight = CurrentSummary.FirstEndpoint.x >= CurrentSummary.SecondEndpoint.x;
            bool firstEndpointIsAbove = CurrentSummary.FirstEndpoint.y >= CurrentSummary.SecondEndpoint.y;

            if(firstEndpointIsToRight) {
                if(firstEndpointIsAbove) {
                    //FirstEndpointPane is aligned with its bottom-left corner
                    FirstEndpointPane.anchoredPosition = new Vector2(0, 0);
                    FirstEndpointPane.pivot = new Vector2(0, 0);
                    //SecondEndpointPane is aligned with its top-right corner
                    SecondEndpointPane.anchoredPosition = new Vector2(1, 1);
                    SecondEndpointPane.pivot = new Vector2(1, 1);
                }else {
                    //FirstEndpointPane is aligned with its top-left corner
                    FirstEndpointPane.anchoredPosition = new Vector2(0, 1);
                    FirstEndpointPane.pivot = new Vector2(0, 1);
                    //SecondEndpointPane is aligned with its bottom right corner
                    SecondEndpointPane.anchoredPosition = new Vector2(1, 0);
                    SecondEndpointPane.pivot = new Vector2(1, 0);
                }  
            }else {
                if(firstEndpointIsAbove) {
                    //FirstEndpointPane is aligned with its bottom-right corner
                    FirstEndpointPane.anchoredPosition = new Vector2(1, 0);
                    FirstEndpointPane.pivot = new Vector2(1, 0);
                    //SecondEndpointPane is aligned with its top-left corner
                    SecondEndpointPane.anchoredPosition = new Vector2(0, 1);
                    SecondEndpointPane.pivot = new Vector2(0, 1);
                }else {
                    //FirstEndpointPane is aligned top-right corner
                    FirstEndpointPane.anchoredPosition = new Vector2(1, 1);
                    FirstEndpointPane.pivot = new Vector2(1, 1);
                    //SecondEndpointPane is aligned with its bottom-left corner
                    SecondEndpointPane.anchoredPosition = new Vector2(0, 0);
                    SecondEndpointPane.pivot = new Vector2(0, 0);
                }
            }

            FirstEndpointPane.transform.position  = Camera.main.WorldToScreenPoint(CurrentSummary.FirstEndpoint);
            SecondEndpointPane.transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.SecondEndpoint);
        }

        #endregion

    }

}
