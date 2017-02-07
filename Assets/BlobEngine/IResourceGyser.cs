using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;
using UnityEngine;

namespace Assets.BlobEngine {

    public interface IResourceGyser : IBuildingPlot {

        #region properties

        ResourceType BlobTypeGenerated { get; }

        #endregion

    }

}
