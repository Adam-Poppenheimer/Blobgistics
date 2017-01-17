using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class ResourcePool : MonoBehaviour, IPointerClickHandler, IBlobSource, IBlobTarget {

        #region static fields and properties

        private static float BlobRadius = 0.25f;
        private static float BlobHorizontalSeparation = 0.25f;
        private static float BlobVerticalSeparation = 0.25f;

        private static uint Depth = 1;

        private static float BlobRealignmentSpeedPerSecond = 2f;

        #endregion

        #region instance fields and properties

        #region from ITubableObject

        public Vector3 NorthTubeConnectionPoint {
            get { return new Vector3(0f, Height / 2f, Depth) + transform.position; }
        }

        public Vector3 SouthTubeConnectionPoint {
            get { return new Vector3(0f, -Height / 2f, Depth) + transform.position; }
        }

        public Vector3 EastTubeConnectionPoint {
            get { return new Vector3(Width / 2f, 0f, Depth) + transform.position; }
        }

        public Vector3 WestTubeConnectionPoint {
            get { return new Vector3(-Width / 2f, 0f, Depth) + transform.position; }
        }

        #endregion

        [SerializeField] private int MaxCapacity = 25;

        [SerializeField] private uint Width = 2;
        [SerializeField] private uint Height = 2;

        private HashSet<ResourceBlob> BlobsInPool =
            new HashSet<ResourceBlob>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            var attachedMeshFilter = GetComponent<MeshFilter>();
            if(attachedMeshFilter != null) {
                attachedMeshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(
                    new Tuple<uint, uint, uint>(Width, Height, Depth));
            }
        }

        #endregion

        #region from IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData) {
            if(CanPlaceBlobOfTypeInto(ResourceType.Red)) {
                PlaceBlobInto(ResourceBlobBuilder.BuildBlob(ResourceType.Red));
            }
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
                return retval;
            }else {
                throw new BlobException("Cannot extract any blob from this BlobSource");
            }
            
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            var retval = BlobsInPool.Where(candidate => candidate.BlobType == type).Last();
            if(retval != null) {
                BlobsInPool.Remove(retval);
                RealignBlobs();
                return retval;
            }else {
                throw new BlobException("Cannot extract a blob of the specified type from this BlobSource");
            }
        }

        #endregion

        #region from IBlobTarget

        public bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return BlobsInPool.Count < MaxCapacity;
        }

        public void PlaceBlobInto(ResourceBlob blob) {
            if(CanPlaceBlobOfTypeInto(blob.BlobType)) {
                BlobsInPool.Add(blob);
                blob.transform.SetParent(this.transform, true);
                RealignBlobs();
            }else {
                throw new BlobException("Cannot place a blob into a BlobTarget");
            }
        }

        #endregion

        public void ClearAllBlobs() {
            var blobsToDestroy = new List<ResourceBlob>(BlobsInPool);
            for(int i = blobsToDestroy.Count - 1; i >= 0; --i) {
                GameObject.Destroy(blobsToDestroy[i]);
            }
        }

        private void RealignBlobs() {
            int blobIndex = 0;
            var blobList = new List<ResourceBlob>(BlobsInPool);

            for(int verticalIndex = 0; verticalIndex < 5; ++verticalIndex) {
                for(int horizontalIndex = 0; horizontalIndex < 5; ++horizontalIndex){
                    if(blobIndex == blobList.Count) {
                        return;
                    }else {
                        var blobToPlace = blobList[blobIndex++];
                        var newBlobLocation = new Vector3(
                            BlobRadius + (BlobRadius / 2f + BlobHorizontalSeparation) * horizontalIndex,
                            -(BlobRadius + (BlobRadius / 2f + BlobVerticalSeparation  ) * verticalIndex),
                            -BlobRadius
                        ) + new Vector3(
                            -Width / 2f,
                            Height / 2f,
                            -Depth / 2f 
                        ) + transform.position;
                        blobToPlace.PushNewMovementGoal(new MovementGoal(newBlobLocation, BlobRealignmentSpeedPerSecond));
                    }
                }
            }
        }

        #endregion

    }

}
