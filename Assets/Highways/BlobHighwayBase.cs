using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;
using Assets.Core;

namespace Assets.Highways {

    public abstract class BlobHighwayBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract BlobHighwayProfile Profile { get; set; }

        public abstract BlobHighwayFactoryBase  ParentFactory { get; set; }
        public abstract UIControlBase           UIControl     { get; set; }
        public abstract ResourceBlobFactoryBase BlobFactory   { get; set; }

        public abstract MapNodeBase FirstEndpoint  { get; } 
        public abstract MapNodeBase SecondEndpoint { get; }

        public abstract ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromFirstEndpoint  { get; }
        public abstract ReadOnlyCollection<ResourceBlobBase> ContentsPulledFromSecondEndpoint { get; }

        public abstract int Priority { get; set; }

        public abstract float Efficiency { get; set; }

        public abstract float BlobPullCooldownInSeconds { get; }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return string.Format("Highway {0} [{1} <--> {2}]", ID, FirstEndpoint, SecondEndpoint);
        }

        #endregion

        public abstract void SetEndpoints(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract bool GetPullingPermissionForFirstEndpoint(ResourceType type);
        public abstract void SetPullingPermissionForFirstEndpoint(ResourceType type, bool isPermitted);

        public abstract bool GetPullingPermissionForSecondEndpoint(ResourceType type);
        public abstract void SetPullingPermissionForSecondEndpoint(ResourceType type, bool isPermitted);

        public abstract bool CanPullFromFirstEndpoint();
        public abstract void PullFromFirstEndpoint();

        public abstract bool CanPullFromSecondEndpoint();
        public abstract void PullFromSecondEndpoint();

        public abstract bool GetUpkeepRequestedForResource(ResourceType type);
        public abstract void SetUpkeepRequestedForResource(ResourceType type, bool isBeingRequested);

        public abstract void Clear();

        #endregion

    }

}
