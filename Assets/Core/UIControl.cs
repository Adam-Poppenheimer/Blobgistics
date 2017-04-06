using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

using Assets.Highways;
using Assets.UI.Highways;

using Assets.ConstructionZones;
using Assets.UI.ConstructionZones;

using Assets.HighwayUpgraders;
using Assets.UI.HighwayUpgraders;

using Assets.Societies;
using Assets.UI.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class UIControl : UIControlBase {

        #region instance fields and properties

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

        public BlobHighwayGhostBase HighwayGhost {
            get {
                if(_highwayGhost == null) {
                    throw new InvalidOperationException("HighwayGhost is uninitialized");
                } else {
                    return _highwayGhost;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _highwayGhost = value;
                }
            }
        }
        [SerializeField] private BlobHighwayGhostBase _highwayGhost;

        public BlobHighwaySummaryDisplayBase HighwaySummaryDisplay {
            get { return _highwayDisplay; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_highwayDisplay != null) {
                        _highwayDisplay.FirstEndpointResourcePermissionChanged  -= HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                        _highwayDisplay.SecondEndpointResourcePermissionChanged -= HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                        _highwayDisplay.PriorityChanged                         -= HighwaySummaryDisplay_PriorityChanged;
                        _highwayDisplay.BeginHighwayUpgradeRequested            -= HighwayDisplay_BeginHighwayUpgradeRequested;
                        _highwayDisplay.CancelHighwayUpgradeRequested           -= HighwayDisplay_CancelHighwayUpgradeRequested;
                        
                    }
                    _highwayDisplay = value;
                    if(_highwayDisplay != null) {
                        _highwayDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                        _highwayDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                        _highwayDisplay.PriorityChanged                         += HighwaySummaryDisplay_PriorityChanged;
                        _highwayDisplay.BeginHighwayUpgradeRequested            += HighwayDisplay_BeginHighwayUpgradeRequested;
                        _highwayDisplay.CancelHighwayUpgradeRequested           += HighwayDisplay_CancelHighwayUpgradeRequested;
                    }
                }
            }
        }
        [SerializeField] private BlobHighwaySummaryDisplayBase _highwayDisplay;

        public ConstructionZoneSummaryDisplayBase ConstructionZoneSummaryDisplay {
            get {
                if(_constructionZoneSummaryDisplay == null) {
                    throw new InvalidOperationException("ConstructionZoneDisplay is uninitialized");
                } else {
                    return _constructionZoneSummaryDisplay;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_constructionZoneSummaryDisplay != null) {
                        _constructionZoneSummaryDisplay.ConstructionZoneDestructionRequested -= ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
                        _constructionZoneSummaryDisplay.CloseRequested                       -= ConstructionZoneSummaryDisplay_CloseRequested;
                    }
                    _constructionZoneSummaryDisplay = value;
                    if(_constructionZoneSummaryDisplay != null) {
                        _constructionZoneSummaryDisplay.ConstructionZoneDestructionRequested += ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
                        _constructionZoneSummaryDisplay.CloseRequested                       += ConstructionZoneSummaryDisplay_CloseRequested;
                    }
                }
            }
        }
        [SerializeField] private ConstructionZoneSummaryDisplayBase _constructionZoneSummaryDisplay;

        public SocietyUISummaryDisplayBase SocietySummaryDisplay {
            get {
                if(_societySummaryDisplay == null) {
                    throw new InvalidOperationException("SocietySummaryDisplay is uninitialized");
                } else {
                    return _societySummaryDisplay;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_societySummaryDisplay != null) {
                        _societySummaryDisplay.DestructionRequested -= SocietySummaryDisplay_DestructionRequested;
                    }
                    _societySummaryDisplay = value;
                    if(_societySummaryDisplay != null) {
                        _societySummaryDisplay.DestructionRequested += SocietySummaryDisplay_DestructionRequested;
                    }
                }
            }
        }
        [SerializeField] private SocietyUISummaryDisplayBase _societySummaryDisplay;

        public ConstructionPanelBase ConstructionPanel {
            get {
                if(_constructionPanel == null) {
                    throw new InvalidOperationException("ConstructionPanel is uninitialized");
                } else {
                    return _constructionPanel;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_constructionPanel != null) {
                        _constructionPanel.CloseRequested        -= ConstructionPanel_CloseRequested;
                        _constructionPanel.ConstructionRequested -= ConstructionPanel_ConstructionRequested;
                    }
                    _constructionPanel = value;
                    if(_constructionPanel != null) {
                        _constructionPanel.CloseRequested        += ConstructionPanel_CloseRequested;
                        _constructionPanel.ConstructionRequested += ConstructionPanel_ConstructionRequested;
                    }
                }
            }
        }
        [SerializeField] private ConstructionPanelBase _constructionPanel;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(HighwaySummaryDisplay != null) {
                HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged  -= HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged -= HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.PriorityChanged                         -= HighwaySummaryDisplay_PriorityChanged;
                HighwaySummaryDisplay.BeginHighwayUpgradeRequested            -= HighwayDisplay_BeginHighwayUpgradeRequested;
                HighwaySummaryDisplay.CancelHighwayUpgradeRequested           -= HighwayDisplay_CancelHighwayUpgradeRequested;

                HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.PriorityChanged                         += HighwaySummaryDisplay_PriorityChanged;
                HighwaySummaryDisplay.BeginHighwayUpgradeRequested            += HighwayDisplay_BeginHighwayUpgradeRequested;
                HighwaySummaryDisplay.CancelHighwayUpgradeRequested           += HighwayDisplay_CancelHighwayUpgradeRequested;
            }

            if(ConstructionPanel != null) {
                ConstructionPanel.CloseRequested        -= ConstructionPanel_CloseRequested;
                ConstructionPanel.ConstructionRequested -= ConstructionPanel_ConstructionRequested;

                ConstructionPanel.CloseRequested        += ConstructionPanel_CloseRequested;
                ConstructionPanel.ConstructionRequested += ConstructionPanel_ConstructionRequested;
            }

            if(ConstructionZoneSummaryDisplay != null) {
                ConstructionZoneSummaryDisplay.CloseRequested -= ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested -= ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;

                ConstructionZoneSummaryDisplay.CloseRequested += ConstructionZoneSummaryDisplay_CloseRequested;
                ConstructionZoneSummaryDisplay.ConstructionZoneDestructionRequested += ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested;
            }

            if(SocietySummaryDisplay != null) {
                SocietySummaryDisplay.DestructionRequested -= SocietySummaryDisplay_DestructionRequested;

                SocietySummaryDisplay.DestructionRequested += SocietySummaryDisplay_DestructionRequested;
            }
        }

        #endregion

        #region from UIControlBase

        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {
            if(source is MapNodeUISummary) {
                HighwayGhost.Clear();
                HighwayGhost.FirstEndpoint = source as MapNodeUISummary;
                HighwayGhost.Activate();
            }
        }

        public override void PushDragEvent<T>(T source, PointerEventData eventData) {
            if(source is MapNodeUISummary) {
                HighwayGhost.UpdateWithEventData(eventData);
            }
        }

        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {
            if(source is MapNodeUISummary) {
                var firstEndpoint = HighwayGhost.FirstEndpoint;
                var secondEndpoint = HighwayGhost.SecondEndpoint;

                if( firstEndpoint != null && secondEndpoint != null &&
                    SimulationControl.CanConnectNodesWithHighway(firstEndpoint.ID, secondEndpoint.ID)
                ){
                    SimulationControl.ConnectNodesWithHighway(HighwayGhost.FirstEndpoint.ID, HighwayGhost.SecondEndpoint.ID);
                }
                HighwayGhost.Clear();
                HighwayGhost.Deactivate();
            }
        }

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {}

        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {
            if(source is MapNodeUISummary) {
                if(HighwayGhost.IsActivated) {
                    HighwayGhost.SecondEndpoint = source as MapNodeUISummary;
                    HighwayGhost.GhostIsBuildable = SimulationControl.CanConnectNodesWithHighway(HighwayGhost.FirstEndpoint.ID,
                        HighwayGhost.SecondEndpoint.ID);
                }
            }
        }

        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {
            if(source is MapNodeUISummary) {
                if(HighwayGhost.IsActivated) {
                    HighwayGhost.SecondEndpoint = null;
                    HighwayGhost.GhostIsBuildable = false;
                }
            }
        }

        public override void PushSelectEvent<T>(T source, BaseEventData eventData) {
            if(source is MapNodeUISummary) {
                var summary = source as MapNodeUISummary;
                ConstructionPanel.LocationToConstruct = summary;
                ConstructionPanel.SetPermittedProjects(SimulationControl.GetAllPermittedConstructionZoneProjectsOnNode(summary.ID));
                ConstructionPanel.Activate();
            }else if(source is BlobHighwayUISummary) {
                if(HighwaySummaryDisplay != null) {
                    HighwaySummaryDisplay.CurrentSummary = source as BlobHighwayUISummary;
                    HighwaySummaryDisplay.CanBeUpgraded = SimulationControl.CanCreateHighwayUpgraderOnHighway(
                        HighwaySummaryDisplay.CurrentSummary.ID);
                    HighwaySummaryDisplay.IsBeingUpgraded = SimulationControl.HasHighwayUpgraderOnHighway(
                        HighwaySummaryDisplay.CurrentSummary.ID);
                    HighwaySummaryDisplay.Activate();
                }
            }else if(source is SocietyUISummary) {
                if(SocietySummaryDisplay != null) {
                    SocietySummaryDisplay.CurrentSummary = source as SocietyUISummary;
                    SocietySummaryDisplay.CanBeDestroyed = SimulationControl.CanDestroySociety(SocietySummaryDisplay.CurrentSummary.ID);
                    SocietySummaryDisplay.Activate();
                }
            }else if(source is ConstructionZoneUISummary) {
                if(ConstructionZoneSummaryDisplay != null) {
                    ConstructionZoneSummaryDisplay.Clear();
                    ConstructionZoneSummaryDisplay.CurrentSummary = source as ConstructionZoneUISummary;
                    ConstructionZoneSummaryDisplay.Activate();
                }
            }
        }

        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {}

        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {}

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

        private void ConstructionPanel_ConstructionRequested(object sender, StringEventArgs e) {
            if(SimulationControl.CanCreateConstructionSiteOnNode(ConstructionPanel.LocationToConstruct.ID, e.Value)) {
                SimulationControl.CreateConstructionSiteOnNode(ConstructionPanel.LocationToConstruct.ID, e.Value);
            }
            ConstructionPanel.Clear();
            ConstructionPanel.Deactivate();
        }

        private void ConstructionPanel_CloseRequested(object sender, EventArgs e) {
            ConstructionPanel.Clear();
            ConstructionPanel.Deactivate();
        }

        private void ConstructionZoneSummaryDisplay_CloseRequested(object sender, EventArgs e) {
            ConstructionZoneSummaryDisplay.Clear();
            ConstructionZoneSummaryDisplay.Deactivate();
        }

        private void ConstructionZoneSummaryDisplay_ConstructionZoneDestructionRequested(object sender, EventArgs e) {
            SimulationControl.DestroyConstructionZone(ConstructionZoneSummaryDisplay.CurrentSummary.ID);
            ConstructionZoneSummaryDisplay.Deactivate();
        }

        private void HighwayDisplay_BeginHighwayUpgradeRequested(object sender, EventArgs e) {
            if(SimulationControl.CanCreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID)) {
                SimulationControl.CreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID);
                HighwaySummaryDisplay.IsBeingUpgraded = true;
                EventSystem.current.SetSelectedGameObject(HighwaySummaryDisplay.gameObject);
            }
        }

        private void HighwayDisplay_CancelHighwayUpgradeRequested(object sender, EventArgs e) {
            SimulationControl.DestroyHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID);
            HighwaySummaryDisplay.IsBeingUpgraded = false;
            HighwaySummaryDisplay.CanBeUpgraded = SimulationControl.CanCreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID);
            EventSystem.current.SetSelectedGameObject(HighwaySummaryDisplay.gameObject);
        }

        private void SocietySummaryDisplay_DestructionRequested(object sender, EventArgs e) {
            if(SimulationControl.CanDestroySociety(SocietySummaryDisplay.CurrentSummary.ID)) {
                SimulationControl.DestroySociety(SocietySummaryDisplay.CurrentSummary.ID);
                SocietySummaryDisplay.Deactivate();
            }
        }

        #endregion

    }

}
