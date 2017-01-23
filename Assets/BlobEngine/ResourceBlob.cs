using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class ResourceBlob : MonoBehaviour {

        #region static fields and properties

        public static float DesiredZPositionOfAllBlobs = -1f;

        private static float SecondsToPopIn = 0.25f;
        private static Vector3 StartingVelocity = new Vector3(5f, 5f, 5f);

        #endregion

        #region instance fields and properties

        public ResourceType BlobType;

        private Vector3 ScaleToPopTo;
        private Vector3 CurrentScaleVelocity;

        private Queue<MovementGoal> PendingMovementGoals =
            new Queue<MovementGoal>();

        private Coroutine ActiveMovementCoroutine = null;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            ScaleToPopTo = transform.localScale;
        }

        private void OnEnable() {
            CurrentScaleVelocity = new Vector3(StartingVelocity.x, StartingVelocity.y, StartingVelocity.z);
            StartCoroutine(PopIn());
        }

        #endregion

        public void PushNewMovementGoal(MovementGoal goal) {
            int previousCount = PendingMovementGoals.Count;
            PendingMovementGoals.Enqueue(goal);
            if(previousCount == 0) {
                StartCoroutine(ExecutePendingMovementGoals());
            }
        }

        private IEnumerator PopIn() {
            transform.localScale = Vector3.zero;
            while(true) {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, ScaleToPopTo,
                    ref CurrentScaleVelocity, SecondsToPopIn);
                if(Mathf.Approximately(transform.localScale.x, ScaleToPopTo.x)) {
                    yield break;
                }else {
                    yield return null;
                }
            }
        }

        private IEnumerator ExecutePendingMovementGoals() {
            while(PendingMovementGoals.Count > 0) {
                var goalToExecute = PendingMovementGoals.Peek();
                float currentDistance = Vector3.Distance(transform.position, goalToExecute.DesiredLocation);
                while(!Mathf.Approximately(currentDistance, 0f) ) {
                    transform.position = Vector3.MoveTowards(transform.position, goalToExecute.DesiredLocation, 
                        goalToExecute.SpeedPerSecond * Time.deltaTime);
                    currentDistance = Vector3.Distance(transform.position, goalToExecute.DesiredLocation);
                    yield return null;
                }
                transform.position = goalToExecute.DesiredLocation;
                if(goalToExecute.ActionToPerformOnTermination != null) {
                    goalToExecute.ActionToPerformOnTermination();
                }
                PendingMovementGoals.Dequeue();
            }
            yield break;
        }

        #endregion

    }

}
