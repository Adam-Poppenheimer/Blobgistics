using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    /// <summary>
    /// Abstract base class for handling the generation of visible terrain in the form of a hex grid.
    /// This is used in tandem with MapGraphBase to define and display a cohesive map.
    /// </summary>
    public abstract class TerrainGridBase : HexGrid<TerrainHexTile> {

        #region instance fields and properties

        /// <summary>
        /// The radius of the generated hex grid.
        /// </summary>
        public abstract int Radius { get; set; }

        /// <summary>
        /// The maximum distance that a TerrainHexTile can be from a MapNodeBase to be eligible for association.
        /// </summary>
        public abstract float MaxAcquisitionDistance { get; set; }

        /// <summary>
        /// The bounds of the generated TerrainGrid, used to inform the bounds of the CameraLogic.
        /// </summary>
        public abstract Rect Bounds { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Clears the map of all its tiles.
        /// </summary>
        public abstract void ClearMap();

        /// <summary>
        /// Generates a new map with the preconfigured dimensions that has no association data.
        /// </summary>
        public abstract void CreateMap();

        /// <summary>
        /// Establishes association data between TerrainHexTiles and refreshes the appearance of the
        /// map.
        /// </summary>
        public abstract void RefreshMapTerrains();

        #endregion

    }

}
