using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Depots {

    public abstract class ResourceDepotFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract bool HasDepotAtLocation(MapNodeBase location);
        public abstract ResourceDepotBase GetDepotAtLocation(MapNodeBase location);

        public abstract ResourceDepotBase ConstructDepot(MapNodeBase location);
        public abstract void DestroyDepot(ResourceDepotBase depot);

        #endregion

    }

}
