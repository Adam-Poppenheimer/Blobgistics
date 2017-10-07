using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a highway.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableHighwayData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the highway.
        /// </summary>
        [DataMember()] public int ID;

        /// <summary>
        /// The ID of the highway's first endpoint.
        /// </summary>
        [DataMember()] public int FirstEndpointID;

        /// <summary>
        /// The ID of the highway's second endpoint.
        /// </summary>
        [DataMember()] public int SecondEndpointID;

        /// <summary>
        /// The priority of the highway.
        /// </summary>
        /// <remarks>
        /// This property is now redundant and should probably be removed in the next refactor.
        /// </remarks>
        [DataMember()] public int Priority;

        /// <summary>
        /// The efficiency of the highway.
        /// </summary>
        [DataMember()] public float Efficiency;

        /// <summary>
        /// The per-resource upkeep requests for the highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> UpkeepRequestedForResource;

        /// <summary>
        /// The per-resource pulling permissions for the first endpoint of the highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> PullingPermissionForFirstEndpoint;

        /// <summary>
        /// The per-resource pulling permissions for the second endpoint of the highway.
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)] public Dictionary<ResourceType, bool> PullingPermissionForSecondEndpoint;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given highway.
        /// </summary>
        /// <param name="highway">The highway to pull data from</param>
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
