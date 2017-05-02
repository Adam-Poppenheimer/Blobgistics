using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockResourceBlobFactory : ResourceBlobFactoryBase {

        #region events

        public event EventHandler<FloatEventArgs> Ticked;

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
            throw new NotImplementedException();
        }

        public override void UnsubscribeBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override void TickAllBlobs(float secondsPassed) {
            if(Ticked != null) {
                Ticked(this, new FloatEventArgs(secondsPassed));
            }
        }

        #endregion

        #endregion
        
    }

}
