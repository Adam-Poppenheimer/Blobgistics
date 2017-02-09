using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.AI;

using Assets.Map;

namespace Assets.Mobs {

    public abstract class BlobletBase : AutonomousAgent2DBase {

        #region instance methods

        public abstract void EnqueueNewMovementGoal(MapNode locationToSeek);
        public abstract void ClearAllMovementGoals();

        #endregion

    }

}
