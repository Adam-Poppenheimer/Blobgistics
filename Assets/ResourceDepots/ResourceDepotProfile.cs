using System;
using System.Runtime.Serialization;

using UnityEngine;

using Assets.Blobs;

namespace Assets.ResourceDepots {

    /// <summary>
    /// A POD struct that tells a ResourceDepot how much capacity it should 
    /// allow per resource.
    /// </summary>
    /// <remarks>
    /// This class seems trivial and unnecessary. Consider removing during
    /// next refactor.
    /// </remarks>
    [Serializable, DataContract]
    public struct ResourceDepotProfile {

        #region static fields and properties

        /// <summary>
        /// A profile with a per-resource capacity of zero.
        /// </summary>
        public static ResourceDepotProfile Empty {
            get { return _empty; }
        }
        private static ResourceDepotProfile _empty = new ResourceDepotProfile(0);

        #endregion

        #region instance fields and properties

        /// <summary>
        /// The per-resource capacity for all resources that the ResourceDepot should
        /// enforce.
        /// </summary>
        public int PerResourceCapacity {
            get { return _perResourceCapacity; }
        }
        [SerializeField, DataMember()] private int _perResourceCapacity;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a profile with the given PerResourceCapacity.
        /// </summary>
        /// <param name="perResourceCapacity">The PerResourceCapacity of this profile</param>
        public ResourceDepotProfile(int perResourceCapacity) {
            _perResourceCapacity = perResourceCapacity;
        }

        #endregion

    }

}