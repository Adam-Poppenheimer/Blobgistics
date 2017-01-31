using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    public class ResourcePool : BlobSourceAndTargetBehaviour, IResourcePool, IPointerEnterHandler,
        IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

        #region instance fields and properties

        #region from BlobSourceAndTargetBehaviour

        public override Vector3 NorthTubeConnectionPoint {
            get { return PrivateData.LocalNorthConnectionPoint + transform.position; }
        }

        public override Vector3 SouthTubeConnectionPoint {
            get { return PrivateData.LocalSouthConnectionPoint + transform.position; }
        }

        public override Vector3 EastTubeConnectionPoint {
            get { return PrivateData.LocalEastConnectionPoint + transform.position; }
        }

        public override Vector3 WestTubeConnectionPoint {
            get { return PrivateData.LocalWestConnectionPoint + transform.position; }
        }

        

        #endregion

        public ResourcePoolPrivateData PrivateData {
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
                    RealignToDimensions();
                }
            }
        }
        [SerializeField] private ResourcePoolPrivateData _privateData;

        private IBlobAlignmentStrategy AlignmentStrategy;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Initialize();
            Capacity = PrivateData.Capacity;
            RealignToDimensions();
        }

        #endregion

        #region Unity EventSystem message implementations

        public void OnPointerEnter(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandlePointerEnter(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandlePointerExit(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleBeginDrag(this, eventData);
        }
        
        public void OnDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleEndDrag(this, eventData);
        }

        #endregion

        #region from BlobSourceAndTargetBehaviour

        protected override void OnBlobPlacedInto(ResourceBlob blobPlaced) {
            AlignmentStrategy.RealignBlobs(BlobsWithin.Blobs, (Vector2)transform.position,
                PrivateData.RealignmentSpeedPerSecond);
        }

        #endregion

        private void RealignToDimensions() {
            var attachedMeshFilter = GetComponent<MeshFilter>();
            if(attachedMeshFilter != null) {
                attachedMeshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(
                    new Tuple<uint, uint, uint>(PrivateData.Width, PrivateData.Height, PrivateData.Depth));
            }
            var boxCollider = GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(PrivateData.Width, PrivateData.Height);
            }
            AlignmentStrategy = new BoxyBlobAlignmentStrategy(PrivateData.Width, PrivateData.Height, 5, 5);
        }

        #endregion

    }

}
