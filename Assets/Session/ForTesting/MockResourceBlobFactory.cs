using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.Session.ForTesting {

    public class MockResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance methods

        #region from ResourceBlobFactoryBase

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var newBlob = (new GameObject()).AddComponent<MockResourceBlob>();
            newBlob.BlobType = typeOfResource;
            return newBlob;
        }

        public override void DestroyBlob(ResourceBlobBase blob) {
            DestroyImmediate(blob.gameObject);
        }

        public override void TickAllBlobs(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
