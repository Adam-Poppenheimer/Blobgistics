using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core {

    public abstract class ResourceDepotControlBase : MonoBehaviour {

        #region instance methods

        public abstract void DestroyResourceDepotOfID(int depotID);

        #endregion

    }

}
