using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BlobEngine {

    public interface IResourceGyser : IBuildingPlot {

        #region properties

        ResourceType BlobTypeGenerated { get; }
        Transform Transform { get; }

        #endregion

    }

}
