using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones.ForTesting {

    public class MockConstructionZoneDisplaySummary : ConstructionZoneSummaryDisplayBase {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        public bool HasBeenCleared { get; set; }

        #endregion

        #region instance methods

        #region from ConstructionZoneSummaryDisplayBase

        public override void ClearDisplay() {
            HasBeenCleared = true;
        }

        #endregion

        public void GenerateDeactivationRequest() {
            RaiseDeactivationRequested();
        }

        public void GenerateDestructionRequest() {
            RaiseDestructionRequested();
        }

        #endregion

    }

}
