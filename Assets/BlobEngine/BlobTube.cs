using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BlobTube : MonoBehaviour, IBlobTube {

        #region instance fields and properties

        #region from IBlobTube

        public ReadOnlyCollection<ResourceBlob> BlobsWithin {
            get {
                throw new NotImplementedException();
            }
        }

        public Vector3 LocationOfSourceToPullFrom {
            get {
                throw new NotImplementedException();
            }
        }

        public Vector3 LocationOfTargetToPushTo {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from IBlobTube

        public bool CanPullBlobFrom(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public bool CanPushBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public void PullBlobFrom(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public void PushBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation) {
            throw new NotImplementedException();
        }

        public void TickMovement() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
