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

    /// <summary>
    /// The standard implementation of BlobHighwaySummaryDisplayBase, which gives players information
    /// and commands relating to highways.
    /// </summary>
    [ExecuteInEditMode]
    public class BlobHighwaySummaryDisplay : BlobHighwaySummaryDisplayBase {

        #region instance fields and properties

        #region from BlobHighwaySummaryDisplayBase

        /// <inheritdoc/>
        public override BlobHighwayUISummary CurrentSummary {
            get { return _currentSummary; }
            set { _currentSummary = value; }
        }
        private BlobHighwayUISummary _currentSummary;

        #endregion

        [SerializeField] private Text EfficiencyField;

        [SerializeField] private RectTransform CommonActionsPane;
        [SerializeField] private RectTransform FirstEndpointPane;
        [SerializeField] private RectTransform SecondEndpointPane;

        [SerializeField] private MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private TogglePerResourceDictionary FirstEndpointTogglesForResourceTypes;
        [SerializeField] private TogglePerResourceDictionary SecondEndpointTogglesForResourceTypes;

        [SerializeField] private TogglePerResourceDictionary UpkeepRequestTogglesForResourceTypes;

        #endregion

        #region instance methods

        #region Unity event methods

        /// <inheritdoc/>
        protected override void DoOnUpdate() {
            if(CurrentSummary != null) {
                FirstEndpointPane.transform.position  = Camera.main.WorldToScreenPoint(CurrentSummary.FirstEndpoint);
                SecondEndpointPane.transform.position = Camera.main.WorldToScreenPoint(CurrentSummary.SecondEndpoint);
            }
        }

        /*
         * This block of code modifies the colors of the various permission and upkeep
         * toggles to match those defined by MaterialsForResourceTypes. To change those
         * colors, it makes some very specific assumptions about the form the toggles
         * have taken. If you wanted to decouple this code from the component structure
         * surrounding the Toggle elements, you'd probably want to create a new class
         * (say, HighwayToggle) that obscures these details and modify the color through
         * that.
         */
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

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            AlignPermissionPanes();

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

        /// <inheritdoc/>
        protected override void DoOnDeactivate() {
            gameObject.SetActive(false);

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

        /// <inheritdoc/>
        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                EfficiencyField.text = CurrentSummary.Efficiency.ToString();

                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    var firstEndpointToggle = FirstEndpointTogglesForResourceTypes[resourceType];
                    if(firstEndpointToggle != null) {
                        firstEndpointToggle.isOn = CurrentSummary.ResourcePermissionsForFirstEndpoint[resourceType];
                    }

                    var secondEndpointToggle = SecondEndpointTogglesForResourceTypes[resourceType];
                    if(secondEndpointToggle != null) {
                        secondEndpointToggle.isOn = CurrentSummary.ResourcePermissionsForSecondEndpoint[resourceType];
                    }

                    var upkeepRequestToggle = UpkeepRequestTogglesForResourceTypes[resourceType];
                    if(upkeepRequestToggle != null) {
                        upkeepRequestToggle.isOn = CurrentSummary.IsRequestingUpkeepForResource[resourceType];
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void ClearDisplay() {
            CurrentSummary = null;

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

        /// <summary>
        /// Aligns the permissions panes so that they lie next to the
        /// endpoints they refer to, but offset enough so that they don't obscure the
        /// highway itself.
        /// </summary>
        /// <remarks>
        /// This implementation tries to match the appropriate corner of the endpoint
        /// panes to their corresponding endpoints. It does this by determining the
        /// orientation of the two endpoints and then changing the pivots of the
        /// endpoint panes so that the panes bracket the highway without overlapping
        /// it very much. Since the positions of the endpoint panes has already been
        /// set to the endpoints themselves, this is sufficient for a reasonable behavior.
        /// </remarks>
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
