using System;

using UnityEngine;

namespace Assets.Map {

    [Serializable]
    public class SaveableNodeSummary {

        #region instance fields and properties

        public int ID;
        public Vector3 LocalPosition;
        public TerrainType Terrain;

        #endregion

        #region constructors

        public SaveableNodeSummary(int id, Vector3 localPosition, TerrainType terrain) {
            ID = id;
            LocalPosition = localPosition;
            Terrain = terrain;
        }

        #endregion

    }

}