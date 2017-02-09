using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Mobs;

namespace Assets.BlobEngine {

    public abstract class FactoryPileBase : MonoBehaviour {

        #region instance fields and properties

        public abstract BlobGeneratorFactoryBase   BlobGeneratorFactory  { get; }
        public abstract BlobletBarracksFactoryBase BlobletBarracksFactory{ get; }
        public abstract BuildingPlotFactoryBase    BuildingPlotFactory   { get; }
        public abstract ResourcePoolFactoryBase    ResourcePoolFactory   { get; }

        #endregion

    }

}
