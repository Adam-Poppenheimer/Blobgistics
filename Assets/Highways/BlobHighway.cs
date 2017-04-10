using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;
using System.Collections.ObjectModel;

namespace Assets.Highways {

    public class BlobHighway : BlobHighwayBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region static fields and properties

        public static readonly float HighwayCollisionWidth = 0.6f;
        public static readonly float TubeCenterOffset = 0.2f;

        #endregion

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

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint {
            get { return PrivateData.TubePullingFromFirstEndpoint.Contents; }
        }

        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint {
            get { return PrivateData.TubePullingFromSecondEndpoint.Contents; }
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
                    _privateData.TubePullingFromFirstEndpoint.BlobReachedEndOfTube += TubePullingFromFirstEndpoint_BlobReachedEndOfTube;
                    _privateData.TubePullingFromSecondEndpoint.BlobReachedEndOfTube += TubePullingFromSecondEndpoint_BlobReachedEndOfTube;
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
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerEnterEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerExitEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            PrivateData.UIControl.PushSelectEvent(new BlobHighwayUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            PrivateData.UIControl.PushDeselectEvent(new BlobHighwayUISummary(this), eventData);
        }

        #endregion

        #region from BlobHighwayBase

        public override bool CanPullFromFirstEndpoint() {
            if(PrivateData.TubePullingFromFirstEndpoint.SpaceLeft > 0) {
                foreach(var blob in FirstEndpoint.BlobSite.Contents) {
                    if(
                        FirstEndpoint.BlobSite.CanExtractBlob(blob) &&
                        SecondEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        PrivateData.TubePullingFromFirstEndpoint.CanPushBlobInto(blob)
                    ){
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PullFromFirstEndpoint() {
            if(PrivateData.TubePullingFromFirstEndpoint.SpaceLeft > 0) {
                foreach(var blob in FirstEndpoint.BlobSite.Contents) {
                    if(
                        FirstEndpoint.BlobSite.CanExtractBlob(blob) &&
                        SecondEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        PrivateData.TubePullingFromFirstEndpoint.CanPushBlobInto(blob)
                    ){
                        FirstEndpoint.BlobSite.ExtractBlob(blob);
                        PrivateData.TubePullingFromFirstEndpoint.PushBlobInto(blob);
                        return;
                    }
                }
            }
            throw new BlobHighwayException("Cannot pull from this BlobHighway's FirstEndpoint");
        }

        public override bool CanPullFromSecondEndpoint() {
            if(PrivateData.TubePullingFromSecondEndpoint.SpaceLeft > 0) {
                foreach(var blob in SecondEndpoint.BlobSite.Contents) {
                    if(
                        SecondEndpoint.BlobSite.CanExtractBlob(blob) &&
                        FirstEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        PrivateData.TubePullingFromSecondEndpoint.CanPushBlobInto(blob)
                    ){
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PullFromSecondEndpoint() {
            if(PrivateData.TubePullingFromSecondEndpoint.SpaceLeft > 0) {
                foreach(var blob in SecondEndpoint.BlobSite.Contents) {
                    if(
                        SecondEndpoint.BlobSite.CanExtractBlob(blob) &&
                        FirstEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        PrivateData.TubePullingFromSecondEndpoint.CanPushBlobInto(blob)
                    ){
                        SecondEndpoint.BlobSite.ExtractBlob(blob);
                        PrivateData.TubePullingFromSecondEndpoint.PushBlobInto(blob);
                        return;
                    }
                }
            }
            throw new BlobHighwayException("Cannot pull from this BlobHighway's FirstEndpoint");
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

        public override void Clear() {
            PrivateData.TubePullingFromFirstEndpoint.Clear();
            PrivateData.TubePullingFromSecondEndpoint.Clear();
        }

        public override void GetEndpointPositions(out Vector3 firstEndpointPosition, out Vector3 secondEndpointPosition) {
            var directionFromFirstToSecond = FirstEndpoint.transform.GetDominantManhattanDirectionTo(SecondEndpoint.transform);
            var directionFromSecondToFirst = SecondEndpoint.transform.GetDominantManhattanDirectionTo(FirstEndpoint.transform);

            firstEndpointPosition = FirstEndpoint.BlobSite.GetConnectionPointInDirection(directionFromFirstToSecond);
            secondEndpointPosition = SecondEndpoint.BlobSite.GetConnectionPointInDirection(directionFromSecondToFirst);
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
            PrivateData.TubePullingFromFirstEndpoint.transform.SetParent(transform, false);
            PrivateData.TubePullingFromSecondEndpoint.transform.SetParent(transform, false);

            PrivateData.TubePullingFromFirstEndpoint.transform.Translate(Vector3.up * TubeCenterOffset);
            PrivateData.TubePullingFromSecondEndpoint.transform.Translate(Vector3.down * TubeCenterOffset);

            Vector3 firstConnectionPoint, secondConnectionPoint;
            GetEndpointPositions(out firstConnectionPoint, out secondConnectionPoint);

            EdgeOrientationUtil.AlignTransformWithEndpoints(transform, firstConnectionPoint, secondConnectionPoint, false);

            var boxCollider = GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(Vector3.Distance(firstConnectionPoint, secondConnectionPoint), boxCollider.size.y);
            }

            PrivateData.TubePullingFromFirstEndpoint.SetEndpoints(
                firstConnectionPoint + PrivateData.TubePullingFromFirstEndpoint.transform.TransformDirection(Vector3.up * TubeCenterOffset),
                secondConnectionPoint + PrivateData.TubePullingFromFirstEndpoint.transform.TransformDirection(Vector3.up * TubeCenterOffset)
            );
            PrivateData.TubePullingFromSecondEndpoint.SetEndpoints(
                secondConnectionPoint + PrivateData.TubePullingFromSecondEndpoint.transform.TransformDirection(Vector3.down * TubeCenterOffset),
                firstConnectionPoint + PrivateData.TubePullingFromSecondEndpoint.transform.TransformDirection(Vector3.down * TubeCenterOffset)
            );
        }

        private void TubePullingFromFirstEndpoint_BlobReachedEndOfTube(object sender, BlobEventArgs e) {
            if(PrivateData.SecondEndpoint.BlobSite.CanPlaceBlobInto(e.Blob)) {
                PrivateData.TubePullingFromFirstEndpoint.PullBlobFrom(e.Blob);
                PrivateData.SecondEndpoint.BlobSite.PlaceBlobInto(e.Blob);
            }else {
                PrivateData.BlobFactory.DestroyBlob(e.Blob);
            }
        }

        private void TubePullingFromSecondEndpoint_BlobReachedEndOfTube(object sender, BlobEventArgs e) {
            if(PrivateData.FirstEndpoint.BlobSite.CanPlaceBlobInto(e.Blob)) {
                PrivateData.TubePullingFromSecondEndpoint.PullBlobFrom(e.Blob);
                PrivateData.FirstEndpoint.BlobSite.PlaceBlobInto(e.Blob);
            }else {
                PrivateData.BlobFactory.DestroyBlob(e.Blob);
            }
        }

        #endregion

    }

}
