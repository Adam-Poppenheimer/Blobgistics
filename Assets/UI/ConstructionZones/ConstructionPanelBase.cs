using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.ConstructionZones;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.ConstructionZones {

    public abstract class ConstructionPanelBase : IntelligentPanel {

        #region instance fields and properties

        public abstract MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        #region events

        public event EventHandler<StringEventArgs> ConstructionRequested;
        

        protected void RaiseConstructionRequested(string projectName) { RaiseEvent(ConstructionRequested, new StringEventArgs(projectName)); }

        #endregion

        #region instance methods

        public abstract void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects);

        #endregion

    }

}
