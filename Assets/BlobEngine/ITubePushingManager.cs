using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BlobEngine {

    public interface ITubePushingManager {

        #region methods

        IEnumerable<IBlobHighway> GetHighwaysConnectedToSite(IBlobSite site);

        IEnumerable<IBlobHighway> GetHighestPriorityHighwaysConnectedToSite(IBlobSite site, ResourceType resourceConsidered,
            int withLowerPriorityThan = -1);

        void PerformDistributionOnSite(IBlobSite site);

        #endregion

    }

}
