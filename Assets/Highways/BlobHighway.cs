using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;

using Assets.Blobs;

namespace Assets.Highways {

    public class BlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

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

        public override BlobHighwayProfile Profile {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase FirstEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase SecondEndpoint {
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

        public override void PullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override bool CanPullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override void PullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override bool GetPullingPermissionForEndpoint1(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPullingPermissionForEndpoint1(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override bool GetPullingPermissionForEndpoint2(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPullingPermissionForEndpoint2(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void TickMovement(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
