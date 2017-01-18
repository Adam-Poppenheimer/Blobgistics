using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public static class ResourceBlobBuilder {

        #region static fields and properties

        private static GameObject BlobPrefab {
            get {
                if(_blobPrefab == null) {
                    _blobPrefab = Resources.Load("BlobPrefab") as GameObject;
                }
                return _blobPrefab;
            }
        }
        private static GameObject _blobPrefab;

        private static Dictionary<ResourceType, Material> MaterialDict =
            new Dictionary<ResourceType, Material>();

        #endregion

        #region static constructors

        static ResourceBlobBuilder() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                MaterialDict[resourceType] = Resources.Load<Material>("ResourceTypeMaterials/" + resourceType.ToString());
            }
        }

        #endregion

        #region static methods

        public static ResourceBlob BuildBlob(ResourceType typeOfResource, Vector3 startingLocation) {
            var blobGameObject = GameObject.Instantiate<GameObject>(BlobPrefab);
            var blobComponent = blobGameObject.GetComponent<ResourceBlob>();
            if(blobComponent == null) {
                throw new BlobException("BlobBuilder's BlobPrefab lacks a ResourceBlob component");
            }
            blobGameObject.transform.position = startingLocation;
            var blobRenderer = blobGameObject.GetComponent<MeshRenderer>();
            if(blobRenderer != null) {
                blobRenderer.material = MaterialDict[typeOfResource];
            }
            return blobComponent;
        }

        #endregion

    }

}
