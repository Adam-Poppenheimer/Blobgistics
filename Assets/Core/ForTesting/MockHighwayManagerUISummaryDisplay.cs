using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.HighwayManager;
using Assets.UI.HighwayManager;

namespace Assets.Core.ForTesting {

    public class MockHighwayManagerUISummaryDisplay : HighwayManagerSummaryDisplayBase {

        #region instance fields and properties

        #region from HighwayManagerUISummaryDisplayBase

        public override HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion

        #endregion

        #region instance methods

        public void RaiseDestructionRequestedEvent() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
