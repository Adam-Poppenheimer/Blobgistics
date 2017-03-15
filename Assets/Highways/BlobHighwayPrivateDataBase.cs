using System;

using UnityEngine;

using Assets.Core;
using Assets.Map;

using UnityCustomUtilities.UI;

namespace Assets.Highways {

    public abstract class BlobHighwayPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract UIControl UIControl { get; }

        public abstract BlobTubeBase TubePullingFromFirstEndpoint { get; }
        public abstract BlobTubeBase TubePullingFromSecondEndpoint { get; }

        public abstract MapNodeBase FirstEndpoint { get; }
        public abstract MapNodeBase SecondEndpoint { get; }

        #endregion

    }

}