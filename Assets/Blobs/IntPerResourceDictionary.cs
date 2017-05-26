using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    [Serializable]
    public class IntPerResourceDictionary : PerResourceDictionaryBase<int>, IEnumerable<ResourceType> {

        #region instance fields and properties

        #region from ResourceSummaryBase<int>

        protected override int DefaultValue {
            get { return 0; }
        }

        #endregion

        #endregion

        #region static methods

        public static IntPerResourceDictionary BuildSummary(GameObject objectToAddTo) {
            var newSummary = objectToAddTo.AddComponent<IntPerResourceDictionary>();
            if(newSummary.ValueList.Count != EnumUtil.GetValues<ResourceType>().Count()) {
                newSummary.ValueList.Clear();
                #pragma warning disable 0168
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    newSummary.ValueList.Add(0);
                }
                #pragma warning restore 0168
            }
            return newSummary;
        }

        public static IntPerResourceDictionary BuildSummary(GameObject objectToAddTo, Dictionary<ResourceType, int> resourceCountByType){
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourceCountByType) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        public static IntPerResourceDictionary BuildSummary(GameObject objectToAddTo, params KeyValuePair<ResourceType, int>[] resourcePairs) {
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourcePairs) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        #endregion

        #region instance methods

        public int GetTotalResourceCount() {
            return ValueList.Sum();
        }

        public bool IsContainedWithinBlobSite(BlobSiteBase site) {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(site.GetContentsOfType(resourceType).Count() < this[resourceType]) {
                    return false;
                }
            }
            return true;
        }

        public string GetSummaryString() {
            var retval = "";
            foreach(var resourceType in this) {
                if(this[resourceType] != 0) {
                    retval += string.Format("{0} : {1}\n", resourceType, this[resourceType]);
                }
            }
            return retval;
        }

        #endregion

    }

}
