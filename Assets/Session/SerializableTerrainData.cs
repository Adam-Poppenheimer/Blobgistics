using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using UnityCustomUtilities.Grids;

namespace Assets.Session {

    [Serializable]
    public class SerializableTerrainData {

        #region instance fields and properties

        public HexGridLayout Layout;
        
        public int Radius;
        public float MaxTerrainAcquisitionRange;

        #endregion

        #region constructors

        public SerializableTerrainData(TerrainTileHexGrid tileGrid) {
            Layout = tileGrid.Layout;
            Radius = tileGrid.Radius;
            MaxTerrainAcquisitionRange = tileGrid.MaxAcquisitionDistance;
        }

        #endregion

    }

}
