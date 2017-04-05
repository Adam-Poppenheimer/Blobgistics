using System;

using UnityEngine;

namespace Assets.Blobs {

    public abstract class ResourceBlobFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource);
        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates);

        public abstract void DestroyBlob(ResourceBlobBase blob);

        public abstract void TickAllBlobs();

        #endregion

    }

}