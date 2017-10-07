using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.ResourceDepots;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a resource depot.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableResourceDepotData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the depot's location.
        /// </summary>
        [DataMember()] public int LocationID;

        /// <summary>
        /// The profile that determines the depot's capacity.
        /// </summary>
        [DataMember()] public ResourceDepotProfile Profile;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given resource depot.
        /// </summary>
        /// <param name="resourceDepot">The depot to pull data from</param>
        public SerializableResourceDepotData(ResourceDepotBase resourceDepot) {
            LocationID = resourceDepot.Location.ID;
            Profile = resourceDepot.Profile;
        }

        #endregion

    }

}
