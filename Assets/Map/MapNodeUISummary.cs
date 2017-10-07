using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    /// <summary>
    /// A class containing information that MapNode should pass into UIControl whenever
    /// it catches user input.
    /// </summary>
    public class MapNodeUISummary {

        #region instance fields and properties

        /// <summary>
        /// The MapNodeBase-unique ID of the underlying MapNodeBase.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// A similarly indirected block of data representing the MapNodeBase's BlobSite.
        /// </summary>
        public BlobSiteUISummary BlobSite { get; set; }

        /// <summary>
        /// The TerrainType of the MapNodeBase.
        /// </summary>
        public TerrainType Terrain { get; set; }

        /// <summary>
        /// the transform of the MapNodeBase.
        /// </summary>
        /// <remarks>
        /// The necessity of properties like this put into question the value of creating such
        /// drastic indirection between the UI and the simulation.
        /// </remarks>
        public Transform Transform { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an empty MapNodeUISummary.
        /// </summary>
        public MapNodeUISummary() { }

        /// <summary>
        /// Creates a MapNodeUISummary that adequately summarizes the specified MapNodeBase.
        /// </summary>
        /// <param name="nodeToSummarize">The MapNodeBase to summarize</param>
        public MapNodeUISummary(MapNodeBase nodeToSummarize) {
            ID = nodeToSummarize.ID;
            BlobSite = new BlobSiteUISummary(nodeToSummarize.BlobSite);
            Terrain = nodeToSummarize.Terrain;
            Transform = nodeToSummarize.transform;
        }

        #endregion

    }

}
