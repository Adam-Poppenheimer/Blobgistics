using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones {

    public abstract class ConstructionZoneBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapNodeBase Location { get; }

        public abstract ConstructionProjectBase CurrentProject { get; set; }

        #endregion

    }

}
