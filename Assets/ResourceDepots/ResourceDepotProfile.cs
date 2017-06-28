using System;
using System.Runtime.Serialization;

using UnityEngine;

using Assets.Blobs;

namespace Assets.ResourceDepots {

    [Serializable, DataContract]
    public struct ResourceDepotProfile {

        #region static fields and properties

        public static ResourceDepotProfile Empty {
            get { return _empty; }
        }
        private static ResourceDepotProfile _empty = new ResourceDepotProfile(0);

        #endregion

        #region instance fields and properties

        public int PerResourceCapacity {
            get { return _perResourceCapacity; }
        }
        [SerializeField, DataMember()] private int _perResourceCapacity;

        #endregion

        #region constructors

        public ResourceDepotProfile(int perResourceCapacity) {
            _perResourceCapacity = perResourceCapacity;
        }

        #endregion

    }

}