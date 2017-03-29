using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    public class BoxyBlobAlignmentStrategy : BlobAlignmentStrategyBase {

        #region instance fields and properties

        [SerializeField, Range(0f, float.MaxValue)] private float BoundingWidth;
        [SerializeField, Range(0f, float.MaxValue)] private float BoundingHeight;

        [SerializeField, Range(0f, 10000)] private int BlobsPerRow;
        [SerializeField, Range(0f, 10000)] private int BlobsPerColumn;

        private Vector2 CenteringVector;

        #endregion

        #region constructors

        public BoxyBlobAlignmentStrategy(float boundingWidth, float boundingHeight,
            int blobsPerRow, int blobsPerColumn) {
            if(boundingWidth < 0f) {
                throw new ArgumentOutOfRangeException("boundingWidth must be greater than or equal to zero");
            }else if(boundingHeight < 0f) {
                throw new ArgumentOutOfRangeException("boundingHeight must be greater than or equal to zero");
            }else if(blobsPerRow == 0) {
                throw new ArgumentOutOfRangeException("blobsPerRow must be greater than zero");
            }else if(blobsPerColumn == 0) {
                throw new ArgumentOutOfRangeException("blobsPerColumn must be greater than zero");
            }
            BoundingWidth  = boundingWidth;
            BoundingHeight = boundingHeight;
            BlobsPerRow    = blobsPerRow;
            BlobsPerColumn = blobsPerColumn;

            
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            CenteringVector = new Vector2(-BoundingWidth / 2f, -BoundingHeight / 2f);
        }

        #endregion

        #region from BlobAlignmentStrategyBase

        public override void RealignBlobs(IEnumerable<ResourceBlob> blobsToAlign, Vector2 centerPosition,
            float realignmentSpeedPerSecond) {
            int blobIndex = 0;
            var blobList = new List<ResourceBlob>(blobsToAlign);

            float xDistanceToWorkWith = BoundingWidth  - ResourceBlob.RadiusOfBlobs;
            float yDistanceToWorkWith = BoundingHeight - ResourceBlob.RadiusOfBlobs;

            for(int verticalIndex = 0; verticalIndex < BlobsPerColumn; ++verticalIndex) {
                for(int horizontalIndex = 0; horizontalIndex < BlobsPerRow; ++horizontalIndex) {
                    if(blobIndex == blobList.Count) {
                        return;
                    }else {
                        var blobToPlace = blobList[blobIndex++];
                        var newBlobLocation = new Vector3(
                            ResourceBlob.RadiusOfBlobs + ((float)horizontalIndex / (float)BlobsPerRow)    * xDistanceToWorkWith,
                            ResourceBlob.RadiusOfBlobs + ((float)verticalIndex   / (float)BlobsPerColumn) * yDistanceToWorkWith,
                            ResourceBlob.DesiredZPositionOfAllBlobs
                        ) + (Vector3)CenteringVector + (Vector3)centerPosition;

                        blobToPlace.PushNewMovementGoal(new MovementGoal(newBlobLocation, realignmentSpeedPerSecond));
                    }
                }
            }
        }

        #endregion

        #endregion
        
    }

}
