using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using UnityCustomUtilities.Grids;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about the terrain grid.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableTerrainData {

        #region instance fields and properties

        /// <summary>
        /// The layout the grid was operating under.
        /// </summary>
        [DataMember()] public HexGridLayout Layout;
        
        /// <summary>
        /// The radius, in hexes, of the grid.
        /// </summary>
        [DataMember()] public int Radius;

        /// <summary>
        /// The max terrain acquisition range of the grid.
        /// </summary>
        [DataMember()] public float MaxTerrainAcquisitionRange;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given terrain grid.
        /// </summary>
        /// <param name="terrainGrid">The terrain grid to pull data from.</param>
        public SerializableTerrainData(TerrainGridBase terrainGrid) {
            Layout = terrainGrid.Layout;
            Radius = terrainGrid.Radius;
            MaxTerrainAcquisitionRange = terrainGrid.MaxAcquisitionDistance;
        }

        #endregion

    }

}
