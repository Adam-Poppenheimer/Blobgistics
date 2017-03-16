using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways {

    public class BlobHighway : BlobHighwayBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler {

        #region instance fields and properties

        #region from BlobHighwayBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override int Priority {
            get { return _priority; }
            set { _priority = value; }
        }
        [SerializeField] private int _priority = Int32.MaxValue;

        public override BlobHighwayProfile Profile {
            get { return _profile; }
            set {
                _profile = value;
                UpdateTubesFromProfile();
            }
        }
        [SerializeField, HideInInspector] private BlobHighwayProfile _profile;

        public override MapNodeBase FirstEndpoint {
            get { return PrivateData.FirstEndpoint; }
        }

        public override MapNodeBase SecondEndpoint {
            get { return PrivateData.SecondEndpoint; }
        }

        #endregion

        public BlobHighwayPrivateDataBase PrivateData {
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
                    UpdateTubeEndpoints();
                }
            }
        }
        [SerializeField, HideInInspector] private BlobHighwayPrivateDataBase _privateData;

        #endregion

        #region instance methods

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushBeginDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushEndDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerClickEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerEnterEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerExitEvent(new BlobHighwayUISummary(this), eventData);
        }

        #endregion

        #region from BlobHighwayBase

        public override bool CanPullFromFirstEndpoint() {
            if(PrivateData.TubePullingFromFirstEndpoint.SpaceLeft > 0) {
                foreach(var resourceType in GetCommonResourceTypes(FirstEndpoint.BlobSite, SecondEndpoint.BlobSite)) {
                    if(GetPullingPermissionForFirstEndpoint(resourceType)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PullFromFirstEndpoint() {
            if(CanPullFromFirstEndpoint()) {
                foreach(var resourceType in GetCommonResourceTypes(FirstEndpoint.BlobSite, SecondEndpoint.BlobSite)) {
                    if(GetPullingPermissionForFirstEndpoint(resourceType)) {
                        var pulledBlob = FirstEndpoint.BlobSite.ExtractBlobOfType(resourceType);
                        PrivateData.TubePullingFromFirstEndpoint.PushBlobInto(pulledBlob);
                        return;
                    }
                }
            }else {
                throw new BlobHighwayException("Cannot pull from this BlobHighway's FirstEndpoint");
            }
        }

        public override bool CanPullFromSecondEndpoint() {
            if(PrivateData.TubePullingFromSecondEndpoint.SpaceLeft > 0) {
                foreach(var resourceType in GetCommonResourceTypes(SecondEndpoint.BlobSite, FirstEndpoint.BlobSite)) {
                    if(GetPullingPermissionForSecondEndpoint(resourceType)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PullFromSecondEndpoint() {
            if(CanPullFromSecondEndpoint()) {
                foreach(var resourceType in GetCommonResourceTypes(SecondEndpoint.BlobSite, FirstEndpoint.BlobSite)) {
                    if(GetPullingPermissionForSecondEndpoint(resourceType)) {
                        var pulledBlob = SecondEndpoint.BlobSite.ExtractBlobOfType(resourceType);
                        PrivateData.TubePullingFromSecondEndpoint.PushBlobInto(pulledBlob);
                        return;
                    }
                }
            }else {
                throw new BlobHighwayException("Cannot pull from this BlobHighway's SecondEndpoint");
            }
        }

        public override bool GetPullingPermissionForFirstEndpoint(ResourceType type) {
            return PrivateData.TubePullingFromFirstEndpoint.GetPermissionForResourceType(type);
        }

        public override void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted) {
            PrivateData.TubePullingFromFirstEndpoint.SetPermissionForResourceType(type, isPermitted);
        }

        public override bool GetPullingPermissionForSecondEndpoint(ResourceType type) {
            return PrivateData.TubePullingFromSecondEndpoint.GetPermissionForResourceType(type);
        }

        public override void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted) {
            PrivateData.TubePullingFromSecondEndpoint.SetPermissionForResourceType(type, isPermitted);
        }

        public override void TickMovement(float secondsPassed) {
            PrivateData.TubePullingFromFirstEndpoint.TickMovement(secondsPassed);
            PrivateData.TubePullingFromSecondEndpoint.TickMovement(secondsPassed);
        }

        public override void Clear() {
            PrivateData.TubePullingFromFirstEndpoint.Clear();
            PrivateData.TubePullingFromSecondEndpoint.Clear();
        }

        #endregion

        private List<ResourceType> GetCommonResourceTypes(BlobSiteBase pullSite, BlobSiteBase pushSite) {
            var retval = new List<ResourceType>();
            foreach(var resourceType in pullSite.GetExtractableTypes()) {
                if(pushSite.CanPlaceBlobOfTypeInto(resourceType)) {
                    retval.Add(resourceType);
                }
            }
            return retval;
        }

        private void UpdateTubesFromProfile() {
            PrivateData.TubePullingFromFirstEndpoint.TransportSpeedPerSecond = _profile.BlobSpeedPerSecond;
            PrivateData.TubePullingFromFirstEndpoint.Capacity = _profile.Capacity;

            PrivateData.TubePullingFromSecondEndpoint.TransportSpeedPerSecond = _profile.BlobSpeedPerSecond;
            PrivateData.TubePullingFromSecondEndpoint.Capacity = _profile.Capacity;
        }

        private void UpdateTubeEndpoints() {
            var directionFromFirstToSecond = FirstEndpoint.transform.GetDominantManhattanDirectionTo(SecondEndpoint.transform);
            var directionFromSecondToFirst = SecondEndpoint.transform.GetDominantManhattanDirectionTo(FirstEndpoint.transform);

            var firstConnectionPoint = FirstEndpoint.BlobSite.GetConnectionPointInDirection(directionFromFirstToSecond);
            var secondConnectionPoint = SecondEndpoint.BlobSite.GetConnectionPointInDirection(directionFromSecondToFirst);

            PrivateData.TubePullingFromFirstEndpoint.SetEndpoints(firstConnectionPoint, secondConnectionPoint);
            PrivateData.TubePullingFromSecondEndpoint.SetEndpoints(secondConnectionPoint, firstConnectionPoint);
        }

        #endregion

    }

}
