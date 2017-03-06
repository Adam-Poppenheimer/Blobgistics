using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobSites {

    public abstract class BlobSitePrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract Vector3 NorthConnectionOffset { get; }
        public abstract Vector3 SouthConnectionOffset { get; }
        public abstract Vector3 EastConnectionOffset  { get; }
        public abstract Vector3 WestConnectionOffset  { get; }

        #endregion

    }

}
