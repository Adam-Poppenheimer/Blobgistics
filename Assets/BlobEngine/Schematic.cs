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

        public Dictionary<ResourceType, int> Cost {
            get { return _cost; }
        }

        private readonly Dictionary<ResourceType, int> _cost;

        private readonly Action<MapNode> ConstructionAction;

        #endregion

        #region constructors

        public Schematic(string name, Dictionary<ResourceType, int> cost, Action<MapNode> constructionAction) {
            _name = name;
            _cost = new Dictionary<ResourceType, int>(cost);
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
