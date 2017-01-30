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

    public class ResourcePool : MonoBehaviour, IResourcePool, IPointerEnterHandler,
        IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

        #region static fields and properties

        private static uint Depth = 1;
        private static float BlobRealignmentSpeedPerSecond = 2f;
        public static readonly BlobPileCapacity Cost = new BlobPileCapacity(new Dictionary<ResourceType, int>() {
            { ResourceType.Red, 10 },
        });

        #endregion

        #region instance fields and properties

        #region from ITubableObject

        public Vector3 NorthTubeConnectionPoint {
            get { return new Vector3(0f, Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public Vector3 SouthTubeConnectionPoint {
            get { return new Vector3(0f, -Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public Vector3 EastTubeConnectionPoint {
            get { return new Vector3(Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public Vector3 WestTubeConnectionPoint {
            get { return new Vector3(-Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        #endregion

        [SerializeField] private int MaxCapacity = 10;

        [SerializeField] private uint Width = 2;
        [SerializeField] private uint Height = 2;

        private HashSet<ResourceBlob> BlobsInPool =
            new HashSet<ResourceBlob>();

        private HashSet<ResourceBlob> BlobsWithReservedPositions =
            new HashSet<ResourceBlob>();

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
                }
            }
        }
        [SerializeField] private ResourcePoolPrivateData _privateData;

        private IBlobAlignmentStrategy AlignmentStrategy;

        #endregion

        #region events

        public event EventHandler<BlobEventArgs> NewBlobAvailable;

        protected void RaiseNewBlobAvailable(ResourceBlob newBlob) {
            if(NewBlobAvailable != null) {
                NewBlobAvailable(this, new BlobEventArgs(newBlob));
            }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            var attachedMeshFilter = GetComponent<MeshFilter>();
            if(attachedMeshFilter != null) {
                attachedMeshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(
                    new Tuple<uint, uint, uint>(Width, Height, Depth));
            }
            var boxCollider = GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(Width, Height);
            }
        }

        private void Awake() {
            AlignmentStrategy = new BoxyBlobAlignmentStrategy(Width, Height, 5, 5);
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

        #region from ITubableObject

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthTubeConnectionPoint;
                case ManhattanDirection.South: return SouthTubeConnectionPoint;
                case ManhattanDirection.East:  return EastTubeConnectionPoint;
                case ManhattanDirection.West:  return WestTubeConnectionPoint;
                default: return NorthTubeConnectionPoint;
            }
        }

        #endregion

        #region from IBlobSource

        public bool CanExtractAnyBlob() {
            return BlobsInPool.Count > 0;
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return BlobsInPool.Where(candidate => candidate.BlobType == type).Count() > 0;
        }

        public ResourceType GetTypeOfNextExtractedBlob() {
            if(CanExtractAnyBlob()) {
                return BlobsInPool.Last().BlobType;
            }else {
                throw new BlobException("There is no next blob to extract from this BlobSource");
            }
        }

        public ResourceBlob ExtractAnyBlob() {
            if(BlobsInPool.Count > 0) {
                var retval = BlobsInPool.Last();
                BlobsInPool.Remove(retval);
                retval.transform.SetParent(null, true);
                AlignmentStrategy.RealignBlobs(BlobsInPool, transform.position, BlobRealignmentSpeedPerSecond);
                return retval;
            }else {
                throw new BlobException("Cannot extract any blob from this BlobSource");
            }
            
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            var retval = BlobsInPool.Where(candidate => candidate.BlobType == type).Last();
            if(retval != null) {
                BlobsInPool.Remove(retval);
                retval.transform.SetParent(null, true);
                AlignmentStrategy.RealignBlobs(BlobsInPool, transform.position, BlobRealignmentSpeedPerSecond);
                return retval;
            }else {
                throw new BlobException("Cannot extract a blob of the specified type from this BlobSource");
            }
        }

        #endregion

        #region from IBlobTarget

        public bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return BlobsInPool.Count + BlobsWithReservedPositions.Count < MaxCapacity;
        }

        public void PlaceBlobInto(ResourceBlob blob) {
            BlobsWithReservedPositions.Remove(blob);
            if(CanPlaceBlobOfTypeInto(blob.BlobType)) {
                BlobsInPool.Add(blob);
                blob.transform.SetParent(this.transform, true);
                AlignmentStrategy.RealignBlobs(BlobsInPool, transform.position, BlobRealignmentSpeedPerSecond);
                RaiseNewBlobAvailable(blob);
            }else {
                throw new BlobException("Cannot place a blob into this BlobTarget");
            }
        }

        public void ReservePlaceForBlob(ResourceBlob blob) {
            if(CanPlaceBlobOfTypeInto(blob.BlobType)) {
                BlobsWithReservedPositions.Add(blob);
            }else {
                throw new BlobException("Cannot reserve a place for a blob in this BlobTarget");
            }
        }

        public void UnreservePlaceForBlob(ResourceBlob blob) {
            BlobsWithReservedPositions.Remove(blob);
        }

        #endregion

        public void ClearAllBlobs() {
            var blobsToDestroy = new List<ResourceBlob>(BlobsInPool);
            for(int i = blobsToDestroy.Count - 1; i >= 0; --i) {
                GameObject.Destroy(blobsToDestroy[i]);
            }
        }

        #endregion

    }

}
