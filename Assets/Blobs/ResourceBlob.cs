using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    public class ResourceBlob : ResourceBlobBase {

        #region instance fields and properties

        public override ResourceType BlobType { get; set; }

        public ResourceBlobFactoryBase ParentFactory { get; set; }

        private Vector3 ScaleToPopTo;
        private Vector3 CurrentScaleVelocity;

        private Queue<MovementGoal> PendingMovementGoals =
            new Queue<MovementGoal>();

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

        private void OnDestroy() {
            if(ParentFactory != null) {
                ParentFactory.UnsubscribeBlob(this);
            }
            RaiseBeingDestroyed();
        }

        #endregion

        public override void PushNewMovementGoal(MovementGoal goal) {
            PendingMovementGoals.Enqueue(goal);
        }

        public override void ClearAllMovementGoals() {
            PendingMovementGoals.Clear();
        }

        public override void Tick(float secondsPassed) {
            float secondsLeftToIncrementOn = secondsPassed;
            while(PendingMovementGoals.Count > 0 && secondsLeftToIncrementOn > 0) {
                var currentGoal = PendingMovementGoals.Peek();
                float distanceToCurrentGoal = Vector3.Distance(transform.position, currentGoal.DesiredLocation);
                if(distanceToCurrentGoal <= DestinationSnapDelta) {
                    transform.position = currentGoal.DesiredLocation;
                    if(currentGoal.ActionToPerformOnTermination != null) {
                        currentGoal.ActionToPerformOnTermination();
                    }
                    PendingMovementGoals.Dequeue();
                }else {
                    float timeToAchieveGoal = distanceToCurrentGoal / currentGoal.SpeedPerSecond;
                    if(timeToAchieveGoal <= secondsLeftToIncrementOn) {
                        secondsLeftToIncrementOn -= timeToAchieveGoal;
                        transform.position = currentGoal.DesiredLocation;
                        if(currentGoal.ActionToPerformOnTermination != null) {
                            currentGoal.ActionToPerformOnTermination();
                        }
                        PendingMovementGoals.Dequeue();
                    }else {
                        transform.position = Vector3.MoveTowards(transform.position, currentGoal.DesiredLocation,
                            currentGoal.SpeedPerSecond * secondsLeftToIncrementOn);
                        secondsLeftToIncrementOn = 0f;
                    }
                }
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
                while(currentDistance > DestinationSnapDelta) {
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
