using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ConstructionZones {

    public class ConstructionZoneUISummary {

        #region instance fields and properties

        public int ID { get; set; }

        #endregion

        #region constructors

        public ConstructionZoneUISummary() { }

        public ConstructionZoneUISummary(ConstructionZoneBase zoneToSummarize) {
            ID = zoneToSummarize.ID;
        }

        #endregion

    }

}
