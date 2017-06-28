using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Highways;

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

        public override IEnumerable<BlobHighwayUISummary> GetHighwaysManagedByManagerOfID(int managerID) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
