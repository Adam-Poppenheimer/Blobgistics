using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    public abstract class MapNodeBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapGraphBase ParentGraph { get; }

        public abstract BlobSiteBase BlobSite { get; }

        public abstract IEnumerable<MapNodeBase> Neighbors { get; }

        public abstract TerrainType Terrain { get; set; }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return "MapNodeBase " + name;
        }

        #endregion

        #endregion

    }

}
