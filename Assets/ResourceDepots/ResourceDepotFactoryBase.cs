using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ResourceDepots {

    public abstract class ResourceDepotFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<ResourceDepotBase> ResourceDepots { get; }

        #endregion

        #region instance methods

        public abstract ResourceDepotBase GetDepotOfID(int id);

        public abstract bool HasDepotAtLocation(MapNodeBase location);
        public abstract ResourceDepotBase GetDepotAtLocation(MapNodeBase location);

        public abstract ResourceDepotBase ConstructDepotAt(MapNodeBase location);

        public abstract void DestroyDepot(ResourceDepotBase depot);
        public abstract void UnsubscribeDepot(ResourceDepotBase depot);

        #endregion

    }

}
