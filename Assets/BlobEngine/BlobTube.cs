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
        private static float SecondsBetweenPulls = 1f;
        
        private static float TubeWidth = 0.5f;
        private static float TubeDepth = 0.5f;

        #endregion

        #region instance fields and properties

        private Vector3 TubeStart = Vector3.zero;
        private Vector3 TubeEnd = Vector3.zero;

        public IBlobSite SourceToPullFrom {
            get { return _sourceToPullFrom; }
            private set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _sourceToPullFrom = value;
                }
            }
        }
        private IBlobSite _sourceToPullFrom;

        public IBlobSite TargetToPushTo {
            get { return _targetToPushTo; }
            private set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _targetToPushTo = value;
                }
            }
        }
        private IBlobSite _targetToPushTo;

        private Coroutine BlobPullCoroutine;
        private bool ReadyToPullBlob = true;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            BlobPullCoroutine = StartCoroutine(BlobPullTick());
        }

        #endregion

        public void SetEndpoints(IBlobSite source, IBlobSite target) {
            if(!source.AcceptsExtraction || !target.AcceptsPlacement) {
                throw new BlobException("The given source and target do not represent a valid endpoint configuration");
            }
            if(SourceToPullFrom != null) {
                SourceToPullFrom.BlobPlacedInto -= OnSourceHasNewBlob;
            }
            SourceToPullFrom = source;
            TargetToPushTo   = target;
            RefreshEndpointLocations();
            
            SourceToPullFrom.BlobPlacedInto += OnSourceHasNewBlob;
            if(ReadyToPullBlob && CanTransportAnyBlob()) {
                PullAnyBlobFromSource();
            }
        }

        public bool CanTransportAnyBlob() {
            var meetsBasicConditions = (
                SourceToPullFrom != null &&
                TargetToPushTo   != null && 
                SourceToPullFrom.CanExtractAnyBlob()
            );
            if(meetsBasicConditions) {
                foreach(var extractableType in SourceToPullFrom.GetExtractableTypes()) {
                    if(TargetToPushTo.CanPlaceBlobOfTypeInto(extractableType)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void PullAnyBlobFromSource() {
            if(CanTransportAnyBlob()) {
                var pulledBlob = SourceToPullFrom.ExtractAnyBlob();
                //pulledBlob.transform.SetParent(transform, true);

                TargetToPushTo.ReservePlaceForBlob(pulledBlob);
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
            transform.position = (TubeStart + TubeEnd ) / 2f + new Vector3(0f, 0f, TubeDepth);
            transform.localScale = new Vector3(Vector3.Distance(TubeStart, TubeEnd), TubeWidth, TubeDepth);
            
            var zAngleToRotate = Mathf.Rad2Deg * Mathf.Atan(
                (TubeEnd.y - TubeStart.y) /
                (TubeEnd.x - TubeStart.x)
            );
            transform.Rotate(new Vector3(0f, 0f, zAngleToRotate));
        }

        private void OnSourceHasNewBlob(object sender, BlobEventArgs args) {
            if(ReadyToPullBlob && CanTransportAnyBlob()) {
                PullAnyBlobFromSource();
                ReadyToPullBlob = false;
                StopCoroutine(BlobPullCoroutine);
                BlobPullCoroutine = StartCoroutine(BlobPullTick());
            }
        }

        private IEnumerator BlobPullTick() {
            while(true) {
                yield return new WaitForSeconds(SecondsBetweenPulls);
                if(CanTransportAnyBlob()) {
                    PullAnyBlobFromSource();
                    ReadyToPullBlob = false;
                }else {
                    ReadyToPullBlob = true;
                }
            }
        }

        #endregion

    }

}
