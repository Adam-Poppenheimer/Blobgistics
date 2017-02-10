using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BlobTubeFactoryBase : MonoBehaviour{

        #region instance methods

        public abstract IEnumerable<IBlobSite> GetSitesTubedToSite(IBlobSite site);

        public abstract bool TubeExistsBetweenSites(IBlobSite site1, IBlobSite site2);

        public abstract bool     CanBuildTubeBetween(IBlobSite site1, IBlobSite site2);
        public abstract BlobTube BuildTubeBetween   (IBlobSite site1, IBlobSite site2);

        public abstract void DestroyAllTubesConnectingTo(IBlobSite site);

        #endregion

    }

}
