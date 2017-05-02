using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;
using Assets.Map;
using Assets.UI.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        #endregion

        #region events



        #endregion

        #region instance methods

        #region from ConstructionPanelBase

        public override void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
