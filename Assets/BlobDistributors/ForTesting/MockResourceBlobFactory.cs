using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.BlobDistributors.ForTesting {

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
            throw new NotImplementedException();
        }

        public override ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingXYCoordinates) {
            throw new NotImplementedException();
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
