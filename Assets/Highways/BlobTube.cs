using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.Meshes;

namespace Assets.Highways {

    public class BlobTube : BlobTubeBase {

        #region instance fields and properties

        #region from BlobTubeBase

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

        private List<ResourceBlobBase> BlobsAtEnd = new List<ResourceBlobBase>();

        public override Vector3 SourceLocation {
            get { return sourceLocation; }
        }
        [SerializeField] private Vector3 sourceLocation;

        public override Vector3 TargetLocation {
            get { return targetLocation; }
        }
        [SerializeField] private Vector3 targetLocation;

        [SerializeField] public override int Capacity { get; set; }

        public override int SpaceLeft {
            get { return Capacity - contents.Count; }
        }

        [SerializeField] public override float TransportSpeedPerSecond { get; set; }

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
        [SerializeField] private BlobTubePrivateDataBase _privateData;

        public BoolPerResourceDictionary PermissionsForBlobTypes {
            get { return _permissionsForBlobTypes; }
            set { _permissionsForBlobTypes = value; }
        }
        [SerializeField] private BoolPerResourceDictionary _permissionsForBlobTypes;

        private float distanceBetweenEndpoints = 0f;

        #endregion

        #region instance methods

        #region from BlobTubeBase

        public override bool CanPullBlobFrom(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(!contents.Contains(blob)) {
                throw new BlobTubeException("Cannot pull a blob from a tube it's not in");
            }
            return BlobsAtEnd.Contains(blob) && contents.Contains(blob);
        }

        public override void PullBlobFrom(ResourceBlobBase blob) {
            if(CanPullBlobFrom(blob)) {
                contents.Remove(blob);
                BlobsAtEnd.Remove(blob);
            }else {
                throw new BlobTubeException("Cannot pull this blob from this tube");
            }
        }

        public override bool CanPushBlobInto(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            bool blobTypeIsPermitted = false;
            PermissionsForBlobTypes.TryGetValue(blob.BlobType, out blobTypeIsPermitted);
            return blobTypeIsPermitted && contents.Count < Capacity && !contents.Contains(blob);
        }

        public override void PushBlobInto(ResourceBlobBase blob) {
            if(CanPushBlobInto(blob)) {
                contents.Add(blob);

                blob.BeingDestroyed += Blob_OnBeingDestroyed;
                blob.transform.SetParent(transform, true);
                blob.transform.rotation = Quaternion.identity;

                var zOffsetSource = new Vector3(SourceLocation.x, SourceLocation.y, ResourceBlob.DesiredZPositionOfAllBlobs);
                var zOffsetTarget = new Vector3(TargetLocation.x, TargetLocation.y, ResourceBlob.DesiredZPositionOfAllBlobs);

                blob.ClearAllMovementGoals();
                blob.PushNewMovementGoal(new MovementGoal(zOffsetSource, TransportSpeedPerSecond));
                blob.PushNewMovementGoal(new MovementGoal(zOffsetTarget, TransportSpeedPerSecond, delegate() {
                    BlobsAtEnd.Add(blob);
                    RaiseBlobReachedEndOfTube(blob);
                }));
            }else {
                throw new BlobTubeException("Cannot push this blob into this BlobTube");
            }
        }

        public override bool RemoveBlobFrom(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            BlobsAtEnd.Remove(blob);

            bool retval = contents.Remove(blob);

            if(retval) {
                PrivateData.BlobFactory.DestroyBlob(blob);
            }
            return retval;
        }

        public override void Clear() {
            var blobsToRemove = new List<ResourceBlobBase>(contents);
            for(int i = blobsToRemove.Count - 1; i >= 0; --i) {
                RemoveBlobFrom(blobsToRemove[i]);
            }
            contents.Clear();
        }

        public override void SetEndpoints(Vector3 newSourceLocation, Vector3 newTargetLocation) {
            sourceLocation = newSourceLocation;
            targetLocation = newTargetLocation;

            distanceBetweenEndpoints = Vector3.Distance(sourceLocation, targetLocation);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if(spriteRenderer != null) {
                spriteRenderer.size = new Vector2(distanceBetweenEndpoints, PrivateData.TubeWidth);
            }
        }

        public override bool GetPermissionForResourceType(ResourceType type) {
            bool retval;
            PermissionsForBlobTypes.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetPermissionForResourceType(ResourceType type, bool isPermitted) {
            PermissionsForBlobTypes[type] = isPermitted;
        }

        #endregion

        private void Blob_OnBeingDestroyed(object sender, EventArgs e) {
            var blob = sender as ResourceBlobBase;
            BlobsAtEnd.Remove(blob);
            contents.Remove(blob);
        }

        #endregion

    }

}
