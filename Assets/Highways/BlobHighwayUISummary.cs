using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways {

    public class BlobHighwayUISummary {

        #region instance fields and properties

        public int ID { get; private set; }

        public int Priority { get; private set; }

        public Transform Transform { get; private set; }

        public IReadOnlyDictionary<ResourceType, bool> ResourcePermissionsForEndpoint1 {
            get { return new ReadOnlyDictionary<ResourceType, bool>(_resourcePermissionsForEndpoint1); }
        }
        private Dictionary<ResourceType, bool> _resourcePermissionsForEndpoint1 = new Dictionary<ResourceType, bool>();

        public IReadOnlyDictionary<ResourceType, bool> ResourcePermissionsForEndpoint2 {
            get { return new ReadOnlyDictionary<ResourceType, bool>(_resourcePermissionsForEndpoint2); }
        }
        private Dictionary<ResourceType, bool> _resourcePermissionsForEndpoint2 = new Dictionary<ResourceType, bool>();

        #endregion

        #region constructors

        public BlobHighwayUISummary(BlobHighwayBase highwayToSummarize) {
            ID = highwayToSummarize.ID;
            Priority = highwayToSummarize.Priority;
            Transform = highwayToSummarize.transform;

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                _resourcePermissionsForEndpoint1[resourceType] = highwayToSummarize.GetPermissionForEndpoint1(resourceType);
                _resourcePermissionsForEndpoint2[resourceType] = highwayToSummarize.GetPermissionForEndpoint2(resourceType);
            }
        }

        #endregion

    }

}
