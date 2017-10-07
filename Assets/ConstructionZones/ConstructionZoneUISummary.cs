using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.ConstructionZones {

    /// <summary>
    /// A class containing information that ConstructionZoneBase should pass into UIControl whenever
    /// it catches user input.
    /// </summary>
    public class ConstructionZoneUISummary {

        #region instance fields and properties

        /// <summary>
        /// The ConstructionZoneBase-unique ID of the underlying ConstructionZoneBase.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Summary information about construction project of the construction zone.
        /// </summary>
        public ConstructionProjectUISummary Project { get; set; }

        /// <summary>
        /// The transform associated with the construction zone.
        /// </summary>
        public Transform Transform { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an empty summary.
        /// </summary>
        public ConstructionZoneUISummary() { }

        /// <summary>
        /// Creates a summary with appropriate information supplied from a given construction zone.
        /// </summary>
        /// <param name="zoneToSummarize">The construction zone the summary should summarize</param>
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
