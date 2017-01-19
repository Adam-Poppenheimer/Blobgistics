using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class BlobTube : MonoBehaviour {

        #region static fields and properties

        private static float BlobSpeed = 2f;
        private static float SecondsBetweenSourceChecks = 1f;
        
        private static float TubeWidth = 0.5f;
        private static float TubeDepth = 0.5f;

        #endregion

        #region instance fields and properties

        private Vector3 TubeStart = Vector3.zero;
        private Vector3 TubeEnd = Vector3.zero;

        private IBlobSource SourceToPullFrom;
        private IBlobTarget TargetToPushTo;

        private Coroutine BlobPullingCoroutine;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            
        }

        #endregion

        public void SetEndpoints(IBlobSource source, IBlobTarget target) {
            if(BlobPullingCoroutine != null) {
                StopCoroutine(BlobPullingCoroutine);
            }
            SourceToPullFrom = source;
            TargetToPushTo   = target;
            RefreshEndpointLocations();
            BlobPullingCoroutine = StartCoroutine(BlobPullTick());
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
                //pulledBlob.transform.SetParent(transform, true);

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
            
            
            var meshFilter = GetComponent<MeshFilter>();
            if(meshFilter != null) {
                meshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(new Tuple<uint, uint, uint>(1, 1, 1));
            }
            
            transform.rotation = Quaternion.identity;
            transform.position = (TubeStart + TubeEnd ) / 2f;
            transform.localScale = new Vector3(Vector3.Distance(TubeStart, TubeEnd), TubeWidth, TubeDepth);
            
            var zAngleToRotate = Mathf.Rad2Deg * Mathf.Atan(
                (TubeEnd.y - TubeStart.y) /
                (TubeEnd.x - TubeStart.x)
            );
            transform.Rotate(new Vector3(0f, 0f, zAngleToRotate));
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
