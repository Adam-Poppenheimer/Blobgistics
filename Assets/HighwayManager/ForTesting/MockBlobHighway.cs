using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Core;
using Assets.Highways;
using Assets.Map;
using UnityEngine;

namespace Assets.HighwayManager.ForTesting {

    public class MockBlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

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

        public override MapNodeBase FirstEndpoint {
            get { return firstEndpoint; }
        }
        private MapNodeBase firstEndpoint;

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override int Priority { get; set; }

        public override float BlobPullCooldownInSeconds {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayProfile Profile {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase SecondEndpoint {
            get { return secondEndpoint; }
        }
        private MapNodeBase secondEndpoint;

        public override float Efficiency { get; set; }

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayFactoryBase ParentFactory {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override UIControlBase UIControl {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        #endregion

        private Dictionary<ResourceType, bool> UpkeepRequestedForResource =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region from BlobHighwayBase

        public override void SetEndpoints(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            this.firstEndpoint = firstEndpoint;
            this.secondEndpoint = secondEndpoint;
        }

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
            throw new NotImplementedException();
        }

        public override bool GetPullingPermissionForSecondEndpoint(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void PullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override void PullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override bool GetUpkeepRequestedForResource(ResourceType type) {
            bool retval;
            UpkeepRequestedForResource.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested) {
            UpkeepRequestedForResource[type] = isBeingRequested;
        }

        #endregion

        #endregion

    }

}
