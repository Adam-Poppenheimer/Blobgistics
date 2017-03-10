using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Depots {

    public abstract class ResourceDepotFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract ResourceDepotBase ConstructDepot(MapNodeBase map);
        public abstract void DestroyDepot(ResourceDepotBase depot);

        #endregion

    }

}
