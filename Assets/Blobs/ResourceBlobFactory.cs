using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    public class ResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance fields and properties

        [SerializeField] private Material MaterialForFood   = null;
        [SerializeField] private Material MaterialForYellow = null;
        [SerializeField] private Material MaterialForWhite  = null;
        [SerializeField] private Material MaterialForBlue   = null;

        private Dictionary<ResourceType, Material> MaterialDict =
            new Dictionary<ResourceType, Material>();

        [SerializeField] private GameObject BlobPrefab = null;

        private List<ResourceBlobBase> InstantiatedBlobs = 
            new List<ResourceBlobBase>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            MaterialDict[ResourceType.Food  ] = MaterialForFood;
            MaterialDict[ResourceType.Yellow] = MaterialForYellow;
            MaterialDict[ResourceType.White ] = MaterialForWhite;
            MaterialDict[ResourceType.Blue  ] = MaterialForBlue;
        }

        #endregion

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var prefabClone = Instantiate<GameObject>(BlobPrefab);
            var newBlob = prefabClone.GetComponent<ResourceBlob>();
            if(newBlob == null) {
                throw new BlobException("BlobBuilder's BlobPrefab lacks a ResourceBlob component");
            }
            prefabClone.transform.position = (Vector3)startingXYCoordinates + new Vector3(0f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs);

            var blobRenderer = prefabClone.GetComponent<MeshRenderer>();
            if(blobRenderer != null) {
                blobRenderer.material = MaterialDict[typeOfResource];
            }
            newBlob.BlobType = typeOfResource;
            newBlob.gameObject.name = string.Format("Blob ({0})", typeOfResource);

            InstantiatedBlobs.Add(newBlob);

            return newBlob;
        }

        public override void DestroyBlob(ResourceBlobBase blob) {
            InstantiatedBlobs.Remove(blob);
            DestroyImmediate(blob.gameObject);
        }

        public override void TickAllBlobs() {
            throw new NotImplementedException();
        }

        #endregion

    }

}
