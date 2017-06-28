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
            get { return firstEndpoint; }
        }
        private MapNodeBase firstEndpoint;

        public override MapNodeBase SecondEndpoint {
            get { return secondEndpoint; }
        }
        private MapNodeBase secondEndpoint;

        public override int ID {
            get { return 42; }
        }

        public override int Priority { get; set; }

        public override BlobHighwayProfile Profile { get; set; }

        public override float Efficiency { get; set; }

        public override float BlobPullCooldownInSeconds {
            get { return Profile.BlobPullCooldownInSeconds / Efficiency; }
        }

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

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return string.Format("BlobHighway [{0} <--> {1}]", FirstEndpoint, SecondEndpoint);
        }

        #endregion

        #region from BlobHighwayBase

        public override void SetEndpoints(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            this.firstEndpoint = firstEndpoint;
            this.secondEndpoint = secondEndpoint;
        }

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

        public override bool GetUpkeepRequestedForResource(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
