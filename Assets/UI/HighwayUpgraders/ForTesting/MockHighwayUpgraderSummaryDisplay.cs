using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.HighwayUpgraders;

namespace Assets.UI.HighwayUpgraders.ForTesting {

    public class MockHighwayUpgraderSummaryDisplay : HighwayUpgraderSummaryDisplayBase {

        #region instance fields and properties

        #region from HighwayUpgraderSummaryDisplayBase

        public override bool IsActivated {
            get { return isActivated; }
        }
        private bool isActivated;

        public override HighwayUpgraderUISummary SummaryToDisplay { get; set; }

        #endregion

        public bool HasBeenCleared = false;

        #endregion

        #region instance methods

        #region from HighwayUpgraderSummaryDisplayBase

        public override void Activate() {
            isActivated = true;
        }

        public override void Clear() {
            HasBeenCleared = true;
        }

        public override void Deactivate() {
            isActivated = false;
        }

        #endregion

        public void GenerateDestructionRequest() {
            RaiseUpgraderDestructionRequested();
        }

        public void GenerateCloseRequest() {
            RaiseDisplayCloseRequested();
        }

        #endregion

    }

}
