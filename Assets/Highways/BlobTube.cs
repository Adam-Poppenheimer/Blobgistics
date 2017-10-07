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

    /// <summary>
    /// The standard implementation of BlobTubeBase. This class handles the transit of resources
    /// from one point to another.
    /// </summary>
    public class BlobTube : BlobTubeBase {

        #region instance fields and properties

        #region from BlobTubeBase

        /// <inheritdoc/>
        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

        private List<ResourceBlobBase> BlobsAtEnd = new List<ResourceBlobBase>();

        /// <inheritdoc/>
        public override Vector3 SourceLocation {
            get { return sourceLocation; }
        }
        [SerializeField] private Vector3 sourceLocation;

        /// <inheritdoc/>
        public override Vector3 TargetLocation {
            get { return targetLocation; }
        }
        [SerializeField] private Vector3 targetLocation;

        /// <inheritdoc/>
        [SerializeField] public override int Capacity { get; set; }

        /// <inheritdoc/>
        public override int SpaceLeft {
            get { return Capacity - contents.Count; }
        }

        /// <inheritdoc/>
        [SerializeField] public override float TransportSpeedPerSecond { get; set; }

        #endregion

        /// <summary>
        /// Configuration data for the tube.
        /// </summary>
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

        /// <summary>
        /// The pulling permissions of the various resource types.
        /// </summary>
        /// <remarks>
        /// Permissions must be stored as a BoolPerResourceDictionary because this information
        /// needs to persist between runtimes. Highway permissions are an important tool for
        /// designing maps, especially tutorial maps. It could be argued that this serialization
        /// is redundant with the classes in the Session namespace that require it, making it a
        /// possible refactoring candidate.
        /// </remarks>
        public BoolPerResourceDictionary PermissionsForBlobTypes {
            get { return _permissionsForBlobTypes; }
            set { _permissionsForBlobTypes = value; }
        }
        [SerializeField] private BoolPerResourceDictionary _permissionsForBlobTypes;

        private float distanceBetweenEndpoints = 0f;

        #endregion

        #region instance methods

        #region from BlobTubeBase

        /// <inheritdoc/>
        public override bool CanPullBlobFrom(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(!contents.Contains(blob)) {
                throw new BlobTubeException("Cannot pull a blob from a tube it's not in");
            }
            return BlobsAtEnd.Contains(blob) && contents.Contains(blob);
        }

        /// <inheritdoc/>
        public override void PullBlobFrom(ResourceBlobBase blob) {
            if(CanPullBlobFrom(blob)) {
                contents.Remove(blob);
                BlobsAtEnd.Remove(blob);
            }else {
                throw new BlobTubeException("Cannot pull this blob from this tube");
            }
        }

        /// <inheritdoc/>
        public override bool CanPushBlobInto(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            bool blobTypeIsPermitted = false;
            PermissionsForBlobTypes.TryGetValue(blob.BlobType, out blobTypeIsPermitted);
            return blobTypeIsPermitted && contents.Count < Capacity && !contents.Contains(blob);
        }

        /// <inheritdoc/>
        public override void PushBlobInto(ResourceBlobBase blob) {
            if(CanPushBlobInto(blob)) {
                contents.Add(blob);

                blob.BeingDestroyed += Blob_OnBeingDestroyed;
                blob.transform.SetParent(transform, true);
                blob.transform.rotation = Quaternion.identity;

                //The Z position of the blobs should not change, since the game is 2D.
                var zOffsetSource = new Vector3(SourceLocation.x, SourceLocation.y, ResourceBlob.DesiredZPositionOfAllBlobs);
                var zOffsetTarget = new Vector3(TargetLocation.x, TargetLocation.y, ResourceBlob.DesiredZPositionOfAllBlobs);

                blob.ClearAllMovementGoals();
                blob.EnqueueNewMovementGoal(new MovementGoal(zOffsetSource, TransportSpeedPerSecond));
                blob.EnqueueNewMovementGoal(new MovementGoal(zOffsetTarget, TransportSpeedPerSecond, delegate() {
                    BlobsAtEnd.Add(blob);
                    RaiseBlobReachedEndOfTube(blob);
                }));
            }else {
                throw new BlobTubeException("Cannot push this blob into this BlobTube");
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void Clear() {
            var blobsToRemove = new List<ResourceBlobBase>(contents);
            for(int i = blobsToRemove.Count - 1; i >= 0; --i) {
                RemoveBlobFrom(blobsToRemove[i]);
            }
            contents.Clear();
        }

        /// <inheritdoc/>
        public override void SetEndpoints(Vector3 newSourceLocation, Vector3 newTargetLocation) {
            sourceLocation = newSourceLocation;
            targetLocation = newTargetLocation;

            distanceBetweenEndpoints = Vector3.Distance(sourceLocation, targetLocation);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if(spriteRenderer != null) {
                spriteRenderer.size = new Vector2(distanceBetweenEndpoints, PrivateData.TubeWidth);
            }
        }

        /// <inheritdoc/>
        public override bool GetPermissionForResourceType(ResourceType type) {
            bool retval;
            PermissionsForBlobTypes.TryGetValue(type, out retval);
            return retval;
        }

        /// <inheritdoc/>
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
