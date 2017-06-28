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

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways {

    [ExecuteInEditMode]
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

        [SerializeField] private MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private ToggleResourceSummary FirstEndpointTogglesForResourceTypes;
        [SerializeField] private ToggleResourceSummary SecondEndpointTogglesForResourceTypes;

        [SerializeField] private ToggleResourceSummary UpkeepRequestTogglesForResourceTypes;

        #endregion

        #region instance methods

        #region Unity event methods

        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                FirstEndpointPane.transform.position  = Camera.main.WorldToScreenPoint(CurrentSummary.FirstEndpoint);
                SecondEndpointPane.transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.SecondEndpoint);
            }
        }

        private void OnValidate() {
            if(MaterialsForResourceTypes != null) {
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    var materialForResource = MaterialsForResourceTypes[resourceType];

                    if(FirstEndpointTogglesForResourceTypes != null){
                        var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                        if(firstEndpointToggle != null) {
                            firstEndpointToggle.transform.GetChild(0).GetComponent<Image>().color = 
                                materialForResource != null ? materialForResource.color : Color.white;
                        }
                    }

                    if(SecondEndpointTogglesForResourceTypes != null) {
                        var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                        if(secondEndpointToggle != null) {
                            secondEndpointToggle.transform.GetChild(0).GetComponent<Image>().color = 
                                materialForResource != null ? materialForResource.color : Color.white;
                        }
                    }
                    
                    if(UpkeepRequestTogglesForResourceTypes != null) {
                        var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                        if(upkeepRequestToggle != null) {
                            upkeepRequestToggle.transform.GetChild(0).GetComponent<Image>().color = 
                                materialForResource != null ? materialForResource.color : Color.white;
                        }
                    }
                }
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

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                var cachedResourceType = resourceType;
                var materialForResource = MaterialsForResourceTypes[resourceType];

                var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                if(firstEndpointToggle != null) {
                    firstEndpointToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                        RaiseFirstEndpointPermissionChanged(cachedResourceType, newPermission);
                    });
                    var endpointColors = firstEndpointToggle.colors;
                    endpointColors.normalColor = materialForResource != null ? materialForResource.color : Color.white;
                }

                var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                if(secondEndpointToggle != null) {
                    secondEndpointToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                        RaiseSecondEndpointPermissionChanged(cachedResourceType, newPermission);
                    });
                    var endpointColors = secondEndpointToggle.colors;
                    endpointColors.normalColor = materialForResource != null ? materialForResource.color : Color.white;
                }

                var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                if(upkeepRequestToggle != null) {
                    upkeepRequestToggle.onValueChanged.AddListener(delegate(bool isBeingRequested) {
                        RaiseResourceRequestedForUpkeep(cachedResourceType, isBeingRequested);
                    });
                    var upkeepColors = upkeepRequestToggle.colors;
                    upkeepColors.normalColor = materialForResource != null ? materialForResource.color : Color.white;
                }
            }
        }

        protected override void DoOnDeactivate() {
            gameObject.SetActive(false);

            PriorityInput.onEndEdit.RemoveAllListeners();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                if(firstEndpointToggle != null) {
                    firstEndpointToggle.onValueChanged.RemoveAllListeners();
                }

                var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                if(secondEndpointToggle != null) {
                    secondEndpointToggle.onValueChanged.RemoveAllListeners();
                }

                var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                if(upkeepRequestToggle != null) {
                    upkeepRequestToggle.onValueChanged.RemoveAllListeners();
                }
            }
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                PriorityInput.text = CurrentSummary.Priority.ToString();
                EfficiencyField.text = CurrentSummary.Efficiency.ToString();

                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                    if(firstEndpointToggle != null) {
                        firstEndpointToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[resourceType];
                    }

                    var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                    if(secondEndpointToggle != null) {
                        secondEndpointToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[resourceType];
                    }

                    var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                    if(upkeepRequestToggle != null) {
                        upkeepRequestToggle.isOn = CurrentSummary.IsRequestingUpkeepForResource[resourceType];
                    }
                }

            }
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            PriorityInput.text = "0";
            EfficiencyField.text = "0";

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                if(firstEndpointToggle != null) {
                    firstEndpointToggle.isOn = false;
                }

                var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                if(secondEndpointToggle != null) {
                    secondEndpointToggle.isOn = false;
                }

                var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                if(upkeepRequestToggle != null) {
                    upkeepRequestToggle.isOn = false;
                }
            }
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
