using System;

namespace Assets.BlobEngine {

    public interface IBlobTubePrivateData {

        #region properties

        int Capacity { get; }
        float TransportSpeedPerSecond { get; }

        #endregion

    }

}