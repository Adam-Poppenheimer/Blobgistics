using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;

namespace Assets.UI.ConstructionZones.ForTesting {

    public class MockConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override bool HasPermissionForResourceDepot { get; set; }

        public override bool IsActivated {
            get { return isActivated; }
        }
        private bool isActivated = false;

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        public bool HasBeenCleared = false;

        #endregion

        #region instance methods

        #region from ConstructionPanelBase

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

        public void RaiseCloseRequestedEvent() {
            RaiseCloseRequested();
        }

        public void RaiseResourceDepotConstructionRequest() {
            RaiseDepotConstructionRequested();
        }

        #endregion

    }

}
