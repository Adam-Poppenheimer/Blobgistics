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
            get { return new List<ResourceBlob>(BlobQueue).AsReadOnly(); }
        }
        private Queue<ResourceBlob> BlobQueue = new Queue<ResourceBlob>();

        public Vector3 SourceLocation {
            get { return _locationOfSourceToPullFrom; }
            private set { _locationOfSourceToPullFrom = value; }
        }
        [SerializeField] private Vector3 _locationOfSourceToPullFrom;

        public Vector3 TargetLocation {
            get { return _locationOfTargetToPushTo; }
            private set { _locationOfTargetToPushTo = value; }
        }
        [SerializeField] private Vector3 _locationOfTargetToPushTo;

        #endregion

        public IBlobTubePrivateData PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                }
            }
        }
        private IBlobTubePrivateData _privateData;

        private Dictionary<ResourceType, bool> PermissionsForBlobTypes = 
            new Dictionary<ResourceType, bool>();

        private Vector3 DirectionOfTubeMovement = Vector3.zero;

        #endregion

        #region instance methods

        #region from IBlobTube

        public bool CanPullBlobFrom(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(!BlobQueue.Contains(blob)) {
                throw new BlobTubeException("Cannot pull a blob from a tube it's not in");
            }
            return blob.transform.position == TargetLocation && BlobQueue.Peek() == blob;
        }

        public void PullBlobFrom(ResourceBlob blob) {
            if(CanPullBlobFrom(blob)) {
                BlobQueue.Dequeue();
            }else {
                throw new BlobTubeException("Cannot pull this blob from this tube");
            }
        }

        public bool CanPushBlobInto(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            bool blobTypeIsPermitted = false;
            PermissionsForBlobTypes.TryGetValue(blob.BlobType, out blobTypeIsPermitted);
            return blobTypeIsPermitted && BlobQueue.Count < PrivateData.Capacity && !BlobQueue.Contains(blob);
        }

        public void PushBlobInto(ResourceBlob blob) {
            if(CanPushBlobInto(blob)) {
                BlobQueue.Enqueue(blob);
                blob.transform.position = SourceLocation;
            }else {
                throw new BlobTubeException("Cannot push this blob into this BlobTube");
            }
        }

        public void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation) {
            SourceLocation = sourceLocation;
            TargetLocation = targetLocation;
            DirectionOfTubeMovement = (TargetLocation - SourceLocation).normalized;
        }

        public void TickMovement(float secondsPassed) {
            foreach(var blobWithin in BlobQueue) {
                var distanceToMove = Mathf.Min(secondsPassed * PrivateData.TransportSpeedPerSecond,
                    Vector3.Distance(blobWithin.transform.position, TargetLocation));
                blobWithin.transform.Translate(DirectionOfTubeMovement * distanceToMove);
            }
        }

        public void SetPermissionForResourceType(ResourceType type, bool isPermitted) {
            PermissionsForBlobTypes[type] = isPermitted;
        }

        #endregion

        #endregion

    }

}
