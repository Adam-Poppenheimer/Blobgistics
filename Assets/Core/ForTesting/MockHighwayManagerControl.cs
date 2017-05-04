using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Core.ForTesting {

    public class MockHighwayManagerControl : HighwayManagerControlBase {

        #region events

        public event Action<int> DestroyHighwayManagerOfIDCalled;

        #endregion

        #region instance methods

        #region from HighwayManagerControlBase

        public override void DestroyHighwayManagerOfID(int managerID) {
            if(DestroyHighwayManagerOfIDCalled != null) {
                DestroyHighwayManagerOfIDCalled(managerID);
            }
        }

        #endregion

        #endregion
        
    }

}
