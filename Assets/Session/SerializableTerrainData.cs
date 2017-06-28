using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using UnityCustomUtilities.Grids;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableTerrainData {

        #region instance fields and properties

        [DataMember()] public HexGridLayout Layout;
        
        [DataMember()] public int Radius;
        [DataMember()] public float MaxTerrainAcquisitionRange;

        #endregion

        #region constructors

        public SerializableTerrainData(TerrainGridBase terrainGrid) {
            Layout = terrainGrid.Layout;
            Radius = terrainGrid.Radius;
            MaxTerrainAcquisitionRange = terrainGrid.MaxAcquisitionDistance;
        }

        #endregion

    }

}
