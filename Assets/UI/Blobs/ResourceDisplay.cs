using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Blobs {

    /// <summary>
    /// The standard implementation for ResourceDisplayBase, which is designed to take resource
    /// summaries and display them with text and the corresponding colors of their
    /// materials.
    /// </summary>
    /// <remarks>
    /// This implementation solves the resource display problem by creating objects of type
    /// ResourceTypeColoredCountDisplay for each resource type, configuring them, and then
    /// assigning them as children under a particular transform. Using a vertical layout
    /// group, this has the effect of displaying resource summaries as vertical lists.
    /// 
    /// All four of the methods use very similar logic, and there's an instance of the
    /// copy-paste antipattern between the IDictionary and flexible cost overloads that
    /// should probably be removed in the next refactor.
    /// </remarks>
    public class ResourceDisplay : ResourceDisplayBase {

        #region static fields and properties

        /// <summary>
        /// The string used to preface the display of a flexible cost.
        /// </summary>
        private static string PreambleText = "{0} of some combination of";

        #endregion

        #region instance fields and properties

        [SerializeField] private ResourceTypeColoredCountDisplay ColoredDisplayPrefab;

        [SerializeField] private RectTransform TransformToPinUnder;

        [SerializeField] private MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private bool DisplayResourceName;

        [SerializeField] private Text FlexibleCostPreamble;

        /// <summary>
        /// A cache of all the sub-displays used to render individual resources. Whenever the object
        /// needs to display a particular resource, it checks the cache first.
        /// </summary>
        private Dictionary<ResourceType, ResourceTypeColoredCountDisplay> DisplayOfResourceTypes =
            new Dictionary<ResourceType, ResourceTypeColoredCountDisplay>();

        #endregion

        #region instance methods

        #region from BlobCollectionDisplayBase

        /// <inheritdoc/>
        public override void PushAndDisplaySummary(IntPerResourceDictionary summary) {
            PushAndDisplaySummary(summary.ToReadOnlyDictionary());
        }

        /// <inheritdoc/>
        public override void PushAndDisplaySummary(IDictionary<ResourceType, int> summaryDictionary) {
            if(FlexibleCostPreamble != null) {
                FlexibleCostPreamble.gameObject.SetActive(false);
            }
            foreach(var resourceType in summaryDictionary.Keys) {
                ResourceTypeColoredCountDisplay displayForResource;
                DisplayOfResourceTypes.TryGetValue(resourceType, out displayForResource);

                if(displayForResource == null) {
                    displayForResource = Instantiate(ColoredDisplayPrefab.gameObject).GetComponent<ResourceTypeColoredCountDisplay>();
                    displayForResource.transform.SetParent(TransformToPinUnder, false);
                    displayForResource.gameObject.SetActive(true);

                    displayForResource.MaterialsForResourceTypes = MaterialsForResourceTypes;

                    DisplayOfResourceTypes[resourceType] = displayForResource;
                }

                displayForResource.WillDisplayCount              = true;
                displayForResource.WillDisplayMaterial           = true;
                displayForResource.WillDisplayName               = DisplayResourceName;

                int countForResource = summaryDictionary[resourceType];
                if(countForResource <= 0) {
                    displayForResource.gameObject.SetActive(false);
                    continue;
                }

                displayForResource.gameObject.SetActive(true);
                displayForResource.transform.SetAsLastSibling();
                displayForResource.DisplayCountOfResourceType(resourceType, countForResource);
            }
        }

        /// <inheritdoc/>
        public override void PushAndDisplaySummary(IEnumerable<ResourceType> typesAccepted, int countNeeded) {
            FlexibleCostPreamble.gameObject.SetActive(true);
            FlexibleCostPreamble.text = string.Format(PreambleText, countNeeded);

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                ResourceTypeColoredCountDisplay displayForResource;
                DisplayOfResourceTypes.TryGetValue(resourceType, out displayForResource);

                if(displayForResource == null) {
                    displayForResource = Instantiate(ColoredDisplayPrefab.gameObject).GetComponent<ResourceTypeColoredCountDisplay>();
                    displayForResource.transform.SetParent(TransformToPinUnder, false);
                    displayForResource.gameObject.SetActive(true);

                    displayForResource.MaterialsForResourceTypes = MaterialsForResourceTypes;

                    DisplayOfResourceTypes[resourceType] = displayForResource;
                }

                displayForResource.WillDisplayCount              = false;
                displayForResource.WillDisplayMaterial           = true;
                displayForResource.WillDisplayName               = DisplayResourceName;

                if(typesAccepted.Contains(resourceType)) {
                    displayForResource.gameObject.SetActive(true);
                    displayForResource.transform.SetAsLastSibling();
                    displayForResource.DisplayCountOfResourceType(resourceType, 0);
                }else {
                    displayForResource.gameObject.SetActive(false);
                }
            }
        }

        /// <inheritdoc/>
        public override void PushAndDisplayInfo(ResourceDisplayInfo infoToDisplay) {
            if(infoToDisplay.PerResourceDictionary != null) {
                PushAndDisplaySummary(infoToDisplay.PerResourceDictionary);
            }else if(infoToDisplay.TypesAccepted != null) {
                PushAndDisplaySummary(infoToDisplay.TypesAccepted, infoToDisplay.CountNeeded);
            }
        }

        #endregion

        #endregion

    }

}
