using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a highway manager.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableHighwayManagerData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the location of the highway manager.
        /// </summary>
        [DataMember()] public int LocationID;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given highway manager.
        /// </summary>
        /// <param name="highwayManager">The highway manager to pull data from</param>
        public SerializableHighwayManagerData(HighwayManagerBase highwayManager) {
            LocationID = highwayManager.Location.ID;
        }

        #endregion

    }

}
