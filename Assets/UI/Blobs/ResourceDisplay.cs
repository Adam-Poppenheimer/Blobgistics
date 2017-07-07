using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Blobs {

    public class ResourceDisplay : ResourceDisplayBase {

        #region static fields and properties

        private static string PreambleText = "{0} of some combination of";

        #endregion

        #region instance fields and properties

        [SerializeField] private ResourceTypeColoredCountDisplay ColoredDisplayPrefab;
        [SerializeField] private RectTransform TransformToPinUnder;

        [SerializeField] private MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private bool DisplayResourceName;

        [SerializeField] private Text FlexibleCostPreamble;

        private Dictionary<ResourceType, ResourceTypeColoredCountDisplay> DisplayOfResourceTypes =
            new Dictionary<ResourceType, ResourceTypeColoredCountDisplay>();

        #endregion

        #region instance methods

        #region from BlobCollectionDisplayBase

        public override void PushAndDisplaySummary(IntPerResourceDictionary summary) {
            PushAndDisplaySummary(summary.ToReadOnlyDictionary());
        }

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

                displayForResource.DisplayCount              = true;
                displayForResource.DisplayMaterial           = true;
                displayForResource.DisplayName               = DisplayResourceName;

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

        public override void PushAndDisplaySummary(IEnumerable<ResourceType> typesAccepted, int countNeeded) {
            FlexibleCostPreamble.gameObject.SetActive(true);
            FlexibleCostPreamble.text = string.Format(PreambleText, countNeeded);

            foreach(var resourceType in typesAccepted) {
                ResourceTypeColoredCountDisplay displayForResource;
                DisplayOfResourceTypes.TryGetValue(resourceType, out displayForResource);

                if(displayForResource == null) {
                    displayForResource = Instantiate(ColoredDisplayPrefab.gameObject).GetComponent<ResourceTypeColoredCountDisplay>();
                    displayForResource.transform.SetParent(TransformToPinUnder, false);
                    displayForResource.gameObject.SetActive(true);

                    displayForResource.MaterialsForResourceTypes = MaterialsForResourceTypes;

                    DisplayOfResourceTypes[resourceType] = displayForResource;
                }

                displayForResource.DisplayCount              = false;
                displayForResource.DisplayMaterial           = true;
                displayForResource.DisplayName               = DisplayResourceName;

                displayForResource.gameObject.SetActive(true);
                displayForResource.transform.SetAsLastSibling();
                displayForResource.DisplayCountOfResourceType(resourceType, 0);
            }
        }

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
