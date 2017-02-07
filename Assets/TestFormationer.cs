using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;

using Assets.Mobs;
using Assets.Map;

namespace Assets {

    public class TestFormationer : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private Bloblet Leader;
        [SerializeField] private List<Bloblet> OtherParticipants;
        [SerializeField] private MapGraph Map;
        [SerializeField] private MapNode SpecialNode;

        private Formation TestFormation = new Formation();

        #endregion

        #region instance fields and properties

        #region Unity event methods

        private void Start() {
            foreach(var participant in OtherParticipants) {
                TestFormation.AddParticipant(participant);
            }
            TestFormation.AddParticipant(Leader);
            TestFormation.SetLeader(Leader);

            var pathWaypoints = new List<Vector2>() { SpecialNode.transform.position };
            foreach(var neighbor in Map.GetNeighborsOfNode(SpecialNode)) {
                pathWaypoints.Add((Vector2)neighbor.transform.position);
            }

            TestFormation.SetPath(pathWaypoints);
        }

        #endregion

        #endregion

    }

}
