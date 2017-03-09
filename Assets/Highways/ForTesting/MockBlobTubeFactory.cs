using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Highways.ForTesting {

    public class MockBlobTubeFactory : BlobTubeFactoryBase {

        #region instance methods

        #region from BlobTubeFactoryBase

        public override BlobTubeBase ConstructTube(Vector3 pullLocation, Vector3 pushLocation) {
            var hostingObject = new GameObject();
            var newTube = hostingObject.AddComponent<MockBlobTube>();
            return newTube;
        }

        public override void DestroyTube(BlobTubeBase tube) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}