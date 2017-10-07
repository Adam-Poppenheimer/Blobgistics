using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    /// <summary>
    /// A class containing information that the UI might need to know about a given construction project.
    /// </summary>
    public class ConstructionProjectUISummary {

        #region instance fields and properties

        /// <summary>
        /// The name of the construction project.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Information about how to display its cost.
        /// </summary>
        public ResourceDisplayInfo Cost { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes an empty summary.
        /// </summary>
        public ConstructionProjectUISummary() { }

        /// <summary>
        /// Initializes a summary with the specified name and costDisplayInfo.
        /// </summary>
        /// <param name="name">The name to send to the UI</param>
        /// <param name="costDisplayInfo">The cost summary info to send to the UI</param>
        public ConstructionProjectUISummary(string name, ResourceDisplayInfo costDisplayInfo) {
            Name = name;
            Cost = costDisplayInfo;
        }

        /// <summary>
        /// Creates a summary from the given construction project.
        /// </summary>
        /// <param name="projectToSummarize">The project the summary should summarize</param>
        public ConstructionProjectUISummary(ConstructionProjectBase projectToSummarize) {
            Name = projectToSummarize.name;
            Cost = projectToSummarize.GetCostInfo();
        }

        #endregion

    }

}
