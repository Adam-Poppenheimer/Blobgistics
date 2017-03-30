using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones {

    public abstract class ConstructionProjectBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ResourceSummary Cost { get; }

        #endregion

        #region instance methods

        public abstract void ExecuteBuild(MapNodeBase location);

        #endregion

    }

}
