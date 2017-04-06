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

        public override bool IsActivated {
            get { return isActivated; }
        }
        private bool isActivated = false;

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        public bool HasBeenCleared = false;
        public IEnumerable<ConstructionProjectUISummary> LastPermissionsSet;

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

        public void RaiseConstructionRequest(string projectName) {
            RaiseConstructionRequested(projectName);
        }

        public override void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects) {
            LastPermissionsSet = permittedProjects;
        }

        #endregion

    }

}
