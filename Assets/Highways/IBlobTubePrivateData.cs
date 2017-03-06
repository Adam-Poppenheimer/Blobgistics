using System;

namespace Assets.Blobs {

    public abstract class BlobTubePrivateDataBase {

        #region instance fields and properties

        public abstract int Capacity { get; }
        public abstract float TransportSpeedPerSecond { get; }

        #endregion

    }

}