using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Map;
using Assets.ConstructionZones;
using Assets.UI.Blobs;

namespace Assets.UI.ConstructionZones {

    /// <summary>
    /// The standard implementation of ConstructionPanelBase. This gives the player the ability
    /// to select various construction projects and build construction zones on them.
    /// </summary>
    public class ConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        /// <inheritdoc/>
        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private List<Button> ConstructionProjectButtons = new List<Button>();
        
        #endregion

        #region instance methods

        #region Unity event methods

        //Since delegate assignments to events don't persist between runtimes, it's necessary to reattach
        //our event handlers whenever the program starts. This may be an indication that the class shouldn't
        //use C# events, instead opting to use Unity's EventSystem to handle all eventing.
        private void Start() {
            foreach(var projectButton in ConstructionProjectButtons) {
                if(projectButton != null) {
                    var cachedButtonName = projectButton.name;
                    projectButton.onClick.AddListener(delegate() {
                        RaiseConstructionRequested(cachedButtonName);
                    });
                }
            }
            MovePanelWithCamera = true;
        }

        #endregion

        #region from IntelligentPanel

        /// <inheritdoc/>
        protected override void DoOnActivate() {
            if(LocationToConstruct != null) {
                DesiredWorldPosition = LocationToConstruct.Transform.position;
            }
        }

        /// <inheritdoc/>
        public override void ClearDisplay() {
            LocationToConstruct = null;
        }

        #endregion

        #region from ConstructionPanelBase

        /// <inheritdoc/>
        /// <remarks>
        /// This method requires a few things to work. For a project to be buildable, there
        /// needs to exist a project button with the same name as that project in the invariant
        /// culture. In order to display the cost of that project, that button must also
        /// share a transform with a ResourceDisplayBase component.
        /// </remarks>
        public override void SetPermittedProjects(IEnumerable<ConstructionProjectUISummary> permittedProjects) {
            foreach(var projectButton in ConstructionProjectButtons) {
                var sameNamedProject = permittedProjects.Where(
                    project => project.Name.Equals(projectButton.name, StringComparison.InvariantCultureIgnoreCase)
                ).FirstOrDefault();
                if(sameNamedProject != null) {
                    projectButton.interactable = true;
                    var displayOnButton = projectButton.GetComponentInChildren<ResourceDisplayBase>();
                    if(displayOnButton != null) {
                        displayOnButton.PushAndDisplayInfo(sameNamedProject.Cost);
                    }
                }else {
                    projectButton.interactable = false;
                }
            }
        }

        #endregion

        #endregion

    }
}
