using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;

namespace Assets.UI.ConstructionZones.ForTesting {

    public class MockConstructionZoneDisplaySummary : ConstructionZoneSummaryDisplayBase {

        #region instance fields and properties

        #region from ConstructionZoneSummaryDisplayBase

        public override bool IsActivated {
            get { return isActivated; }
        }
        private bool isActivated = false;

        public override ConstructionZoneUISummary CurrentSummary { get; set; }

        #endregion

        public bool HasBeenCleared { get; set; }

        #endregion

        #region instance methods

        #region from ConstructionZoneSummaryDisplayBase

        public override void Activate() {
            isActivated = true;
        }

        public override void Clear() {
            HasBeenCleared = true;
        }

        public override void Deactivate() {
            isActivated = false;
        }

        public override void UpdateDisplay() {
            throw new NotImplementedException();
        }

        #endregion

        public void GenerateCloseRequest() {
            RaiseCloseRequested();
        }

        public void GenerateDestructionRequest() {
            RaiseConstructionZoneDestructionRequested();
        }

        #endregion

    }

}
