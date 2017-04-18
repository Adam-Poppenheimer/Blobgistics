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

        public bool IsActive = false;

        #endregion

        #region instance methods

        #region from HighwayManagerUISummaryDisplayBase

        public override void Activate() {
            IsActive = true;
        }

        public override void ClearDisplay() {
            throw new NotImplementedException();
        }

        public override void Deactivate() {
            IsActive = false;
        }

        public override void UpdateDisplay() {
            throw new NotImplementedException();
        }

        #endregion

        public void RaiseDestructionRequestedEvent() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
