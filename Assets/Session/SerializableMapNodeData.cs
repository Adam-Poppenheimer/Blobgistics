using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    [Serializable]
    public class SerializableMapNodeData {

        #region instance fields and properties

        public int ID;
        
        public SerializableVector3 LocalPosition;

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
