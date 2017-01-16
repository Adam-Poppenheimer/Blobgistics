using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class ResourcePool : MonoBehaviour, IPointerClickHandler {

        #region static fields and properties

        private static float BlobRadius = 0.25f;
        private static float BlobHorizontalSeparation = 0.25f;
        private static float BlobVerticalSeparation = 0.25f;

        private static uint Depth = 1;

        #endregion

        #region instance fields and properties

        [SerializeField] private int MaxCapacity = 25;

        [SerializeField] private uint Width = 2;
        [SerializeField] private uint Height = 2;

        private Queue<ResourceBlob> BlobQueue =
            new Queue<ResourceBlob>();

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
            if(CanInsertBlob()) {
                InsertBlob(ResourceBlobBuilder.BuildBlob(ResourceType.Red));
            }
        }

        #endregion

        public bool CanExtractBlob() {
            return BlobQueue.Count > 0;
        }

        public ResourceBlob ExtractBlob() {
            if(CanExtractBlob()) {
                return BlobQueue.Dequeue();
            }else {
                throw new BlobException("Cannot extract a blob from this ResourcePool");
            }
        }

        public bool CanInsertBlob() {
            return BlobQueue.Count < MaxCapacity;
        }

        public void InsertBlob(ResourceBlob blobToInsert) {
            if(CanInsertBlob()) {
                blobToInsert.transform.SetParent(this.transform, false);
                BlobQueue.Enqueue(blobToInsert);
                RealignBlobs();
            }else {
                throw new BlobException("Cannot insert a blob into this ResourcePool");
            }
        }

        public void ClearAllBlobs() {
            var blobsToDestroy = new List<ResourceBlob>(BlobQueue);
            for(int i = blobsToDestroy.Count - 1; i >= 0; --i) {
                GameObject.Destroy(blobsToDestroy[i]);
            }
        }

        private void RealignBlobs() {
            int blobIndex = 0;
            var blobList = new List<ResourceBlob>(BlobQueue);

            for(int verticalIndex = 0; verticalIndex < 5; ++verticalIndex) {
                for(int horizontalIndex = 0; horizontalIndex < 5; ++horizontalIndex){
                    if(blobIndex == blobList.Count) {
                        return;
                    }else {
                        var blobToPlace = blobList[blobIndex++];
                        blobToPlace.transform.localPosition = new Vector3(
                            BlobRadius + (BlobRadius / 2f + BlobHorizontalSeparation) * horizontalIndex,
                            -(BlobRadius + (BlobRadius / 2f + BlobVerticalSeparation  ) * verticalIndex),
                            -BlobRadius
                        ) + new Vector3(
                            -Width / 2f,
                            Height / 2f,
                            -Depth / 2f 
                        );
                    }
                }
            }
        }

        #endregion

    }

}
