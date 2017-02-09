using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;
using UnityCustomUtilities.Extensions;

using Assets.Map;

namespace Assets.Mobs {

    public class Bloblet : BlobletBase {

        #region instance fields and properties

        #region from AutonomousAgent2DBase

        public override float Mass {
            get { return PrivateData.Mass; }
        }

        public override float MaxForce {
            get { return PrivateData.MaxForce; }
        }

        public override float MaxSpeed {
            get { return PrivateData.MaxSpeed; }
        }

        public override float MaxTurnRateInRadians {
            get { return PrivateData.MaxTurnRateInRadians; }
        }

        protected override ISteeringLogic2DConfig SteeringLogicConfig {
            get { return PrivateData.SteeringLogicConfig; }
        }

        #endregion

        public BlobletPrivateData PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                }
            }
        }
        [SerializeField] private BlobletPrivateData _privateData;

        private Queue<MapNode> PendingMovementGoals = new Queue<MapNode>();
        
        private MapNode CurrentMovementGoal;

        #endregion

        #region instance methods

        #region Unity event methods

        protected override void DoOnFixedUpdate() {
            if(CurrentMovementGoal != null) {
                var distanceFromGoal = Vector2.Distance(CurrentMovementGoal.transform.position, 
                    this.transform.position);
                if(distanceFromGoal <= SteeringLogicConfig.WaypointSeekDistance) {
                    Debug.Log("Arrive at MovementGoal");
                    if(PendingMovementGoals.Count > 0) {
                        SwitchToNextMovementGoal();
                    }else {
                        ComeToStopAtCurrentMovementGoal();
                    }
                }
            }
        }

        #endregion

        #region from BlobletBase

        public override IEnumerable<AutonomousAgent2DBase> GetNeighborsWithinViewRange() {
            var retval = new List<AutonomousAgent2DBase>();
            foreach(var collider in Physics2D.OverlapCircleAll(this.transform.position, PrivateData.NeighborCheckRadius)) {
                var agent = collider.GetComponent<AutonomousAgent2DBase>();
                if(agent != null) {
                    retval.Add(agent);
                }
            }
            return retval;
        }

        public override void EnqueueNewMovementGoal(MapNode locationToSeek) {
            if(locationToSeek == null) {
                throw new ArgumentNullException("locationToSeek");
            }
            PendingMovementGoals.Enqueue(locationToSeek);
            if(CurrentMovementGoal == null) {
                SwitchToNextMovementGoal();
            }
        }

        public override void ClearAllMovementGoals() {
            TerminateCurrentMovementGoal();
            PendingMovementGoals.Clear();
        }

        #endregion

        private bool SwitchToNextMovementGoal() {
            TerminateCurrentMovementGoal();
            if(PendingMovementGoals.Count > 0) {
                CurrentMovementGoal = PendingMovementGoals.Dequeue();
                SteeringLogic.CurrentTargetPoint = CurrentMovementGoal.transform.position;
                SteeringLogic.TurnOn(SteeringLogic2D.BehaviourTypeFlags.Arrive);
                return true;
            }else {
                return false;
            }
        }

        private void TerminateCurrentMovementGoal() {
            CurrentMovementGoal = null;
            SteeringLogic.CurrentTargetPoint = transform.position;
            SteeringLogic.TurnOff(SteeringLogic2D.BehaviourTypeFlags.Arrive);
        }

        private void ComeToStopAtCurrentMovementGoal() {
            CurrentMovementGoal = null;
            PendingMovementGoals.Clear();
        }

        #endregion

    }

}
