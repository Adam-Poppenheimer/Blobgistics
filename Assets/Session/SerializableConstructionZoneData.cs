using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a construction zone.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableConstructionZoneData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the map node on which the ConstructionZone is located.
        /// </summary>
        [DataMember()] public int LocationID;

        /// <summary>
        /// The name of the project that was within the ConstructionZone.
        /// </summary>
        [DataMember()] public string ProjectName;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from a given construction zone.
        /// </summary>
        /// <param name="constructionZone">The construction zone to pull data from</param>
        public SerializableConstructionZoneData(ConstructionZoneBase constructionZone) {
            LocationID = constructionZone.Location.ID;
            ProjectName = constructionZone.CurrentProject.name;
        }

        #endregion

    }

}
