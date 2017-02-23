using System;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IResourceBlobFactory {

        #region methods

        ResourceBlob BuildBlob(ResourceType typeOfResource);
        ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates);

        void DestroyBlob(ResourceBlob blob);

        #endregion

    }

}