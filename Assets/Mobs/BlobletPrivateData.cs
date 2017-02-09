using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;

using Assets.Map;

namespace Assets.Mobs {

    public class BlobletPrivateData : MonoBehaviour {

        public float Mass {
            get { return _mass; }
        }
        [SerializeField] private float _mass;

        public float MaxForce {
            get { return _maxForce; }
        }
        [SerializeField] private float _maxForce;

        public float MaxSpeed {
            get { return _maxSpeed; }
        }
        [SerializeField] private float _maxSpeed;

        public float MaxTurnRateInRadians {
            get { return _maxTurnRateInRadians; }
        }
        [SerializeField] private float _maxTurnRateInRadians;

        public ISteeringLogic2DConfig SteeringLogicConfig {
            get { return _steeringLogicConfig; }
        }
        [SerializeField] private SteeringLogic2DConfig _steeringLogicConfig;

        public float NeighborCheckRadius {
            get { return _neighborCheckRadius; }
        }
        [SerializeField] private float _neighborCheckRadius;

    }

}
