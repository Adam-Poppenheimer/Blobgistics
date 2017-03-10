using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Depots {

    public class ResourceDepot : ResourceDepotBase {

        #region instance fields and properties

        #region from ResourceDepotBase

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        [SerializeField] private MapNodeBase _location;

        public override ResourceDepotProfile Profile {
            get { return _profile; }
            set {
                _profile = value;
                var blobSite = Location.BlobSite;
                int intendedTotalCapacity = 0;

                blobSite.ClearPermissionsAndCapacity();

                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    blobSite.SetPlacementPermissionForResourceType(resourceType, true);
                    blobSite.SetExtractionPermissionForResourceType(resourceType, true);
                    blobSite.SetCapacityForResourceType(resourceType, _profile.PerResourceCapacity);
                    intendedTotalCapacity += _profile.PerResourceCapacity;
                }

                blobSite.TotalCapacity = intendedTotalCapacity;
            }
        }
        [SerializeField, HideInInspector] private ResourceDepotProfile _profile;

        #endregion

        #endregion

        #region instance methods

        #region from ResourceDepotBase

        public override void Clear() {
            Location.BlobSite.ClearContents();
        }

        #endregion

        #endregion

    }

}
