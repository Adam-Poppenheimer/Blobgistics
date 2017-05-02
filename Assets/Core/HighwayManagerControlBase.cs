using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core {

    public abstract class HighwayManagerControlBase : MonoBehaviour {

        #region instance fields and properties

        public abstract void DestroyHighwayManagerOfID(int managerID);

        #endregion

    }

}
