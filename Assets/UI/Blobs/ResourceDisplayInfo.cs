using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.UI.Blobs {

    /// <summary>
    /// A simple POD class that contains information for displaying resource costs.
    /// </summary>
    public class ResourceDisplayInfo {

        #region instance fields and properties

        /// <summary>
        /// The per-resource dictionary used when the cost is fixed. Set to null otherwise.
        /// </summary>
        public readonly IntPerResourceDictionary PerResourceDictionary;

        /// <summary>
        /// A number of types accepted when the cost is flexible. Set to null otherwise.
        /// </summary>
        public readonly IEnumerable<ResourceType> TypesAccepted;

        /// <summary>
        /// The number of blobs required when the cost is flexible. Set to -1 otherwise.
        /// </summary>
        public readonly int CountNeeded = -1;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes this object with a fixed cost.
        /// </summary>
        /// <param name="perResourceDictionary">The dictionary containing the fixed costs</param>
        public ResourceDisplayInfo(IntPerResourceDictionary perResourceDictionary) {
            PerResourceDictionary = perResourceDictionary;
        }

        /// <summary>
        /// Initializes this object with a flexible cost. 
        /// </summary>
        /// <param name="typesAccepted">Resources that can be spent on the cost</param>
        /// <param name="countNeeded">The number of blobs required</param>
        public ResourceDisplayInfo(IEnumerable<ResourceType> typesAccepted, int countNeeded) {
            TypesAccepted = typesAccepted;
            CountNeeded = countNeeded;
        }

        #endregion

    }

}
