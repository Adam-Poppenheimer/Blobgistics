using System;

using UnityEngine;

namespace Assets.Blobs {

    public abstract class ResourceBlobFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract ResourceBlob BuildBlob(ResourceType typeOfResource);
        public abstract ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates);

        public abstract void DestroyBlob(ResourceBlob blob);

        #endregion

    }

}