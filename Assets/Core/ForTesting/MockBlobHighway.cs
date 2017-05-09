using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Highways;
using Assets.Map;

namespace Assets.Core.ForTesting {

    public class MockBlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

        public override float BlobPullCooldownInSeconds {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override float Efficiency { get; set; }

        public override MapNodeBase FirstEndpoint {
            get { return firstEndpoint; }
        }
        public MapNodeBase firstEndpoint;

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override int Priority { get; set; }

        public override BlobHighwayProfileBase Profile {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase SecondEndpoint {
            get { return secondEndpoint; }
        }
        public MapNodeBase secondEndpoint;

        #endregion

        private Dictionary<ResourceType, bool> FirstEndpointPermissions = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> SecondEndpointPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> UpkeepsRequested =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region from BlobHighwayBase

        public override bool CanPullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override bool CanPullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool GetPullingPermissionForFirstEndpoint(ResourceType type) {
            bool retval;
            FirstEndpointPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override bool GetPullingPermissionForSecondEndpoint(ResourceType type) {
            bool retval;
            SecondEndpointPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override void PullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override void PullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted) {
            FirstEndpointPermissions[type] = isPermitted;
        }

        public override void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted) {
            SecondEndpointPermissions[type] = isPermitted;
        }

        public override bool GetUpkeepRequestedForResource(ResourceType type) {
            bool retval;
            UpkeepsRequested.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested) {
            UpkeepsRequested[type] = isBeingRequested;
        }

        #endregion

        #endregion

    }

}
