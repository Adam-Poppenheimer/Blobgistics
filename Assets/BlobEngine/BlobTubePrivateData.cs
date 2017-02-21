using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BlobTubePrivateData : MonoBehaviour, IBlobTubePrivateData {

        #region instance fields and properties

        public float TransportSpeedPerSecond { get; set; }
        public int Capacity { get; set; }

        #endregion

    }

}
