using System;

using UnityEngine;

namespace Assets.Highways {

    public abstract class BlobTubePrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract float TransportSpeedPerSecond { get; set; }
        public  abstract int Capacity { get; set; }

        #endregion

    }

}