using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    public class BlobTube : BlobTubeBase {

        #region instance fields and properties

        #region from IBlobTube

        public override ReadOnlyCollection<ResourceBlob> BlobsWithin {
            get { return new List<ResourceBlob>(BlobQueue).AsReadOnly(); }
        }
        private Queue<ResourceBlob> BlobQueue = new Queue<ResourceBlob>();

        public override Vector3 SourceLocation {
            get { return sourceLocation; }
        }
        [SerializeField] private Vector3 sourceLocation;

        public override Vector3 TargetLocation {
            get { return targetLocation; }
        }
        [SerializeField] private Vector3 targetLocation;

        #endregion

        public BlobTubePrivateDataBase PrivateData {
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
        private BlobTubePrivateDataBase _privateData;

        private Dictionary<ResourceType, bool> PermissionsForBlobTypes = 
            new Dictionary<ResourceType, bool>();

        private Vector3 DirectionOfTubeMovement = Vector3.zero;

        #endregion

        #region instance methods

        #region from IBlobTube

        public override bool CanPullBlobFrom(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(!BlobQueue.Contains(blob)) {
                throw new BlobTubeException("Cannot pull a blob from a tube it's not in");
            }
            return blob.transform.position == TargetLocation && BlobQueue.Peek() == blob;
        }

        public override void PullBlobFrom(ResourceBlob blob) {
            if(CanPullBlobFrom(blob)) {
                BlobQueue.Dequeue();
            }else {
                throw new BlobTubeException("Cannot pull this blob from this tube");
            }
        }

        public override bool CanPushBlobInto(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            bool blobTypeIsPermitted = false;
            PermissionsForBlobTypes.TryGetValue(blob.BlobType, out blobTypeIsPermitted);
            return blobTypeIsPermitted && BlobQueue.Count < PrivateData.Capacity && !BlobQueue.Contains(blob);
        }

        public override void PushBlobInto(ResourceBlob blob) {
            if(CanPushBlobInto(blob)) {
                BlobQueue.Enqueue(blob);
                blob.transform.position = SourceLocation;
            }else {
                throw new BlobTubeException("Cannot push this blob into this BlobTube");
            }
        }

        public override void SetEndpoints(Vector3 newSourceLocation, Vector3 newTargetLocation) {
            sourceLocation = newSourceLocation;
            targetLocation = newTargetLocation;
            DirectionOfTubeMovement = (TargetLocation - SourceLocation).normalized;
        }

        public override void TickMovement(float secondsPassed) {
            foreach(var blobWithin in BlobQueue) {
                var distanceToMove = Mathf.Min(secondsPassed * PrivateData.TransportSpeedPerSecond,
                    Vector3.Distance(blobWithin.transform.position, TargetLocation));
                blobWithin.transform.Translate(DirectionOfTubeMovement * distanceToMove);
            }
        }

        public override void SetPermissionForResourceType(ResourceType type, bool isPermitted) {
            PermissionsForBlobTypes[type] = isPermitted;
        }

        #endregion

        #endregion

    }

}
