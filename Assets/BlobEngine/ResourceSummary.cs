using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class ResourceSummary : IEnumerable<ResourceType> {

        #region static fields and properties

        public static ResourceSummary Empty {
            get {
                if(_empty == null) {
                    _empty = new ResourceSummary();
                }
                return _empty;
            }
        }
        private static ResourceSummary _empty;

        #endregion

        #region instance fields and properties

        public int this[ResourceType type] {
            get {
                int retval;
                ResourceCountByType.TryGetValue(type, out retval);
                return retval;
            }
        }

        private readonly IReadOnlyDictionary<ResourceType, int> ResourceCountByType;

        #endregion

        #region constructors

        public ResourceSummary(Dictionary<ResourceType, int> resourceCountByType) {
            ResourceCountByType = new ReadOnlyDictionary<ResourceType, int>(
                new Dictionary<ResourceType, int>(resourceCountByType)
            );
        }

        public ResourceSummary(params KeyValuePair<ResourceType, int>[] resourcePairs) {
            var newDict = new Dictionary<ResourceType, int>();
            foreach(var pair in resourcePairs) {
                newDict.Add(pair.Key, pair.Value);
            }
            ResourceCountByType = new ReadOnlyDictionary<ResourceType, int>(newDict);
        }

        #endregion

        #region instance methods

        #region from IEnumerable<ResourceType>

        public IEnumerator<ResourceType> GetEnumerator() {
            return ResourceCountByType.Keys.GetEnumerator();
        }

        #endregion

        #region from IEnumerable

        IEnumerator IEnumerable.GetEnumerator() {
            return ResourceCountByType.Keys.GetEnumerator();
        }

        #endregion

        public int GetTotalResourceCount() {
            return ((IDictionary<ResourceType, int>)ResourceCountByType).Values.Sum();
        }

        public int GetCountOfResourceType(ResourceType type) {
            int retval;
            ResourceCountByType.TryGetValue(type, out retval);
            return retval;
        }

        public bool IsContainedWithinBlobPile(BlobPileBase pile) {
            foreach(var resourceType in ResourceCountByType.Keys) {
                int countOfResource;
                ResourceCountByType.TryGetValue(resourceType, out countOfResource);
                if(pile.GetAllBlobsOfType(resourceType).Count() < countOfResource) {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }

}
