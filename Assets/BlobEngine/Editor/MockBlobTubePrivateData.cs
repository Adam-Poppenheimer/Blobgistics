using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

namespace Assets.BlobEngine.Editor {

    class MockBlobTubePrivateData : IBlobTubePrivateData {

        #region instance fields and properties

        #region from IBlobTubePrivateData

        public int Capacity { get; set; }

        public float TransportSpeedPerSecond { get; set; }

        #endregion

        #endregion

    }

}
