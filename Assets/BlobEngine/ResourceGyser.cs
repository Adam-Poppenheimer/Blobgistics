using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public class ResourceGyser : BuildingPlot, IResourceGyser {

        #region instance fields and properties

        #region from IResourceGyser

        public ResourceType BlobTypeGenerated {
            get { return _resourceTypeGenerated; }
        }
        [SerializeField] private ResourceType _resourceTypeGenerated;

        #endregion

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Initialize();
            RealignToDimensions();
        }

        #endregion

        #endregion

    }

}
