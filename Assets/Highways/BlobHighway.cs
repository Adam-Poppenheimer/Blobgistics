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

        public override BlobHighwayProfile Profile {
            get { return PrivateData.Profile; }
        }

        public override float Efficiency {
            get { return _efficiency; }
            set {
                _efficiency = value;
                UpdateTubeSpeedAndCapacity();
            }
        }
        private float _efficiency = 1f;

        public override float BlobPullCooldownInSeconds {
            get { return Profile.BlobPullCooldownInSeconds / Efficiency; }
        }

        #endregion

        public BlobHighwayPrivateDataBase PrivateData {
            get { return _privateData; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                    _privateData.TubePullingFromFirstEndpoint.BlobReachedEndOfTube += TubePullingFromFirstEndpoint_BlobReachedEndOfTube;
                    _privateData.TubePullingFromSecondEndpoint.BlobReachedEndOfTube += TubePullingFromSecondEndpoint_BlobReachedEndOfTube;
                    UpdateTubeSpeedAndCapacity();
                    UpdateTubeEndpoints();
                }
            }
        }
        [SerializeField, HideInInspector] private BlobHighwayPrivateDataBase _privateData;

        private Dictionary<ResourceType, bool> UpkeepRequestedForResource =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(PrivateData != null) {
                UpdateTubeSpeedAndCapacity();
            }
        }

        private void OnDestroy() {
            if(PrivateData != null && PrivateData.UIControl != null) {
                PrivateData.UIControl.PushObjectDestroyedEvent(new BlobHighwayUISummary(this));
            }
        }

        private void OnValidate() {
            if(PrivateData != null) {
                UpdateTubeSpeedAndCapacity();
            }            
        }

        #endregion

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
                    var canExtractFromFirstEndpoint = FirstEndpoint.BlobSite.CanExtractBlob(blob);
                    var canPlaceIntoSecondEndpoint = SecondEndpoint.BlobSite.CanPlaceBlobInto(blob);
                    var CanPushIntoTube = PrivateData.TubePullingFromFirstEndpoint.CanPushBlobInto(blob);
                    if(canExtractFromFirstEndpoint && canPlaceIntoSecondEndpoint && CanPushIntoTube){
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

                    var canExtractFromSecondEndpoint = SecondEndpoint.BlobSite.CanExtractBlob(blob);
                    var canPlaceIntoFirstEndpoint = FirstEndpoint.BlobSite.CanPlaceBlobInto(blob);
                    var canPushIntoTube = PrivateData.TubePullingFromSecondEndpoint.CanPushBlobInto(blob);
                    if(canExtractFromSecondEndpoint && canPlaceIntoFirstEndpoint && canPushIntoTube){
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

        public override bool GetUpkeepRequestedForResource(ResourceType type) {
            bool retval;
            UpkeepRequestedForResource.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested) {
            UpkeepRequestedForResource[type] = isBeingRequested;
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

        private void UpdateTubeSpeedAndCapacity() {
            PrivateData.TubePullingFromFirstEndpoint.TransportSpeedPerSecond = Profile.BlobSpeedPerSecond * Efficiency;
            PrivateData.TubePullingFromFirstEndpoint.Capacity = Profile.Capacity;

            PrivateData.TubePullingFromSecondEndpoint.TransportSpeedPerSecond = Profile.BlobSpeedPerSecond * Efficiency;
            PrivateData.TubePullingFromSecondEndpoint.Capacity = Profile.Capacity;
        }

        private void UpdateTubeEndpoints() {
            PrivateData.TubePullingFromFirstEndpoint.transform.SetParent(transform, false);
            PrivateData.TubePullingFromSecondEndpoint.transform.SetParent(transform, false);

            PrivateData.TubePullingFromFirstEndpoint.transform.Translate(Vector3.up * TubeCenterOffset);
            PrivateData.TubePullingFromSecondEndpoint.transform.Translate(Vector3.down * TubeCenterOffset);

            var firstConnectionPoint = FirstEndpoint.BlobSite.GetPointOfConnectionFacingPoint(SecondEndpoint.transform.position);
            var secondConnectionPoint = SecondEndpoint.BlobSite.GetPointOfConnectionFacingPoint(FirstEndpoint.transform.position);

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
