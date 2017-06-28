using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.Core {

    public abstract class HighwayManagerControlBase : MonoBehaviour {

        #region instance methods

        public abstract void DestroyHighwayManagerOfID(int managerID);

        public abstract IEnumerable<BlobHighwayUISummary> GetHighwaysManagedByManagerOfID(int managerID);

        #endregion

    }

}
