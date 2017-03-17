using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Highways;
using Assets.Map;

namespace Assets.HighwayUpgraders.ForTesting {

    public class MockBlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

        public override MapNodeBase FirstEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override int Priority {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayProfile Profile { get; set; }

        public override MapNodeBase SecondEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlob> ContentsPulledFromFirstEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlob> ContentsPulledFromSecondEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

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

        public override void TickMovement(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
