using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.Depots {

    public abstract class ResourceDepotBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapNodeBase Location { get; }

        public abstract ResourceDepotProfile Profile { get; set; }

        #endregion

        #region instance methods

        public abstract void Clear();

        #endregion

    }

}
