using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class BlobTube : MonoBehaviour {

        #region static fields and properties

        private static float BlobSpeed = 2f;
        private static float SecondsBetweenSourceChecks = 1f;

        #endregion

        #region instance fields and properties

        private Vector3 TubeStart = Vector3.zero;
        private Vector3 TubeEnd = Vector3.zero;

        private IBlobSource SourceToPullFrom;
        private IBlobTarget TargetToPushTo;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            StartCoroutine(BlobPullTick());
        }

        #endregion

        public void SetEndpoints(IBlobSource source, IBlobTarget target) {
            SourceToPullFrom = source;
            TargetToPushTo   = target;
            RefreshEndpointLocations();
        }

        public bool CanTransportAnyBlob() {
            return (
                SourceToPullFrom != null &&
                TargetToPushTo   != null && 
                SourceToPullFrom.CanExtractAnyBlob() &&
                TargetToPushTo.CanPlaceBlobOfTypeInto( SourceToPullFrom.GetTypeOfNextExtractedBlob() )
            );  
        }

        public void PullAnyBlobFromSource() {
            if(CanTransportAnyBlob()) {
                var pulledBlob = SourceToPullFrom.ExtractAnyBlob();
                pulledBlob.transform.SetParent(transform, true);

                pulledBlob.PushNewMovementGoal(new MovementGoal(TubeStart, BlobSpeed));
                pulledBlob.PushNewMovementGoal(new MovementGoal(TubeEnd, BlobSpeed, delegate() {
                    TargetToPushTo.PlaceBlobInto(pulledBlob);
                }));

            }else {
                throw new BlobException("This BlobTube either cannot pull a blob from its source, or push it to its target");
            }
        }

        private void RefreshEndpointLocations() {
            var directionFromSource = SourceToPullFrom.transform.GetDominantManhattanDirectionTo(TargetToPushTo.transform);
            var directionFromTarget = TargetToPushTo.transform.GetDominantManhattanDirectionTo(SourceToPullFrom.transform);
            TubeStart = SourceToPullFrom.GetConnectionPointInDirection(directionFromSource);
            TubeEnd = TargetToPushTo.GetConnectionPointInDirection(directionFromTarget);
        }

        private IEnumerator BlobPullTick() {
            while(true) {
                if(CanTransportAnyBlob()) {
                    PullAnyBlobFromSource();
                }
                yield return new WaitForSeconds(SecondsBetweenSourceChecks);
            }
        }

        #endregion

    }

}
