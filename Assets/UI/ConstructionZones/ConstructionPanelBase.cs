using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.ConstructionZones;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.ConstructionZones {

    /// <summary>
    /// The abstract base class for construction panels, which give the player the ability
    /// to select various construction projects and build construction zones on them.
    /// </summary>
    public abstract class ConstructionPanelBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The location the panel is currently considering for construction.
        /// </summary>
        public abstract MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests construction of a certain project.
        /// </summary>
        public event EventHandler<StringEventArgs> ConstructionRequested;        

        /// <summary>
        /// Fires the ConstructionRequested event.
        /// </summary>
        /// <param name="projectName">The name of the construction project to build a construction zone for</param>
        protected void RaiseConstructionRequested(string projectName) { RaiseEvent(ConstructionRequested, new StringEventArgs(projectName)); }

        #endregion

        #region instance methods

        /// <summary>
        /// Sets which projects are permitted for construction, and thus which player actions
        /// are valid.
        /// </summary>
        /// <param name="permittedProjects">Summaries of all the valid projects</param>
        public abstract void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects);

        #endregion

    }

}
