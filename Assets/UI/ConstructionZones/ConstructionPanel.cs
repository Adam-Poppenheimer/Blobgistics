using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Map;
using Assets.ConstructionZones;
using Assets.UI.Blobs;

namespace Assets.UI.ConstructionZones {

    public class ConstructionPanel : ConstructionPanelBase {

        #region instance fields and properties

        #region from ConstructionPanelBase

        public override MapNodeUISummary LocationToConstruct { get; set; }

        #endregion

        [SerializeField] private List<Button> ConstructionProjectButtons = new List<Button>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
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

        protected override void DoOnActivate() {
            if(LocationToConstruct != null) {
                DesiredWorldPosition = LocationToConstruct.Transform.position;
            }
        }

        public override void ClearDisplay() {
            LocationToConstruct = null;
        }

        #endregion

        #region from ConstructionPanelBase

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
