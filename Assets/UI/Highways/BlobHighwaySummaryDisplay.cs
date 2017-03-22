using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;
using Assets.Highways;
using Assets.Core;

using UnityCustomUtilities.UI;

namespace Assets.UI.Highways {

    public class BlobHighwaySummaryDisplay : BlobHighwaySummaryDisplayBase {

        #region instance fields and properties

        #region from BlobHighwaySummaryDisplayBase

        public override BlobHighwayUISummary CurrentSummary { get; set; }

        public override bool CanBeUpgraded { get; set; }

        #endregion

        [SerializeField] private InputField PriorityInput;

        [SerializeField] private Toggle FirstEndpointRedPermissionToggle;
        [SerializeField] private Toggle SecondEndpointRedPermissionToggle;

        [SerializeField] private Toggle FirstEndpointGreenPermissionToggle;
        [SerializeField] private Toggle SecondEndpointGreenPermissionToggle;

        [SerializeField] private Toggle FirstEndpointBluePermissionToggle;
        [SerializeField] private Toggle SecondEndpointBluePermissionToggle;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            PriorityInput.onEndEdit.AddListener(delegate(string textInInput) {
                int newPriority;
                Int32.TryParse(textInInput, out newPriority);
                if(newPriority != CurrentSummary.Priority) {
                    RaisePriorityChanged(newPriority);
                }
            });
            FirstEndpointRedPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Red, newPermission);
            });
            FirstEndpointGreenPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Green, newPermission);
            });
            FirstEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Blue, newPermission);
            });

            SecondEndpointRedPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Red, newPermission);
            });
            SecondEndpointGreenPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Green, newPermission);
            });
            SecondEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Blue, newPermission);
            });
        }

        #endregion

        public override void UpdateDisplay() {
            PriorityInput.text = CurrentSummary.Priority.ToString();

            FirstEndpointRedPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Red];
            SecondEndpointRedPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Red];

            FirstEndpointGreenPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Green];
            SecondEndpointGreenPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Green];

            FirstEndpointBluePermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Blue];
            SecondEndpointBluePermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Blue];

            var clickingContext = GetComponent<ClickingContextMenu>();
            if(clickingContext != null) {
                clickingContext.Show();
            }
        }

        public override void ClearDisplay() {
            PriorityInput.text = "";

            FirstEndpointRedPermissionToggle.isOn = false;
            SecondEndpointRedPermissionToggle.isOn = false;

            FirstEndpointGreenPermissionToggle.isOn = false;
            SecondEndpointGreenPermissionToggle.isOn = false;

            FirstEndpointBluePermissionToggle.isOn = false;
            SecondEndpointBluePermissionToggle.isOn = false;
        }

        #endregion

    }

}
