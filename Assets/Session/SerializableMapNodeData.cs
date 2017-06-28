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

    [Serializable]
    [DataContract()]
    public class SerializableMapNodeData {

        #region instance fields and properties

        [DataMember()] public BlobSitePermissionProfile CurrentBlobSitePermissionProfile;

        [DataMember()] public int ID;
        
        [DataMember()] public SerializableVector3 LocalPosition;

        [DataMember()] public Dictionary<ResourceType, int> ResourceStockpileOfType;

        [DataMember()] public TerrainType LandType;

        #endregion

        #region constructors

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
