using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    /// <summary>
    /// The standard implementation for ResourceBlobBase. These objects represent the
    /// products and needs of societies, and their manipulation represents the bulk of
    /// gameplay.
    /// </summary>
    public class ResourceBlob : ResourceBlobBase {

        #region instance fields and properties

        #region from ResourceBlobBase
        /// <inheritdoc/>
        public override ResourceType BlobType { get; set; }

        #endregion

        /// <summary>
        /// The factory that created this ResourceBlob. Is used for graceful destruction,
        /// though it's possible to remove this dependency by making such destruction the
        /// responsibility of the factory itself.
        /// </summary>
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
            CurrentScaleVelocity = new Vector3(StartingScaleVelocity.x, StartingScaleVelocity.y, StartingScaleVelocity.z);
            StartCoroutine(PopIn());
        }

        private void OnDestroy() {
            if(ParentFactory != null) {
                ParentFactory.UnsubscribeBlob(this);
            }
            RaiseBeingDestroyed();
        }

        #endregion

        /// <inheritdoc/>
        public override void EnqueueNewMovementGoal(MovementGoal goal) {
            PendingMovementGoals.Enqueue(goal);
        }

        /// <inheritdoc/>
        public override void ClearAllMovementGoals() {
            PendingMovementGoals.Clear();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Movement is performed via Vector3.MoveTowards, which performs linear interpolation
        /// between the two locations and guarantees no overshoot. This could ostensibly be
        /// replaced by another similar method, like Vector3.SmoothDamp, without changing the
        /// underlying structure of the method.
        /// </remarks>
        // The complexity of this method arises from two main areas. One is the possibility
        // of floating point errors causing us some problems. It mitigates that possibility by comparing
        // distances against DestinationSnapDelta. The other possibility is that the loop might
        // need to execute several movement goals in a single tick, if secondsPassed is very
        // large. Thus it needs to keep track of how many seconds it's already spent pursuing
        // the current goal, and can't discard the loop until all of those seconds have been
        // eaten up. It's not clear that this process is efficient, but it's also not a performance
        // bottleneck so improving this method wasn't a priority during production.
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

        #endregion

    }

}
