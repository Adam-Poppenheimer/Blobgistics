using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class ResourceGyser : BuildingPlot, IResourceGyser {

        #region instance fields and properties

        #region from IResourceGyser

        public ResourceType BlobTypeGenerated {
            get { return _resourceTypeGenerated; }
        }

        public Transform Transform {
            get { return transform; }
        }

        [SerializeField] private ResourceType _resourceTypeGenerated;

        #endregion

        #endregion

    }

}
