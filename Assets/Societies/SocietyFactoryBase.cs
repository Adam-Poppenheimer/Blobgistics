using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public abstract class SocietyFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract SocietyBase GetSocietyOfID(int id);

        public abstract bool        CanConstructSocietyAt(MapNode location);
        public abstract SocietyBase ConstructSocietyAt   (MapNode location);

        public abstract void DestroySociety(SocietyBase society);

        public abstract void TickSocieties(float secondsPassed);

        #endregion

    }

}
