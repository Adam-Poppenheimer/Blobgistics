using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;
using Assets.Map;

namespace Assets.UI.ConstructionZones.ForTesting {

    public class MockConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        public bool HasBeenCleared = false;
        public IEnumerable<ConstructionProjectUISummary> LastPermissionsSet;

        #endregion

        #region instance methods

        #region from ConstructionPanelBase

        public override void ClearDisplay() {
            HasBeenCleared = true;
        }

        #endregion

        public void RaiseCloseRequestedEvent() {
            RaiseDeactivationRequested();
        }

        public void RaiseConstructionRequest(string projectName) {
            RaiseConstructionRequested(projectName);
        }

        public override void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects) {
            LastPermissionsSet = permittedProjects;
        }

        #endregion

    }

}
