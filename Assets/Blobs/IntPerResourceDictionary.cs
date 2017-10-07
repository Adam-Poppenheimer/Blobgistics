using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    /// <summary>
    /// A PerResourceDictionary that accepts ints.
    /// </summary>
    /// <remarks>
    /// Since Unity doesn't permit generic MonoBehaviours, it's necessary to create separate
    /// classes for every desirable PerResourceDictionary type. This might not hold true if
    /// PerResourceDictionaryBase was a ScriptableObject.
    /// </remarks>
    [Serializable]
    public class IntPerResourceDictionary : PerResourceDictionaryBase<int> {

        #region instance fields and properties

        #region from ResourceSummaryBase<int>

        /// <inheritdoc/>
        protected override int DefaultValue {
            get { return 0; }
        }

        #endregion

        #endregion

        #region static methods

        /// <summary>
        /// Adds a summary to a given GameObject that contains only default values.
        /// </summary>
        /// <param name="objectToAddTo">The game object that will gain the dictionary as a new component</param>
        /// <returns>The created dictionary</returns>
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

        /// <summary>
        /// Adds a summary to a given game object whose values are informed by a dictionary of resource types
        /// </summary>
        /// <param name="objectToAddTo">The game object that will gain the dictionary as a new component</param>
        /// <param name="resourceCountByType">The dictionary to extract values from</param>
        /// <returns>The created dictionary</returns>
        public static IntPerResourceDictionary BuildSummary(GameObject objectToAddTo, Dictionary<ResourceType, int> resourceCountByType){
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourceCountByType) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        /// <summary>
        /// Adds a summary to a given game object whose values are informed by a list of KeyValuePairs
        /// </summary>
        /// <param name="objectToAddTo">The game object that will gain the dictionary as a new component</param>
        /// <param name="resourcePairs">A parameter list of KeyValuePairs that will inform the values of the dictionary</param>
        /// <returns>The created dictionary</returns>
        public static IntPerResourceDictionary BuildSummary(GameObject objectToAddTo, params KeyValuePair<ResourceType, int>[] resourcePairs) {
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourcePairs) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the total amount of resoruces contained within the dictioanry
        /// </summary>
        /// <returns>The sum of the dictionary's resource counts</returns>
        public int GetTotalResourceCount() {
            return ValueList.Sum();
        }

        /// <summary>
        /// Determines whether the resources specified by the dictionary are contained within a given blob site
        /// </summary>
        /// <param name="site">The site to be queried</param>
        /// <returns>Whether or not all the resources in the dictionary could be extracted from the blob site</returns>
        public bool IsContainedWithinBlobSite(BlobSiteBase site) {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(site.GetContentsOfType(resourceType).Count() < this[resourceType]) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a human-readable string that summarizes the contents of the dictionary
        /// </summary>
        /// <returns>A string that summarizes the contents of the dictionary</returns>
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
