using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.BlobSites;

namespace Assets.Highways {

    public class BlobHighway : BlobHighwayBase, IPointerClickHandler {

        #region instance fields and properties

        #region from IBlobHighway

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteBase FirstEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteBase SecondEndpoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayProfile Profile { get; set; }

        public override int Priority { get; set; }

        #endregion

        public BlobHighwayPrivateData PrivateData {
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
        private BlobHighwayPrivateData _privateData;

        #endregion

        #region instance methods

        #region EventSystem pointer event implementations

        public void OnPointerClick(PointerEventData eventData) {
            PrivateData.UIControl.PushHighwayClickEvent(eventData, this);
        }

        #endregion

        #region from IBlobHighway

        public override bool CanPullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override bool CanPullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override bool GetPermissionForEndpoint1(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPermissionForEndpoint1(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override bool GetPermissionForEndpoint2(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPermissionForEndpoint2(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void PullFromFirstEndpoint() {
            throw new NotImplementedException();
        }

        public override void PullFromSecondEndpoint() {
            throw new NotImplementedException();
        }

        public override void TickMovement(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
