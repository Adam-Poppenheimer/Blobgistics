using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public class Schematic {

        #region instance fields and properties

        public string Name {
            get { return _name; }
        }
        private readonly string _name;

        public BlobPileCapacity Cost {
            get { return _cost; }
        }

        private readonly BlobPileCapacity _cost;

        private readonly Action<MapNode> ConstructionAction;

        #endregion

        #region constructors

        public Schematic(string name, BlobPileCapacity cost, Action<MapNode> constructionAction) {
            _name = name;
            _cost = cost;
            ConstructionAction = constructionAction;
        }

        #endregion

        #region instance methods

        public void PerformConstruction(MapNode locationToConstruct) {
            ConstructionAction(locationToConstruct);
        }

        #endregion 

    }

}
