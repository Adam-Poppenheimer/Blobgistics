using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;
using Assets.UI.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockConstructionZoneSummaryDisplay : ConstructionZoneSummaryDisplayBase {  

        #region instance fields and properties

        #region fron ConstructionZoneSummaryDisplayBase

        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        #endregion

        #region instance methods

        public void RaiseDestructionRequestedEvent() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
