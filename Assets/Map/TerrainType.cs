using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Assets.Map {

    /// <summary>
    /// Declares the terrain types that a map node or a terrain tile can have.
    /// </summary>
    public enum TerrainType {
        /// <summary/>
        Desert = 0,
        /// <summary/>
        Forest = 1,
        /// <summary/>
        Grassland = 2,
        /// <summary/>
        Mountains = 3,
        /// <summary/>
        Water = 4
    }

}
