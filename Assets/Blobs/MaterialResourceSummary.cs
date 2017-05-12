using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    public class MaterialResourceSummary : ResourceSummaryBase<Material> {

        #region instance fields and properties

        #region from ResourceSummaryBase<Material>

        protected override Material DefaultValue {
            get { return null; }
        }

        #endregion

        #endregion

        #region static methods

        public static MaterialResourceSummary BuildSummary(GameObject objectToAddTo) {
            var newSummary = objectToAddTo.AddComponent<MaterialResourceSummary>();
            if(newSummary.ValueList.Count != EnumUtil.GetValues<ResourceType>().Count()) {
                newSummary.ValueList.Clear();
                #pragma warning disable 0168
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    newSummary.ValueList.Add(null);
                }
                #pragma warning restore 0168
            }
            return newSummary;
        }

        public static MaterialResourceSummary BuildSummary(GameObject objectToAddTo, Dictionary<ResourceType, Material> resourceCountByType){
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourceCountByType) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        public static MaterialResourceSummary BuildSummary(GameObject objectToAddTo, params KeyValuePair<ResourceType, Material>[] resourcePairs) {
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourcePairs) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        #endregion

    }

}
