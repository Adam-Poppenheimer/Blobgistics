using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.HighwayUpgraders;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways {

    public class BlobHighwayUISummary {

        #region instance fields and properties

        public int ID { get; set; }

        public int Priority { get; set; }

        public Transform Transform { get; set; }

        public Dictionary<ResourceType, bool> ResourcePermissionsForEndpoint1 { get; set; }
        public Dictionary<ResourceType, bool> ResourcePermissionsForEndpoint2 { get; set; }

        public BlobHighwayProfileBase Profile { get; set; }

        public bool IsBeingUpgraded { get; set; }

        public Vector3 FirstEndpoint  { get; set; }
        public Vector3 SecondEndpoint { get; set; }

        #endregion

        #region constructors

        public BlobHighwayUISummary() {}

        public BlobHighwayUISummary(BlobHighwayBase highwayToSummarize) {
            ID = highwayToSummarize.ID;
            Priority = highwayToSummarize.Priority;
            Transform = highwayToSummarize.transform;

            ResourcePermissionsForEndpoint1 = new Dictionary<ResourceType, bool>();
            ResourcePermissionsForEndpoint2 = new Dictionary<ResourceType, bool>();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                ResourcePermissionsForEndpoint1[resourceType] = highwayToSummarize.GetPullingPermissionForFirstEndpoint(resourceType);
                ResourcePermissionsForEndpoint2[resourceType] = highwayToSummarize.GetPullingPermissionForSecondEndpoint(resourceType);
            }

            Profile = highwayToSummarize.Profile;

            Vector3 firstEndpoint, secondEndpoint;
            highwayToSummarize.GetEndpointPositions(out firstEndpoint, out secondEndpoint);
            FirstEndpoint = firstEndpoint;
            SecondEndpoint = secondEndpoint;
        }

        #endregion

    }

}
