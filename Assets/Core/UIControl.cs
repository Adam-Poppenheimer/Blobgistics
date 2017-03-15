using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Highways;
using Assets.UI.Highways;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class UIControl : UIControlBase {

        #region instance fields and properties

        public BlobHighwaySummaryDisplayBase HighwaySummaryDisplay {
            get {
                if(_highwayDisplay == null) {
                    throw new InvalidOperationException("HighwayDisplay is uninitialized");
                } else {
                    return _highwayDisplay;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_highwayDisplay != null) {
                        _highwayDisplay.FirstEndpointResourcePermissionChanged  -= HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                        _highwayDisplay.SecondEndpointResourcePermissionChanged -= HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                        _highwayDisplay.PriorityChanged -= HighwaySummaryDisplay_PriorityChanged;
                    }
                    _highwayDisplay = value;
                    _highwayDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                    _highwayDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                    _highwayDisplay.PriorityChanged += HighwaySummaryDisplay_PriorityChanged;
                }
            }
        }
        [SerializeField] private BlobHighwaySummaryDisplayBase _highwayDisplay;

        public SimulationControlBase SimulationControl {
            get {
                if(_simulationControl == null) {
                    throw new InvalidOperationException("SimulationControl is uninitialized");
                } else {
                    return _simulationControl;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _simulationControl = value;
                }
            }
        }
        [SerializeField] private SimulationControlBase _simulationControl;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
            HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
            HighwaySummaryDisplay.PriorityChanged += HighwaySummaryDisplay_PriorityChanged;
        }

        #endregion

        #region from UIControlBase

        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {}

        public override void PushDragEvent<T>(T source, PointerEventData eventData) {}

        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {}

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            if(source is BlobHighwayUISummary) {
                if(HighwaySummaryDisplay != null) {
                    HighwaySummaryDisplay.ClearDisplay();
                    HighwaySummaryDisplay.CurrentSummary = source as BlobHighwayUISummary;
                    HighwaySummaryDisplay.UpdateDisplay();
                }
            }
        }

        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {}

        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {}

        #endregion

        private void HighwaySummaryDisplay_PriorityChanged(object sender, IntEventArgs e) {
            SimulationControl.SetHighwayPriority(HighwaySummaryDisplay.CurrentSummary.ID, e.Value);
        }

        private void HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            SimulationControl.SetHighwayPullingPermissionOnFirstEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        private void HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged(object sender, ResourcePermissionEventArgs e) {
            SimulationControl.SetHighwayPullingPermissionOnSecondEndpointForResource(
                HighwaySummaryDisplay.CurrentSummary.ID, e.TypeChanged, e.IsNowPermitted);
        }

        #endregion

    }

}
