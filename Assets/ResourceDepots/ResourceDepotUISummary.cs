using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.ResourceDepots {

    /// <summary>
    /// A POD class that provides all the data the UI needs to know about a ResourceDepotBase object.
    /// </summary>
    public class ResourceDepotUISummary {

        #region instance fields and properties

        /// <summary>
        /// The ID that distinguishes between different ResourceDepotBases.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The transform attached to the ResourceDepotBase.
        /// </summary>
        public Transform Transform { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Constructs an empty summary.
        /// </summary>
        public ResourceDepotUISummary() { }

        /// <summary>
        /// Constructs a summary whose information summarizes the given depot.
        /// </summary>
        /// <param name="depotToSummarize">The depot this summary will summarize</param>
        public ResourceDepotUISummary(ResourceDepotBase depotToSummarize) {
            ID = depotToSummarize.ID;
            Transform = depotToSummarize.transform;
        }

        #endregion

    }

}
