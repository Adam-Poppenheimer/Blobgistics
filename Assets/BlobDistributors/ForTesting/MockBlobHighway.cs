using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Highways;
using Assets.Map;

namespace Assets.BlobDistributors.ForTesting {

    public class MockBlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

        public override ReadOnlyCollection<ResourceBlob> ContentsPulledFromFirstEndpoint {
            get { return contentsPulledFromFirstEndpoint.AsReadOnly(); }
        }
        private List<ResourceBlob> contentsPulledFromFirstEndpoint = new List<ResourceBlob>();

        public override ReadOnlyCollection<ResourceBlob> ContentsPulledFromSecondEndpoint {
            get { return contentsPulledFromSecondEndpoint.AsReadOnly(); }
        }
        private List<ResourceBlob> contentsPulledFromSecondEndpoint = new List<ResourceBlob>();

        public override MapNodeBase FirstEndpoint {
            get { return _firstEndpoint; }
        }
        public void SetFirstEndpoint(MapNodeBase value) {
            _firstEndpoint = value;
        }
        private MapNodeBase _firstEndpoint;

        public override MapNodeBase SecondEndpoint {
            get { return _secondEndpoint; }
        }
        public void SetSecondEndpoint(MapNodeBase value) {
            _secondEndpoint = value;
        }
        private MapNodeBase _secondEndpoint;

        public override int ID {
            get { return 42; }
        }

        public override int Priority { get; set; }

        public override BlobHighwayProfile Profile { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from BlobHighwayBase

        public override bool CanPullFromFirstEndpoint() {
            return contentsPulledFromFirstEndpoint.Count < Profile.Capacity;
        }

        public override bool CanPullFromSecondEndpoint() {
            return contentsPulledFromSecondEndpoint.Count < Profile.Capacity;
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override bool GetPullingPermissionForFirstEndpoint(ResourceType type) {
            return true;
        }

        public override bool GetPullingPermissionForSecondEndpoint(ResourceType type) {
            return true;
        }

        public override void PullFromFirstEndpoint() {
            var blobPulled = FirstEndpoint.BlobSite.ExtractAnyBlob();
            contentsPulledFromFirstEndpoint.Add(blobPulled);
        }

        public override void PullFromSecondEndpoint() {
            var blobPulled = SecondEndpoint.BlobSite.ExtractAnyBlob();
            contentsPulledFromSecondEndpoint.Add(blobPulled);
        }

        public override void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted) {
            
        }

        public override void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted) {
            
        }

        public override void TickMovement(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
