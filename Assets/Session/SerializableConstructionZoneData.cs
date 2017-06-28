using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableConstructionZoneData {

        #region instance fields and properties

        [DataMember()] public int LocationID;

        [DataMember()] public string ProjectName;

        #endregion

        #region constructors

        public SerializableConstructionZoneData(ConstructionZoneBase constructionZone) {
            LocationID = constructionZone.Location.ID;
            ProjectName = constructionZone.CurrentProject.name;
        }

        #endregion

    }

}
