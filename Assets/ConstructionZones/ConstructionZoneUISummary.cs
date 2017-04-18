using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.ConstructionZones {

    public class ConstructionZoneUISummary {

        #region instance fields and properties

        public int ID { get; set; }
        public ConstructionProjectUISummary Project { get; set; }

        public Transform Transform { get; set; }

        #endregion

        #region constructors

        public ConstructionZoneUISummary() { }

        public ConstructionZoneUISummary(ConstructionZoneBase zoneToSummarize) {
            ID = zoneToSummarize.ID;
            if(zoneToSummarize.CurrentProject != null) {
                Project = new ConstructionProjectUISummary(zoneToSummarize.CurrentProject);
            }

            Transform = zoneToSummarize.transform;
        }

        #endregion

    }

}
