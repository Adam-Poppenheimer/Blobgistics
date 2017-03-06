using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [Serializable]
    public struct MapEdge {

        #region instance fields and properties

        public MapNode FirstNode {
            get { return _firstNode; }
        }
        [SerializeField] private MapNode _firstNode;

        public MapNode SecondNode {
            get { return _secondNode; }
        }
        [SerializeField] private MapNode _secondNode;

        #endregion

        #region constructors

        public MapEdge(MapNode firstNode, MapNode secondNode) {
            _firstNode = firstNode;
            _secondNode = secondNode;
        }

        #endregion

    }

}
