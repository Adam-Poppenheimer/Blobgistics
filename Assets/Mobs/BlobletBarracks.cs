using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.AI;

using Assets.BlobEngine;
using Assets.Map;

namespace Assets.Mobs {

    public class BlobletBarracks : BlobletBarracksBase, IPointerEnterHandler,
        IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler  {

        #region instance fields and properties

        #region from BlobletBarracksBase

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

        public override bool AcceptsExtraction {
            get { return false; }
        }

        public override bool AcceptsPlacement {
            get { return true; }
        }

        protected override BlobPileBase BlobsWithin {
            get {
                if(_blobsWithin == null) {
                    _blobsWithin = new TypeConstrainedBlobPile(PrivateData.Capacity);
                }
                return _blobsWithin;
            }
        }
        private BlobPileBase _blobsWithin = null;

        protected override BlobPileBase BlobsWithReservedPositions {
            get {
                if(_blobsWithReservedPositions == null) {
                    _blobsWithReservedPositions = new TypeConstrainedBlobPile(PrivateData.Capacity);
                }
                return _blobsWithReservedPositions;
            }
        }
        private BlobPileBase _blobsWithReservedPositions = null;

        #endregion

        public BlobletBarracksPrivateData PrivateData {
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
                    Util.MeshResizerUtility.RealignToDimensions(gameObject,
                        new Tuple<uint, uint, uint>(PrivateData.Width, PrivateData.Height, PrivateData.Depth),
                        out AlignmentStrategy
                    );
                }
            }
        }
        [SerializeField] private BlobletBarracksPrivateData _privateData;

        private IBlobAlignmentStrategy AlignmentStrategy;

        private List<MapNode> NodesToSendBlobletsTo = new List<MapNode>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Util.MeshResizerUtility.RealignToDimensions(gameObject,
                new Tuple<uint, uint, uint>(PrivateData.Width, PrivateData.Height, PrivateData.Depth),
                out AlignmentStrategy
            );
            NodesToSendBlobletsTo.Clear();
            NodesToSendBlobletsTo.AddRange(PrivateData.Map.GetNeighborsOfNode(Location));
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

        #region from BlobSourceBehaviour

        protected override void OnBlobPlacedInto(ResourceBlob blobPlaced) {
            if(BlobsWithin.IsAtCapacity()) {
                ClearAllBlobs(false, true);
                Debug.Log("IsAtCapacity: " + BlobsWithin.IsAtCapacity());
                BuildBloblet();
            }
            AlignmentStrategy.RealignBlobs(BlobsWithin.Contents, (Vector2)transform.position,
                PrivateData.RealignmentSpeedPerSecond);
        }

        #endregion

        private void BuildBloblet() {
            var newBloblet = PrivateData.BlobletFactory.ConstructBloblet(Location.transform.position);
            foreach(var nodeToSeek in NodesToSendBlobletsTo) {
                newBloblet.EnqueueNewMovementGoal(nodeToSeek);
            }
        }

        #endregion

    }

}
