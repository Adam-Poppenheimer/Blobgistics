using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Core.ForTesting {

    public class MockResourceDepotControl : ResourceDepotControlBase {

        #region events

        public event Action<int> DestroyResourceDepotOfIDCalled;

        #endregion

        #region instance methods

        #region from ResourceDepotControlBase

        public override void DestroyResourceDepotOfID(int depotID) {
            if(DestroyResourceDepotOfIDCalled != null) {
                DestroyResourceDepotOfIDCalled(depotID);
            }
        }

        #endregion

        #endregion
        
    }

}
