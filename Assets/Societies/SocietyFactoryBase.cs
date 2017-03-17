using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public abstract class SocietyFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ComplexityLadderBase StandardComplexityLadder { get; }

        #endregion

        #region instance methods

        public abstract SocietyBase GetSocietyOfID(int id);

        public abstract bool HasSocietyAtLocation(MapNodeBase location);
        public abstract SocietyBase GetSocietyAtLocation(MapNodeBase location);

        public abstract bool        CanConstructSocietyAt(MapNodeBase location);
        public abstract SocietyBase ConstructSocietyAt   (MapNodeBase location, ComplexityLadderBase ladder);

        public abstract void DestroySociety(SocietyBase society);

        public abstract void UnsubscribeSocietyBeingDestroyed(SocietyBase societyBeingDestroyed);

        public abstract void TickSocieties(float secondsPassed);

        #endregion

    }

}
