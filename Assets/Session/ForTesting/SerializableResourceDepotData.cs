using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.ResourceDepots;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableResourceDepotData {

        #region instance fields and properties

        [DataMember()] public int LocationID;

        [DataMember()] public ResourceDepotProfile Profile;

        #endregion

        #region constructors

        public SerializableResourceDepotData(ResourceDepotBase resourceDepot) {
            LocationID = resourceDepot.Location.ID;
            Profile = resourceDepot.Profile;
        }

        #endregion

    }

}
