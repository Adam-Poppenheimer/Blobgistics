using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.Core;

namespace Assets.Highways {

    /// <summary>
    /// The standard implementation for BlobHighwayBase, a gameplay element that transfers
    /// blobs between map nodes.
    /// </summary>
    /// <remarks>
    /// BlobHighway operates primarily through the use of two BlobTube objects, each of which handles
    /// resource flows going in one direction.
    /// </remarks>
    [ExecuteInEditMode]
    public class BlobHighway : BlobHighwayBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region static fields and properties

        /// <summary>
        /// The distance between the two tubes that make up a highway.
        /// </summary>
        public static readonly float TubeCenterOffset = 0.2f;

        #endregion

        #region instance fields and properties

        #region from BlobHighwayBase

        /// <inheritdoc/>
        public override int ID {
            get { return GetInstanceID(); }
        }

        /// <inheritdoc/>
        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        /// <inheritdoc/>
        public override BlobHighwayFactoryBase ParentFactory {
            get { return _parentFactory; }
            set { _parentFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _parentFactory;

        /// <inheritdoc/>
        public override UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <inheritdoc/>
        public override int Priority {
            get { return _priority; }
            set { _priority = value; }
        }
        [SerializeField] private int _priority = Int32.MaxValue;

        /// <inheritdoc/>
        public override MapNodeBase FirstEndpoint {
            get { return firstEndpoint; }
        }
        [SerializeField] private MapNodeBase firstEndpoint;

        /// <inheritdoc/>
        public override MapNodeBase SecondEndpoint {
            get { return secondEndpoint; }
        }
        [SerializeField] private MapNodeBase secondEndpoint;

        /// <inheritdoc/>
        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint {
            get { return TubePullingFromFirstEndpoint.Contents; }
        }

        /// <inheritdoc/>
        public override ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint {
            get { return TubePullingFromSecondEndpoint.Contents; }
        }

        /// <inheritdoc/>
        public override BlobHighwayProfile Profile {
            get { return _profile; }
            set { _profile = value; }
        }
        [SerializeField] private BlobHighwayProfile _profile;

        /// <inheritdoc/>
        public override float Efficiency {
            get { return _efficiency; }
            set {
                _efficiency = value;
                UpdateTubeSpeedAndCapacity();
            }
        }
        private float _efficiency = 1f;

        /// <inheritdoc/>
        public override float BlobPullCooldownInSeconds {
            get { return Profile.BlobPullCooldownInSeconds / Efficiency; }
        }

        #endregion

        /// <summary>
        /// The tube that handles all resource transfers from the first endpoint to the second endpoint.
        /// </summary>
        /// <remarks>
        /// This property might represent too much calculation to fit the semantics of a property. It might
        /// be wise to convert it into a method to emphasize the presence of additional calculations.
        /// </remarks>
        public BlobTubeBase TubePullingFromFirstEndpoint {
            get { return _tubePullingFromFirstEndpoint; }
            set {
                _tubePullingFromFirstEndpoint = value;
                if(_tubePullingFromFirstEndpoint != null) {
                    _tubePullingFromFirstEndpoint.BlobReachedEndOfTube += TubePullingFromFirstEndpoint_BlobReachedEndOfTube;
                    UpdateTubeSpeedAndCapacity();
                    UpdateTubeEndpoints();
                }
            }
        }
        [SerializeField] private BlobTubeBase _tubePullingFromFirstEndpoint;

        /// <summary>
        /// The tube that handles all resource transfers from the second endpoint to the first endpoint.
        /// </summary>
        /// <remarks>
        /// This property might represent too much calculation to fit the semantics of a property. It might
        /// be wise to convert it into a method to emphasize the presence of additional calculations.
        /// </remarks>
        public BlobTubeBase TubePullingFromSecondEndpoint {
            get { return _tubePullingFromSecondEndpoint; }
            set {
                _tubePullingFromSecondEndpoint = value;
                if(_tubePullingFromSecondEndpoint != null) {
                    _tubePullingFromSecondEndpoint.BlobReachedEndOfTube += TubePullingFromSecondEndpoint_BlobReachedEndOfTube;
                    UpdateTubeSpeedAndCapacity();
                    UpdateTubeEndpoints();
                }
            }
        }
        [SerializeField] private BlobTubeBase _tubePullingFromSecondEndpoint;

        private Dictionary<ResourceType, bool> UpkeepRequestedForResource =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region Unity event methods

        //As with other classes, it's important to make sure that the ParentFactory is aware of the state
        //of this highway even if it was copied into existence at design time.
        private void Start() {
            UpdateTubeSpeedAndCapacity();
            if(ParentFactory != null && ParentFactory.GetHighwayOfID(ID) == null) {
                ParentFactory.SubscribeHighway(this);
            }
        }

        private void OnDestroy() {
            if(UIControl != null && FirstEndpoint != null && SecondEndpoint != null) {
                UIControl.PushObjectDestroyedEvent(new BlobHighwayUISummary(this));
            }
            if(ParentFactory != null) {
                ParentFactory.UnsubscribeHighway(this);
            }
        }

        private void OnValidate() {
            UpdateTubeSpeedAndCapacity(); 
            if(ParentFactory != null && ParentFactory.GetHighwayOfID(ID) == null) {
                ParentFactory.SubscribeHighway(this);
            }        
        }

        #endregion

        #region EventSystem interface implementations

        /// <inheritdoc/>
        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new BlobHighwayUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new BlobHighwayUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new BlobHighwayUISummary(this), eventData);
        }

        #endregion

        #region from BlobHighwayBase
        /// <inheritdoc/>
        public override void SetEndpoints(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            this.firstEndpoint = firstEndpoint;
            this.secondEndpoint = secondEndpoint;
            UpdateTubeEndpoints();
        }

        /// <inheritdoc/>
        public override bool CanPullFromFirstEndpoint() {
            if(TubePullingFromFirstEndpoint.SpaceLeft > 0) {
                foreach(var blob in FirstEndpoint.BlobSite.Contents) {
                    var canExtractFromFirstEndpoint = FirstEndpoint.BlobSite.CanExtractBlob(blob);
                    var canPlaceIntoSecondEndpoint = SecondEndpoint.BlobSite.CanPlaceBlobInto(blob);
                    var CanPushIntoTube = TubePullingFromFirstEndpoint.CanPushBlobInto(blob);
                    if(canExtractFromFirstEndpoint && canPlaceIntoSecondEndpoint && CanPushIntoTube){
                        return true;
                    }
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public override void PullFromFirstEndpoint() {
            if(TubePullingFromFirstEndpoint.SpaceLeft > 0) {
                foreach(var blob in FirstEndpoint.BlobSite.Contents) {
                    if(
                        FirstEndpoint.BlobSite.CanExtractBlob(blob) &&
                        SecondEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        TubePullingFromFirstEndpoint.CanPushBlobInto(blob)
                    ){
                        FirstEndpoint.BlobSite.ExtractBlob(blob);
                        TubePullingFromFirstEndpoint.PushBlobInto(blob);
                        return;
                    }
                }
            }
            throw new BlobHighwayException("Cannot pull from this BlobHighway's FirstEndpoint");
        }

        /// <inheritdoc/>
        public override bool CanPullFromSecondEndpoint() {
            if(TubePullingFromSecondEndpoint.SpaceLeft > 0) {
                foreach(var blob in SecondEndpoint.BlobSite.Contents) {

                    var canExtractFromSecondEndpoint = SecondEndpoint.BlobSite.CanExtractBlob(blob);
                    var canPlaceIntoFirstEndpoint = FirstEndpoint.BlobSite.CanPlaceBlobInto(blob);
                    var canPushIntoTube = TubePullingFromSecondEndpoint.CanPushBlobInto(blob);
                    if(canExtractFromSecondEndpoint && canPlaceIntoFirstEndpoint && canPushIntoTube){
                        return true;
                    }

                }
            }
            return false;
        }

        /// <inheritdoc/>
        public override void PullFromSecondEndpoint() {
            if(TubePullingFromSecondEndpoint.SpaceLeft > 0) {
                foreach(var blob in SecondEndpoint.BlobSite.Contents) {
                    if(
                        SecondEndpoint.BlobSite.CanExtractBlob(blob) &&
                        FirstEndpoint.BlobSite.CanPlaceBlobInto(blob) &&
                        TubePullingFromSecondEndpoint.CanPushBlobInto(blob)
                    ){
                        SecondEndpoint.BlobSite.ExtractBlob(blob);
                        TubePullingFromSecondEndpoint.PushBlobInto(blob);
                        return;
                    }
                }
            }
            throw new BlobHighwayException("Cannot pull from this BlobHighway's FirstEndpoint");
        }

        /// <inheritdoc/>
        public override bool GetPullingPermissionForFirstEndpoint(ResourceType type) {
            return TubePullingFromFirstEndpoint.GetPermissionForResourceType(type);
        }

        /// <inheritdoc/>
        public override void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted) {
            TubePullingFromFirstEndpoint.SetPermissionForResourceType(type, isPermitted);
        }

        /// <inheritdoc/>
        public override bool GetPullingPermissionForSecondEndpoint(ResourceType type) {
            return TubePullingFromSecondEndpoint.GetPermissionForResourceType(type);
        }

        /// <inheritdoc/>
        public override void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted) {
            TubePullingFromSecondEndpoint.SetPermissionForResourceType(type, isPermitted);
        }

        /// <inheritdoc/>
        public override bool GetUpkeepRequestedForResource(ResourceType type) {
            bool retval;
            UpkeepRequestedForResource.TryGetValue(type, out retval);
            return retval;
        }

        /// <inheritdoc/>
        public override void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested) {
            UpkeepRequestedForResource[type] = isBeingRequested;
        }

        /// <inheritdoc/>
        public override void Clear() {
            TubePullingFromFirstEndpoint.Clear();
            TubePullingFromSecondEndpoint.Clear();
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
            if(TubePullingFromFirstEndpoint != null) {
                TubePullingFromFirstEndpoint.TransportSpeedPerSecond = Profile.BlobSpeedPerSecond * Efficiency;
                TubePullingFromFirstEndpoint.Capacity = Profile.Capacity;
            }
            if(TubePullingFromSecondEndpoint != null) {
                TubePullingFromSecondEndpoint.TransportSpeedPerSecond = Profile.BlobSpeedPerSecond * Efficiency;
                TubePullingFromSecondEndpoint.Capacity = Profile.Capacity;
            }
        }

        /*
         * This method makes sure that both tubes have the correct connection points and the correct
         * rotation. Most of the math in this method is to figure out how to keep a small separation
         * between the two tubes and also make sure that parenting relations and collider dimensions
         * are correct.
         */
        private void UpdateTubeEndpoints() {
            if( FirstEndpoint == null || SecondEndpoint == null ||
                TubePullingFromFirstEndpoint == null || TubePullingFromSecondEndpoint == null ){
                return;
            }
            TubePullingFromFirstEndpoint.transform.SetParent(transform, false);
            TubePullingFromSecondEndpoint.transform.SetParent(transform, false);

            TubePullingFromFirstEndpoint.transform.Translate(Vector3.up * TubeCenterOffset);
            TubePullingFromSecondEndpoint.transform.Translate(Vector3.down * TubeCenterOffset);

            var firstConnectionPoint = FirstEndpoint.BlobSite.GetPointOfConnectionFacingPoint(SecondEndpoint.transform.position);
            var secondConnectionPoint = SecondEndpoint.BlobSite.GetPointOfConnectionFacingPoint(FirstEndpoint.transform.position);

            EdgeOrientationUtil.AlignTransformWithEndpoints(transform, firstConnectionPoint, secondConnectionPoint, false);

            var boxCollider = GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(Vector3.Distance(firstConnectionPoint, secondConnectionPoint), boxCollider.size.y);
            }

            TubePullingFromFirstEndpoint.SetEndpoints(
                firstConnectionPoint + TubePullingFromFirstEndpoint.transform.TransformDirection(Vector3.up * TubeCenterOffset),
                secondConnectionPoint + TubePullingFromFirstEndpoint.transform.TransformDirection(Vector3.up * TubeCenterOffset)
            );
            TubePullingFromSecondEndpoint.SetEndpoints(
                secondConnectionPoint + TubePullingFromSecondEndpoint.transform.TransformDirection(Vector3.down * TubeCenterOffset),
                firstConnectionPoint + TubePullingFromSecondEndpoint.transform.TransformDirection(Vector3.down * TubeCenterOffset)
            );
        }

        //When a blob reaches the end of the tube, the highway checks to see if the blob can be placed in the corresponding
        //endpoint's BlobSite. This must be done because the state of the BlobSite the blob is heading towards could've
        //changed since the blob began its journey. If it can't be placed, the blob is destroyed.
        private void TubePullingFromFirstEndpoint_BlobReachedEndOfTube(object sender, BlobEventArgs e) {
            if(SecondEndpoint.BlobSite.CanPlaceBlobInto(e.Blob)) {
                TubePullingFromFirstEndpoint.PullBlobFrom(e.Blob);
                SecondEndpoint.BlobSite.PlaceBlobInto(e.Blob);
            }else {
                BlobFactory.DestroyBlob(e.Blob);
            }
        }

        private void TubePullingFromSecondEndpoint_BlobReachedEndOfTube(object sender, BlobEventArgs e) {
            if(FirstEndpoint.BlobSite.CanPlaceBlobInto(e.Blob)) {
                TubePullingFromSecondEndpoint.PullBlobFrom(e.Blob);
                FirstEndpoint.BlobSite.PlaceBlobInto(e.Blob);
            }else {
                BlobFactory.DestroyBlob(e.Blob);
            }
        }

        #endregion

    }

}
