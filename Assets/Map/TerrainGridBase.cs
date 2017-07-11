using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    public abstract class TerrainGridBase : HexGrid<TerrainHexTile> {

        #region instance fields and properties

        public abstract int Radius { get; set; }

        public abstract float MaxAcquisitionDistance { get; set; }

        public abstract Rect Bounds { get; }

        #endregion

        #region instance methods

        public abstract void ClearMap();
        public abstract void CreateMap();

        public abstract void RefreshMapTerrains();

        #endregion

    }

}
