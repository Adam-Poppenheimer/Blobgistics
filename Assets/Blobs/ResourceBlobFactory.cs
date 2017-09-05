using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;
using System.Collections.ObjectModel;

namespace Assets.Blobs {

    public class ResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance fields and properties

        #region from ResourceBlobFactoryBase

        public override ReadOnlyCollection<ResourceBlobBase> Blobs {
            get { return blobs.AsReadOnly(); }
        }
        private List<ResourceBlobBase> blobs = new List<ResourceBlobBase>();

        #endregion

        [SerializeField] private MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private GameObject BlobPrefab = null;

        #endregion

        #region instance methods

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
                blobRenderer.material = MaterialsForResourceTypes[typeOfResource];
            }
            newBlob.BlobType = typeOfResource;
            newBlob.gameObject.name = string.Format("Blob ({0})", typeOfResource);
            newBlob.ParentFactory = this;

            blobs.Add(newBlob);

            return newBlob;
        }

        public override void DestroyBlob(ResourceBlobBase blob) {
            UnsubscribeBlob(blob);
            if(Application.isPlaying) {
                Destroy(blob.gameObject);
            }else {
                DestroyImmediate(blob.gameObject);
            }
        }

        public override void UnsubscribeBlob(ResourceBlobBase blob) {
            blobs.Remove(blob);
        }

        public override void TickAllBlobs(float secondsPassed) {
            var blobsToTick = new List<ResourceBlobBase>(blobs);
            foreach(var blob in blobsToTick) {
                blob.Tick(secondsPassed);
            }
        }

        #endregion

    }

}
