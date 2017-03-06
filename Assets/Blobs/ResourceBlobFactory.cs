using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    public class ResourceBlobFactory : ResourceBlobFactoryBase {

        #region static fields and properties

        [SerializeField] private Material MaterialForRed   = null;
        [SerializeField] private Material MaterialForGreen = null;
        [SerializeField] private Material MaterialForBlue  = null;

        private static Dictionary<ResourceType, Material> MaterialDict =
            new Dictionary<ResourceType, Material>();

        #endregion

        #region instance fields and properties

        [SerializeField] private GameObject BlobPrefab = null;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            MaterialDict[ResourceType.Red]   = MaterialForRed;
            MaterialDict[ResourceType.Green] = MaterialForGreen;
            MaterialDict[ResourceType.Blue]  = MaterialForBlue;
        }

        #endregion

        public override ResourceBlob BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public override ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var blobGameObject = Instantiate<GameObject>(BlobPrefab);
            var blobComponent = blobGameObject.GetComponent<ResourceBlob>();
            if(blobComponent == null) {
                throw new BlobException("BlobBuilder's BlobPrefab lacks a ResourceBlob component");
            }
            blobGameObject.transform.position = (Vector3)startingXYCoordinates +
                new Vector3(0f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs);
            var blobRenderer = blobGameObject.GetComponent<MeshRenderer>();
            if(blobRenderer != null) {
                blobRenderer.material = MaterialDict[typeOfResource];
            }
            return blobComponent;
        }

        public override void DestroyBlob(ResourceBlob blob) {
            Destroy(blob);
        }

        #endregion

    }

}
