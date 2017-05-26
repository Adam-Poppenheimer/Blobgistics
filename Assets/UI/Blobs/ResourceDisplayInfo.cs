using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.UI.Blobs {

    public class ResourceDisplayInfo {

        #region instance fields and properties

        public readonly IntPerResourceDictionary PerResourceDictionary;

        public readonly IEnumerable<ResourceType> TypesAccepted;
        public readonly int CountNeeded = -1;

        #endregion

        #region constructors

        public ResourceDisplayInfo(IntPerResourceDictionary perResourceDictionary) {
            PerResourceDictionary = perResourceDictionary;
        }

        public ResourceDisplayInfo(IEnumerable<ResourceType> resourceTypesPermitted, int totalCapacityRequired) {
            TypesAccepted = resourceTypesPermitted;
            CountNeeded = totalCapacityRequired;
        }

        #endregion

    }

}
