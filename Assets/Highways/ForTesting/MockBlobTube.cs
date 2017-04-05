using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways.ForTesting {

    public class MockBlobTube : BlobTubeBase {

        #region instance fields and properties

        #region from BlobTubeBase

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

        public override Vector3 SourceLocation {
            get { return sourceLocation; }
        }
        private Vector3 sourceLocation;

        public override Vector3 TargetLocation {
            get { return targetLocation; }
        }
        private Vector3 targetLocation;

        public override int Capacity { get; set; }

        public override float TransportSpeedPerSecond { get; set; }

        public override int SpaceLeft {
            get { return Capacity - contents.Count; }
        }

        #endregion

        private Dictionary<ResourceType, bool> Permissions = 
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region from BlobTubeBase

        public override bool CanPullBlobFrom(ResourceBlobBase blob) {
            return false;
        }

        public override bool CanPushBlobInto(ResourceBlobBase blob) {
            return GetPermissionForResourceType(blob.BlobType);
        }

        public override void PullBlobFrom(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override void PushBlobInto(ResourceBlobBase blob) {
            contents.Add(blob);
        }

        public override bool RemoveBlobFrom(ResourceBlobBase blob) {
            return contents.Remove(blob);
        }

        public override void Clear() {
            contents.Clear();
        }

        public override void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation) {
            this.sourceLocation = sourceLocation;
            this.targetLocation = targetLocation;
        }

        public override bool GetPermissionForResourceType(ResourceType type) {
            bool retval;
            Permissions.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetPermissionForResourceType(ResourceType type, bool isPermitted) {
            Permissions[type] = isPermitted;
        }

        #endregion

        #endregion

    }

}
