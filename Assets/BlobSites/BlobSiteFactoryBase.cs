using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobSites {

    public abstract class BlobSiteFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract BlobSiteBase ConstructBlobSite(GameObject hostingObject);

        #endregion

    }

}
