using System;
using System.Collections.ObjectModel;

using UnityEngine;

namespace Assets.Blobs {

    public abstract class ResourceBlobFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<ResourceBlobBase> Blobs { get; }

        #endregion

        #region instance methods

        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource);
        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates);

        public abstract void DestroyBlob(ResourceBlobBase blob);
        public abstract void UnsubscribeBlob(ResourceBlobBase blob);

        public abstract void TickAllBlobs(float secondsPassed);

        #endregion

    }

}