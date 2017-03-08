using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.Societies.ForTesting {

    public class MockResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance methods

        #region from ResourceBlobFactoryBase

        public override ResourceBlob BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public override ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfResource;
            newBlob.transform.position = startingXYCoordinates;
            return newBlob;
        }

        public override void DestroyBlob(ResourceBlob blob) {
            GameObject.DestroyImmediate(blob);
        }

        #endregion

        #endregion
        
    }

}
