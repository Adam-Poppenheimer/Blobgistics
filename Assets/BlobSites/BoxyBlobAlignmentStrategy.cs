using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    /// <summary>
    /// An alignment strategy that arranges blobs in a box of a certain width and height,
    /// with a certain number of blobs per row and column within the box.
    /// </summary>
    public class BoxyBlobAlignmentStrategy : BlobAlignmentStrategyBase {

        #region instance fields and properties

        [SerializeField, Range(0f, float.MaxValue)] private float BoundingWidth = 1f;
        [SerializeField, Range(0f, float.MaxValue)] private float BoundingHeight = 1f;

        [SerializeField, Range(0f, 10000)] private int BlobsPerRow = 5;
        [SerializeField, Range(0f, 10000)] private int BlobsPerColumn = 5;

        //The centering vector defines alignment in terms of the center of the box,
        //rather than its corners, and makes some calculations a bit easier.
        [SerializeField] private Vector2 CenteringVector;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            CenteringVector = new Vector2(-BoundingWidth / 2f, -BoundingHeight / 2f);
        }

        #endregion

        #region from BlobAlignmentStrategyBase

        /// <inheritdoc/>
        /// <remarks>
        /// Note that when the number of blobs in blobsToAlign is greater than BlobsPerRow * BlobsPerColumn, 
        /// all remaining blobs will not be moved. This is a problem with the alignment strategy that was of
        /// so little consequence to the end user experience that its resolution was never a priority. It
        /// remains a valid target for refactoring, however.
        /// </remarks>
        public override void RealignBlobs(IEnumerable<ResourceBlobBase> blobsToAlign, Vector2 centerPosition,
            float realignmentSpeedPerSecond) {
            int blobIndex = 0;
            var blobList = new List<ResourceBlobBase>(blobsToAlign);

            float xDistanceToWorkWith = BoundingWidth  - ResourceBlobBase.RadiusOfBlobs;
            float yDistanceToWorkWith = BoundingHeight - ResourceBlobBase.RadiusOfBlobs;

            for(int verticalIndex = 0; verticalIndex < BlobsPerColumn; ++verticalIndex) {
                for(int horizontalIndex = 0; horizontalIndex < BlobsPerRow; ++horizontalIndex) {
                    if(blobIndex == blobList.Count) {
                        return;
                    }else {
                        var blobToPlace = blobList[blobIndex++];
                        var newBlobLocation = new Vector3(
                            ResourceBlobBase.RadiusOfBlobs + ((float)horizontalIndex / (float)BlobsPerRow)    * xDistanceToWorkWith,
                            ResourceBlobBase.RadiusOfBlobs + ((float)verticalIndex   / (float)BlobsPerColumn) * yDistanceToWorkWith,
                            ResourceBlobBase.DesiredZPositionOfAllBlobs
                        ) + (Vector3)CenteringVector + (Vector3)centerPosition;

                        blobToPlace.EnqueueNewMovementGoal(new MovementGoal(newBlobLocation, realignmentSpeedPerSecond));
                    }
                }
            }
        }

        #endregion

        #endregion
        
    }

}
