using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Highways {

    public class BlobTubePrivateData : BlobTubePrivateDataBase {

        #region instance fields and properties

        public override float TransportSpeedPerSecond { get; set; }
        public override int Capacity { get; set; }

        #endregion

    }

}
