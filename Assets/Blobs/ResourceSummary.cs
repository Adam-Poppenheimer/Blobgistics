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
    public class ResourceSummary : MonoBehaviour, IEnumerable<ResourceType> {

        #region instance fields and properties

        public int this[ResourceType type] {
            get {
                return CountList[(int)type];
            }
            private set {
                CountList[(int)type] = value;
            }
        }

        [SerializeField] private List<int> CountList = new List<int>();

        #endregion

        #region static methods

        public static ResourceSummary BuildResourceSummary(GameObject objectToAddTo) {
            var newSummary = objectToAddTo.AddComponent<ResourceSummary>();
            if(newSummary.CountList.Count != EnumUtil.GetValues<ResourceType>().Count()) {
                newSummary.CountList.Clear();
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    newSummary.CountList.Add(0);
                }
            }
            return newSummary;
        }

        public static ResourceSummary BuildResourceSummary(GameObject objectToAddTo, Dictionary<ResourceType, int> resourceCountByType){
            var newSummary = BuildResourceSummary(objectToAddTo);
            foreach(var pair in resourceCountByType) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        public static ResourceSummary BuildResourceSummary(GameObject objectToAddTo, params KeyValuePair<ResourceType, int>[] resourcePairs) {
            var newSummary = BuildResourceSummary(objectToAddTo);
            foreach(var pair in resourcePairs) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = CountList.Count; i < resourceTypeCount; ++i) {
                CountList.Add(0);
            }
        }

        private void Reset() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = CountList.Count; i < resourceTypeCount; ++i) {
                CountList.Add(0);
            }
        }

        #endregion

        #region from IEnumerable<ResourceType>

        public IEnumerator<ResourceType> GetEnumerator() {
            return EnumUtil.GetValues<ResourceType>().GetEnumerator();
        }

        #endregion

        #region from IEnumerable

        IEnumerator IEnumerable.GetEnumerator() {
            return CountList.GetEnumerator();
        }

        #endregion

        public int GetTotalResourceCount() {
            return CountList.Sum();
        }

        public bool IsContainedWithinBlobSite(BlobSiteBase site) {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(site.GetContentsOfType(resourceType).Count() < this[resourceType]) {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }

}
