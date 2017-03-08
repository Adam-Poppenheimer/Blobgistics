using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    public abstract class MapNodeBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapGraphBase ManagingGraph { get; }

        public abstract BlobSiteBase BlobSite { get; }

        #endregion

    }

}
