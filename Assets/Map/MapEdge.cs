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

        public float Weight {
            get { return _weight; }
        }
        [SerializeField] private float _weight;

        #endregion

        #region constructors

        public MapEdge(MapNode firstNode, MapNode secondNode, float weight) {
            _firstNode = firstNode;
            _secondNode = secondNode;
            _weight = weight;
        }

        #endregion

    }

}
