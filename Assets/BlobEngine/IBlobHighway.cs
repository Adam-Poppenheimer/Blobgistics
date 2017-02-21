using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BlobEngine {

    public interface IBlobHighway {

        #region properties

        IBlobSite FirstEndpoint  { get; }
        IBlobSite SecondEndpoint { get; }

        IBlobTube TubePullingFromFirstEndpoint  { get; }
        IBlobTube TubePullingFromSecondEndpoint { get; }

        ITubePermissionLogic FirstTubePermissionLogic  { get; }
        ITubePermissionLogic SecondTubePermissionLogic { get; }

        #endregion

        #region methods

        void SetEndpoints(IBlobSite firstEndpoint, IBlobSite secondEndpoint);

        #endregion

    }

}
