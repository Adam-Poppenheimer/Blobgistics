using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Highways.Editor {

    internal class MockBlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        private BlobTubePrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    var hostingObject = new GameObject();
                    _privateData = hostingObject.AddComponent<MockBlobTubePrivateData>();
                    _privateData.Capacity = 10;
                    _privateData.TransportSpeedPerSecond = 1f;
                }
                return _privateData;
            }
        }
        private BlobTubePrivateDataBase _privateData;

        #endregion

        #region instance methods

        #region from IBlobTubeFactory

        public override BlobTubeBase ConstructTube(Vector3 pullLocation, Vector3 pushLocation) {
            var hostingObject = new GameObject();
            var newTube = hostingObject.AddComponent<BlobTube>();
            newTube.PrivateData = PrivateData;
            return newTube;
        }

        public override void DestroyTube(BlobTubeBase tube) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}