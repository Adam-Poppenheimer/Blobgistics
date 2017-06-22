using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ResourceDepots;

namespace Assets.Session {

    [Serializable]
    public class SerializableResourceDepotData {

        #region instance fields and properties

        public int LocationID;

        public ResourceDepotProfile Profile;

        #endregion

        #region constructors

        public SerializableResourceDepotData(ResourceDepotBase resourceDepot) {
            LocationID = resourceDepot.Location.ID;
            Profile = resourceDepot.Profile;
        }

        #endregion

    }

}
