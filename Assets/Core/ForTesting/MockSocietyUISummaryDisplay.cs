using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Societies;
using Assets.UI.Societies;

namespace Assets.Core.ForTesting {

    public class MockSocietyUISummaryDisplay : SocietyUISummaryDisplayBase {

        #region instance fields and properties

        #region from SocietyUISummaryDisplayBase

        public override bool CanBeDestroyed { get; set; }

        public override SocietyUISummary CurrentSummary { get; set; }

        #endregion

        #endregion

        #region instance methods

        public void RaiseAscensionPermissionChangeRequestedEvent(bool isPermitted) {
            RaiseAscensionPermissionChangeRequested(isPermitted);
        }

        public void RaiseDestructionRequestedEvent() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
