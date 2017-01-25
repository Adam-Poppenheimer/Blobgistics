using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BlobEngine {

    public class BoxyBlobAlignmentStrategy : IBlobAlignmentStrategy {

        #region instance fields and properties

        private readonly float BoundingWidth;
        private readonly float BoundingHeight;

        private readonly int BlobsPerRow;
        private readonly int BlobsPerColumn;

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

            CenteringVector = new Vector2(-BoundingWidth / 2f, -boundingHeight / 2f);
        }

        #endregion

        #region instance methods

        #region from IBlobAlignmentStrategy

        public void RealignBlobs(IEnumerable<ResourceBlob> blobsToAlign, Vector2 centerPosition,
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
