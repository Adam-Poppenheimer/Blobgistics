using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Session {

    [Serializable]
    public class SerializableMapNodeData {

        #region instance fields and properties

        public int ID;
        
        public Vector3 LocalPosition;

        public TerrainType Terrain;

        #endregion

        #region constructors

        public SerializableMapNodeData(MapNodeBase node) {
            ID = node.ID;
            LocalPosition = node.transform.localPosition;
            Terrain = node.Terrain;
        }

        #endregion

    }

}
