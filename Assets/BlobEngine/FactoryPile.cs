using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class FactoryPile : FactoryPileBase {

        #region instance fields and properties

        public override BlobGeneratorFactoryBase BlobGeneratorFactory {
            get { return _blobGeneratorFactory; }
        }
        [SerializeField] private BlobGeneratorFactoryBase _blobGeneratorFactory;

        public override BuildingPlotFactoryBase BuildingPlotFactory {
            get { return _buildingPlotFactory; }
        }
        [SerializeField] private BuildingPlotFactoryBase _buildingPlotFactory;

        public override ResourcePoolFactoryBase ResourcePoolFactory {
            get { return _resourcePoolFactory; }
        }
        [SerializeField] private ResourcePoolFactoryBase _resourcePoolFactory;

        #endregion

    }

}
