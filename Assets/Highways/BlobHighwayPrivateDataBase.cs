using System;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;
using Assets.Map;

namespace Assets.Highways {

    public abstract class BlobHighwayPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract UIControlBase UIControl { get; }
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        public abstract BlobTubeBase TubePullingFromFirstEndpoint { get; }
        public abstract BlobTubeBase TubePullingFromSecondEndpoint { get; }

        public abstract MapNodeBase FirstEndpoint { get; }
        public abstract MapNodeBase SecondEndpoint { get; }

        public abstract BlobHighwayProfile Profile { get; }

        #endregion

    }

}