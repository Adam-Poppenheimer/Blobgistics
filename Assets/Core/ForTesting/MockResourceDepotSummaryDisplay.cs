using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ResourceDepots;
using Assets.UI.ResourceDepots;

namespace Assets.Core.ForTesting {

    public class MockResourceDepotSummaryDisplay : ResourceDepotSummaryDisplayBase {

        #region instance fields and properties

        #region ResourceDepotSummaryDisplayBase

        public override ResourceDepotUISummary CurrentSummary { get; set; }

        #endregion

        public bool IsActive = false;

        #endregion

        #region instance methods

        #region from ResourceDepotSummaryDisplayBase

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
