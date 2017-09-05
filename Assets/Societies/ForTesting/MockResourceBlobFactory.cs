using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.Societies.ForTesting {

    public class MockResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance fields and properties

        #region from ResourceBlobFactoryBase

        public override ReadOnlyCollection<ResourceBlobBase> Blobs {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from ResourceBlobFactoryBase

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfResource;
            newBlob.transform.position = startingXYCoordinates;
            return newBlob;
        }

        public override void DestroyBlob(ResourceBlobBase blob) {
            GameObject.DestroyImmediate(blob.gameObject);
        }

        public override void UnsubscribeBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override void TickAllBlobs(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
