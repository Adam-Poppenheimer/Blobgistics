using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;
using UnityEngine;

namespace Assets.Societies.Editor {

    internal class MockResourceBlobFactory : IResourceBlobFactory {

        #region instance methods

        #region from IResourceBlobFactory

        public ResourceBlob BuildBlob(ResourceType typeOfResource) {
            return BuildBlob(typeOfResource, Vector2.zero);
        }

        public ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfResource;
            newBlob.transform.position = startingXYCoordinates;
            return newBlob;
        }

        public void DestroyBlob(ResourceBlob blob) {
            GameObject.DestroyImmediate(blob);
        }

        #endregion

        #endregion
        
    }

}
