using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.Highways.ForTesting {

    public class MockResourceBlobFactory : ResourceBlobFactoryBase {

        #region instance methods

        #region from ResourceBlobFactoryBase

        public override ResourceBlob BuildBlob(ResourceType typeOfResource) {
            throw new NotImplementedException();
        }

        public override ResourceBlob BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            throw new NotImplementedException();
        }

        public override void DestroyBlob(ResourceBlob blob) {
            DestroyImmediate(blob.gameObject);
        }

        #endregion

        #endregion
        
    }

}
