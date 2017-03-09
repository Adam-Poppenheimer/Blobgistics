using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Highways;

namespace Assets.Highways.ForTesting {

    public class MockBlobTubePrivateData : BlobTubePrivateData {

        #region instance fields and properties

        #region from IBlobTubePrivateData

        public override int Capacity { get; set; }

        public override float TransportSpeedPerSecond { get; set; }

        #endregion

        #endregion

    }

}
