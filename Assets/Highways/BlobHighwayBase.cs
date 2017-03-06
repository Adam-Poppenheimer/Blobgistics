using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

using Assets.Blobs;

namespace Assets.Highways {

    public abstract class BlobHighwayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract BlobSiteBase FirstEndpoint { get; } 
        public abstract BlobSiteBase SecondEndpoint { get; }

        public abstract BlobHighwayProfile Profile { get; set; }

        public abstract int Priority { get; set; }

        #endregion

        #region instance methods methods

        public abstract bool GetPermissionForEndpoint1(ResourceType type);
        public abstract void SetPermissionForEndpoint1(ResourceType type, bool isPermitted);

        public abstract bool GetPermissionForEndpoint2(ResourceType type);
        public abstract void SetPermissionForEndpoint2(ResourceType type, bool isPermitted);

        public abstract bool CanPullFromFirstEndpoint();
        public abstract void PullFromFirstEndpoint();

        public abstract bool CanPullFromSecondEndpoint();
        public abstract void PullFromSecondEndpoint();

        public abstract void TickMovement(float secondsPassed);

        #endregion

    }

}
