using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Highways;
using Assets.Map;
using UnityEngine;

namespace Assets.BlobDistributors.ForTesting {

    public class MockBlobHighway : BlobHighwayBase {

        #region instance fields and properties

        #region from BlobHighwayBase

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint {
            get { return contentsPulledFromFirstEndpoint.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contentsPulledFromFirstEndpoint = new List<ResourceBlobBase>();

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint {
            get { return contentsPulledFromSecondEndpoint.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contentsPulledFromSecondEndpoint = new List<ResourceBlobBase>();

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

        public override BlobHighwayProfileBase Profile {
            get {
                throw new NotImplementedException();
            }
        }

        public override float Efficiency {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override float BlobPullCooldownInSeconds {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsRequestingFood   { get; set; }
        public override bool IsRequestingYellow { get; set; }
        public override bool IsRequestingWhite  { get; set; }
        public override bool IsRequestingBlue   { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return string.Format("BlobHighway [{0} <--> {1}]", FirstEndpoint, SecondEndpoint);
        }

        #endregion

        #region from BlobHighwayBase

        public override bool CanPullFromFirstEndpoint() {
            return FirstEndpoint.BlobSite.CanExtractAnyBlob() && contentsPulledFromFirstEndpoint.Count < Profile.Capacity;
        }

        public override bool CanPullFromSecondEndpoint() {
            return SecondEndpoint.BlobSite.CanExtractAnyBlob() && contentsPulledFromSecondEndpoint.Count < Profile.Capacity;
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

        #endregion

        #endregion

    }

}
