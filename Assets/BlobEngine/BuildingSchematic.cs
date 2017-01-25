using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BuildingSchematic {

        #region instance fields and properties

        public readonly BlobPileCapacity Cost;
        private readonly Action<BuildingPlot> ConstructionAction;

        #endregion

        #region constructors

        public BuildingSchematic(BlobPileCapacity cost, Action<BuildingPlot> constructionAction) {
            Cost = cost;
            ConstructionAction = constructionAction;
        }

        #endregion

        #region instance methods

        public void PerformConstruction(BuildingPlot plotToBuildOn) {
            ConstructionAction(plotToBuildOn);
        }

        #endregion 

    }

}
