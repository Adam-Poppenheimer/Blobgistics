using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a map node.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableMapNodeData {

        #region instance fields and properties

        /// <summary>
        /// The permissions and capacities of the blob site within the node.
        /// </summary>
        [DataMember()] public BlobSitePermissionProfile CurrentBlobSitePermissionProfile;

        /// <summary>
        /// The ID of the node.
        /// </summary>
        [DataMember()] public int ID;
        
        /// <summary>
        /// The local position, in terms of the MapGraph, of the node.
        /// </summary>
        [DataMember()] public SerializableVector3 LocalPosition;

        /// <summary>
        /// A record of the resources currently stockpiled in the node.
        /// </summary>
        [DataMember()] public Dictionary<ResourceType, int> ResourceStockpileOfType;

        /// <summary>
        /// The terrain type of the node.
        /// </summary>
        [DataMember()] public TerrainType LandType;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given node.
        /// </summary>
        /// <param name="node"></param>
        public SerializableMapNodeData(MapNodeBase node) {
            ID = node.ID;
            LocalPosition = node.transform.localPosition;
            LandType = node.Terrain;
            ResourceStockpileOfType = new Dictionary<ResourceType, int>();
            foreach(var blob in node.BlobSite.Contents) {
                int currentCount;
                ResourceStockpileOfType.TryGetValue(blob.BlobType, out currentCount);
                ++currentCount;
                ResourceStockpileOfType[blob.BlobType] = currentCount;
            }
            CurrentBlobSitePermissionProfile = BlobSitePermissionProfile.BuildFromBlobSite(node.BlobSite);
        }

        #endregion

    }

}
