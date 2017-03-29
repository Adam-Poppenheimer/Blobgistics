using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Highways;
using Assets.Core;

using UnityCustomUtilities.UI;

namespace Assets.UI.Highways {

    public class BlobHighwaySummaryDisplay : BlobHighwaySummaryDisplayBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from BlobHighwaySummaryDisplayBase

        public override BlobHighwayUISummary CurrentSummary {
            get { return _currentSummary; }
            set {
                _currentSummary = value;
                if(_currentSummary != null) {
                    FirstEndpointRedPermissionToggle.isOn   = _currentSummary.ResourcePermissionsForEndpoint1[ResourceType.Food  ];
                    FirstEndpointGreenPermissionToggle.isOn = _currentSummary.ResourcePermissionsForEndpoint1[ResourceType.Yellow];
                    FirstEndpointBluePermissionToggle.isOn  = _currentSummary.ResourcePermissionsForEndpoint1[ResourceType.White ];

                    SecondEndpointRedPermissionToggle.isOn   = _currentSummary.ResourcePermissionsForEndpoint2[ResourceType.Food  ];
                    SecondEndpointGreenPermissionToggle.isOn = _currentSummary.ResourcePermissionsForEndpoint2[ResourceType.Yellow];
                    SecondEndpointBluePermissionToggle.isOn  = _currentSummary.ResourcePermissionsForEndpoint2[ResourceType.White ];
                }
            }
        }
        private BlobHighwayUISummary _currentSummary;

        public override bool CanBeUpgraded { get; set; }

        #endregion

        [SerializeField] private InputField PriorityInput;

        [SerializeField] private Toggle FirstEndpointRedPermissionToggle;
        [SerializeField] private Toggle FirstEndpointGreenPermissionToggle;
        [SerializeField] private Toggle FirstEndpointBluePermissionToggle;
        
        [SerializeField] private Toggle SecondEndpointRedPermissionToggle;
        [SerializeField] private Toggle SecondEndpointGreenPermissionToggle;
        [SerializeField] private Toggle SecondEndpointBluePermissionToggle;

        private bool DeactivateOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
            }
        }

        #endregion

        #region Unity EventSystem interfaces

        public void OnSelect(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void OnDeselect(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        #endregion

        #region from BlobHighwaySummaryDisplayBase

        public override void Activate() {
            gameObject.SetActive(true);
            UpdateDisplay();
            
            PriorityInput.onEndEdit.AddListener(delegate(string value) {
                int newPriority;
                if(Int32.TryParse(value, out newPriority)) {
                    RaisePriorityChanged(newPriority);
                }else {
                    PriorityInput.text = CurrentSummary.ToString();
                }
            });

            FirstEndpointRedPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Food, newPermission);
            });
            FirstEndpointGreenPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.Yellow, newPermission);
            });
            FirstEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseFirstEndpointPermissionChanged(ResourceType.White, newPermission);
            });

            SecondEndpointRedPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Food, newPermission);
            });
            SecondEndpointGreenPermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.Yellow, newPermission);
            });
            SecondEndpointBluePermissionToggle.onValueChanged.AddListener(delegate(bool newPermission) {
                RaiseSecondEndpointPermissionChanged(ResourceType.White, newPermission);
            });

            StartCoroutine(ReselectToThis());
        }

        public override void Deactivate() {
            gameObject.SetActive(false);

            PriorityInput.onEndEdit.RemoveAllListeners();

            FirstEndpointRedPermissionToggle.onValueChanged.RemoveAllListeners();
            FirstEndpointGreenPermissionToggle.onValueChanged.RemoveAllListeners();
            FirstEndpointBluePermissionToggle.onValueChanged.RemoveAllListeners();

            SecondEndpointRedPermissionToggle.onValueChanged.RemoveAllListeners();
            SecondEndpointGreenPermissionToggle.onValueChanged.RemoveAllListeners();
            SecondEndpointBluePermissionToggle.onValueChanged.RemoveAllListeners();

            ClearDisplay();
        }

        public override void UpdateDisplay() {
            PriorityInput.text = CurrentSummary.Priority.ToString();

            FirstEndpointRedPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Food];
            FirstEndpointGreenPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.Yellow];
            FirstEndpointBluePermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint1[ResourceType.White];

            SecondEndpointRedPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Food];
            SecondEndpointGreenPermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.Yellow];
            SecondEndpointBluePermissionToggle.isOn = CurrentSummary.ResourcePermissionsForEndpoint2[ResourceType.White];
        }

        public override void ClearDisplay() {
            CurrentSummary = null;

            PriorityInput.text = "0";

            FirstEndpointRedPermissionToggle.isOn = false;
            FirstEndpointGreenPermissionToggle.isOn = false;
            FirstEndpointBluePermissionToggle.isOn = false;

            SecondEndpointRedPermissionToggle.isOn = false;            
            SecondEndpointGreenPermissionToggle.isOn = false;            
            SecondEndpointBluePermissionToggle.isOn = false;
        }

        #endregion

        public void DoOnChildSelected(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void DoOnChildDeselected(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        private IEnumerator ReselectToThis() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        #endregion

    }

}
