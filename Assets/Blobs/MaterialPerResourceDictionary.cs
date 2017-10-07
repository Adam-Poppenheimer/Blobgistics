using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

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
    public class MaterialPerResourceDictionary : PerResourceDictionaryBase<Material> {

        #region instance fields and properties

        #region from ResourceSummaryBase<Material>

        /// <inheritdoc/>
        protected override Material DefaultValue {
            get { return null; }
        }

        #endregion

        #endregion

        #region static methods

        /// <summary>
        /// Adds a summary to a given GameObject that contains only default values.
        /// </summary>
        /// <param name="objectToAddTo">The game object that will gain the dictionary as a new component</param>
        /// <returns>The created dictionary</returns>
        public static MaterialPerResourceDictionary BuildSummary(GameObject objectToAddTo) {
            var newSummary = objectToAddTo.AddComponent<MaterialPerResourceDictionary>();
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

        /// <summary>
        /// Adds a summary to a given game object whose values are informed by a dictionary of resource types
        /// </summary>
        /// <param name="objectToAddTo">The game object that will gain the dictionary as a new component</param>
        /// <param name="resourceCountByType">The dictionary to extract values from</param>
        /// <returns>The created dictionary</returns>
        public static MaterialPerResourceDictionary BuildSummary(GameObject objectToAddTo, Dictionary<ResourceType, Material> resourceCountByType){
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
        public static MaterialPerResourceDictionary BuildSummary(GameObject objectToAddTo, params KeyValuePair<ResourceType, Material>[] resourcePairs) {
            var newSummary = BuildSummary(objectToAddTo);
            foreach(var pair in resourcePairs) {
                newSummary[pair.Key] = pair.Value;
            }
            return newSummary;
        }

        #endregion

    }

}
