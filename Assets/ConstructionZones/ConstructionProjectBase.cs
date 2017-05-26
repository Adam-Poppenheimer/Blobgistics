using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    public abstract class ConstructionProjectBase : MonoBehaviour {

        #region instance methods

        public abstract bool IsValidAtLocation(MapNodeBase location);

        public abstract void ExecuteBuild(MapNodeBase location);
        public abstract void SetSiteForProject(BlobSiteBase site);
        public abstract bool BlobSiteContainsNecessaryResources(BlobSiteBase site);

        public abstract ResourceDisplayInfo GetCostInfo();

        #endregion

    }

}
