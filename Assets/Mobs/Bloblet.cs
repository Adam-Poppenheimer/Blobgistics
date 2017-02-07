using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;

using Assets.Map;

namespace Assets.Mobs {

    public class Bloblet : AutonomousAgent2DBase {

        #region instance fields and properties

        #region from AutonomousAgent2DBase

        public override float Mass {
            get { return Config.Mass; }
        }

        public override float MaxForce {
            get { return Config.MaxForce; }
        }

        public override float MaxSpeed {
            get { return Config.MaxSpeed; }
        }

        public override float MaxTurnRateInRadians {
            get { return Config.MaxTurnRateInRadians; }
        }

        protected override ISteeringLogic2DConfig SteeringLogicConfig {
            get { return Config.SteeringLogicConfig; }
        }

        #endregion

        [SerializeField] private BlobletConfig Config;

        #endregion

        #region instance methods

        #region from AutonomousAgent2DBase

        public override IEnumerable<AutonomousAgent2DBase> GetNeighborsWithinViewRange() {
            var retval = new List<AutonomousAgent2DBase>();
            foreach(var collider in Physics2D.OverlapCircleAll(this.transform.position, Config.NeighborCheckRadius)) {
                var agent = collider.GetComponent<AutonomousAgent2DBase>();
                if(agent != null) {
                    retval.Add(agent);
                }
            }
            return retval;
        }

        #endregion

        #endregion

    }

}
