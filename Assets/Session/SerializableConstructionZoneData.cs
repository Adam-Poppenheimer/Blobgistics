using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;

namespace Assets.Session {

    [Serializable]
    public class SerializableConstructionZoneData {

        #region instance fields and properties

        public int LocationID;

        public string ProjectName;

        #endregion

        #region constructors

        public SerializableConstructionZoneData(ConstructionZoneBase constructionZone) {
            LocationID = constructionZone.Location.ID;
            ProjectName = constructionZone.CurrentProject.name;
        }

        #endregion

    }

}
