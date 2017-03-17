using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

using Assets.Blobs;

namespace Assets.Highways {

    public abstract class BlobHighwayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapNodeBase FirstEndpoint  { get; } 
        public abstract MapNodeBase SecondEndpoint { get; }

        public abstract ReadOnlyCollection<ResourceBlob> ContentsPulledFromFirstEndpoint { get; }
        public abstract ReadOnlyCollection<ResourceBlob> ContentsPulledFromSecondEndpoint { get; }

        public abstract BlobHighwayProfile Profile { get; set; }

        public abstract int Priority { get; set; }

        #endregion

        #region instance methods methods

        public abstract bool GetPullingPermissionForFirstEndpoint(ResourceType type);
        public abstract void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted);

        public abstract bool GetPullingPermissionForSecondEndpoint(ResourceType type);
        public abstract void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted);

        public abstract bool CanPullFromFirstEndpoint();
        public abstract void PullFromFirstEndpoint();

        public abstract bool CanPullFromSecondEndpoint();
        public abstract void PullFromSecondEndpoint();

        public abstract void Clear();

        public abstract void TickMovement(float secondsPassed);

        #endregion

    }

}
