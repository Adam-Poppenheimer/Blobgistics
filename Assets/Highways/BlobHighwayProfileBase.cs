using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    public abstract class BlobHighwayProfileBase : MonoBehaviour {

        #region instance fields and properties

        public abstract float BlobSpeedPerSecond { get; }

        public abstract int Capacity { get; }

        public abstract float BlobPullCooldownInSeconds { get; }

        #endregion

    }

}
