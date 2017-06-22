using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableHighwayData {

        #region instance fields and properties

        [DataMember()] public int ID;

        [DataMember()] public int FirstEndpointID;
        [DataMember()] public int SecondEndpointID;

        [DataMember()] public int Priority;

        [DataMember()] public float Efficiency;

        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> UpkeepRequestedForResource;

        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> PullingPermissionForFirstEndpoint;
        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> PullingPermissionForSecondEndpoint;

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
                UpkeepRequestedForResource[resourceType] = highway.GetUpkeepRequestedForResource(resourceType);
            }

            PullingPermissionForFirstEndpoint = new Dictionary<ResourceType, bool>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                 PullingPermissionForFirstEndpoint[resourceType] = highway.GetPullingPermissionForFirstEndpoint(resourceType);
            }

            PullingPermissionForSecondEndpoint = new Dictionary<ResourceType, bool>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                PullingPermissionForSecondEndpoint[resourceType] = highway.GetPullingPermissionForSecondEndpoint(resourceType);
            }
        }

        #endregion

    }

}
