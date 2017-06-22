using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    [Serializable]
    public class SerializableHighwayData {

        #region instance fields and properties

        public int ID;

        public int FirstEndpointID;
        public int SecondEndpointID;

        public int Priority;

        public float Efficiency;

        public Dictionary<ResourceType, bool> UpkeepRequestedForResource;

        public Dictionary<ResourceType, bool> PullingPermissionForFirstEndpoint;
        public Dictionary<ResourceType, bool> PullingPermissionForSecondEndpoint;

        #endregion

        #region constructors

        public SerializableHighwayData(BlobHighwayBase highway) {
            ID = highway.ID;
            FirstEndpointID = highway.FirstEndpoint.ID;
            SecondEndpointID = highway.SecondEndpoint.ID;
            Priority = highway.Priority;
            Efficiency = highway.Efficiency;

            UpkeepRequestedForResource = new Dictionary<ResourceType, bool>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highway.GetUpkeepRequestedForResource(resourceType)) {
                    UpkeepRequestedForResource[resourceType] = true;
                }
            }

            PullingPermissionForFirstEndpoint = new Dictionary<ResourceType, bool>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highway.GetPullingPermissionForFirstEndpoint(resourceType)) {
                    PullingPermissionForFirstEndpoint[resourceType] = true;
                }
            }

            PullingPermissionForSecondEndpoint = new Dictionary<ResourceType, bool>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(highway.GetPullingPermissionForSecondEndpoint(resourceType)) {
                    PullingPermissionForSecondEndpoint[resourceType] = true;
                }
            }
        }

        #endregion

    }

}
