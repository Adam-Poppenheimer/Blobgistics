using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class ResourceBlobFactory : MonoBehaviour {

        #region static fields and properties

        private static Dictionary<ResourceType, Material> MaterialDict =
            new Dictionary<ResourceType, Material>();

        #endregion

        #region instance fields and properties

        [SerializeField] private GameObject BlobPrefab = null;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                MaterialDict[resourceType] = Resources.Load<Material>("ResourceTypeMaterials/" + resourceType.ToString());
            }
        }

        #endregion

        public ResourceBlob BuildBlob(ResourceType typeOfResource, Vector3 startingLocation) {
            var blobGameObject = Instantiate<GameObject>(BlobPrefab);
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
