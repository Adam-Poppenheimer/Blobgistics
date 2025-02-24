﻿using System;
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

        #endregion

        #region instance methods

        public void RaiseDestructionRequestedEvent() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
