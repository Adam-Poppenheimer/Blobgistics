using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;

using Assets.Map;

namespace Assets.Mobs {

    public class Formation {

        #region instance fields and properties

        private HashSet<Bloblet> Participants = new HashSet<Bloblet>();
        private Bloblet Leader;
        private List<Vector2> PathWaypoints = new List<Vector2>();

        #endregion

        #region constructors

        public Formation() {

        }

        #endregion

        #region instance methods

        public void AddParticipant(Bloblet newParticipant) {
            if(!Participants.Contains(newParticipant)) {
                newParticipant.SteeringLogic.Clear();
                if(Leader != null) {
                    SetParticipantToFollowLeader(newParticipant, Leader,
                        newParticipant.transform.position - Leader.transform.position);
                }
                Participants.Add(newParticipant);
            }
        }

        public void SetLeader(Bloblet newLeader) {
            if(newLeader == null) {
                throw new ArgumentNullException("newLeader");
            }else if(!Participants.Contains(newLeader)) {
                throw new ArgumentOutOfRangeException("newLeader", "the requested leader is not a " + 
                    "participant in the formation");
            }else {
                Leader = newLeader;
                foreach(var participant in Participants) {
                    if(participant != Leader) {
                        SetParticipantToFollowLeader(participant, Leader,
                            participant.transform.position - Leader.transform.position);
                    }
                }
                if(PathWaypoints != null && PathWaypoints.Count > 0) {
                    SetLeaderToFollowPath(Leader, PathWaypoints);
                }
            }
        }

        public void SetPath(List<Vector2> pathWaypoints) {
            if(pathWaypoints == null) {
                throw new ArgumentNullException("pathWaypoints");
            }
            PathWaypoints = new List<Vector2>(pathWaypoints);
            if(Leader != null) {
                SetLeaderToFollowPath(Leader, PathWaypoints);
            }
        }

        private void SetParticipantToFollowLeader(Bloblet participant, Bloblet leader, Vector2 desireOffset) {
            var logicToSet = participant.SteeringLogic;
            logicToSet.Clear();
            logicToSet.CurrentTargetAgent = leader;
            logicToSet.PursuitOffsetPoint = desireOffset;
            logicToSet.TurnOn(SteeringLogic2D.BehaviourTypeFlags.OffsetPursuit);
            logicToSet.TurnOn(SteeringLogic2D.BehaviourTypeFlags.Separation);
        }

        private void SetLeaderToFollowPath(Bloblet leader, List<Vector2> path) {
            if(leader == null) {
                throw new ArgumentNullException("leader");
            }else if(path == null) {
                throw new ArgumentNullException("path");
            }else {
                var leaderLogic = leader.SteeringLogic;
                leaderLogic.Clear();
                leaderLogic.SetPath(path);
                leaderLogic.TurnOn(SteeringLogic2D.BehaviourTypeFlags.FollowPath);
                leaderLogic.TurnOn(SteeringLogic2D.BehaviourTypeFlags.Separation);
            }
        }

        #endregion

    }

}
