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
                        _highwayDisplay.HighwayUpgradeRequested                 -= HighwayDisplay_HighwayUpgradeRequested;
                        
                    }
                    _highwayDisplay = value;
                    if(_highwayDisplay != null) {
                        _highwayDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                        _highwayDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                        _highwayDisplay.PriorityChanged                         += HighwaySummaryDisplay_PriorityChanged;
                        _highwayDisplay.HighwayUpgradeRequested                 += HighwayDisplay_HighwayUpgradeRequested;
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

        public HighwayUpgraderSummaryDisplayBase HighwayUpgraderSummaryDisplay {
            get {
                if(_highwayUpgraderSummaryDisplay == null) {
                    throw new InvalidOperationException("HighwayUpgraderSummaryDisplay is uninitialized");
                } else {
                    return _highwayUpgraderSummaryDisplay;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    if(_highwayUpgraderSummaryDisplay != null) {
                        _highwayUpgraderSummaryDisplay.UpgraderDestructionRequested -= HighwayUpgraderSummaryDisplay_UpgraderDestructionRequested;
                        _highwayUpgraderSummaryDisplay.DisplayCloseRequested        -= HighwayUpgraderSummaryDisplay_DisplayCloseRequested;
                    }
                    _highwayUpgraderSummaryDisplay = value;
                    if(_highwayUpgraderSummaryDisplay != null) {
                        _highwayUpgraderSummaryDisplay.UpgraderDestructionRequested += HighwayUpgraderSummaryDisplay_UpgraderDestructionRequested;
                        _highwayUpgraderSummaryDisplay.DisplayCloseRequested        += HighwayUpgraderSummaryDisplay_DisplayCloseRequested;
                    }
                }
            }
        }
        [SerializeField] private HighwayUpgraderSummaryDisplayBase _highwayUpgraderSummaryDisplay;

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
                        _constructionPanel.CloseRequested             -= ConstructionPanel_CloseRequested;
                        _constructionPanel.DepotConstructionRequested -= ConstructionPanel_DepotConstructionRequested;
                    }
                    _constructionPanel = value;
                    if(_constructionPanel != null) {
                        _constructionPanel.CloseRequested             += ConstructionPanel_CloseRequested;
                        _constructionPanel.DepotConstructionRequested += ConstructionPanel_DepotConstructionRequested;
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

                HighwaySummaryDisplay.FirstEndpointResourcePermissionChanged  += HighwaySummaryDisplay_FirstEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.SecondEndpointResourcePermissionChanged += HighwaySummaryDisplay_SecondEndpointResourcePermissionChanged;
                HighwaySummaryDisplay.PriorityChanged                         += HighwaySummaryDisplay_PriorityChanged;
            }

            if(ConstructionPanel != null) {
                ConstructionPanel.CloseRequested             -= ConstructionPanel_CloseRequested;
                ConstructionPanel.DepotConstructionRequested -= ConstructionPanel_DepotConstructionRequested;

                ConstructionPanel.CloseRequested             += ConstructionPanel_CloseRequested;
                ConstructionPanel.DepotConstructionRequested += ConstructionPanel_DepotConstructionRequested;
            }

            if(HighwayUpgraderSummaryDisplay != null) {
                HighwayUpgraderSummaryDisplay.UpgraderDestructionRequested -= HighwayUpgraderSummaryDisplay_UpgraderDestructionRequested;
                HighwayUpgraderSummaryDisplay.DisplayCloseRequested        -= HighwayUpgraderSummaryDisplay_DisplayCloseRequested;

                HighwayUpgraderSummaryDisplay.UpgraderDestructionRequested += HighwayUpgraderSummaryDisplay_UpgraderDestructionRequested;
                HighwayUpgraderSummaryDisplay.DisplayCloseRequested        += HighwayUpgraderSummaryDisplay_DisplayCloseRequested;
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

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            if(source is BlobHighwayUISummary) {
                if(HighwaySummaryDisplay != null) {
                    HighwaySummaryDisplay.ClearDisplay();
                    HighwaySummaryDisplay.CurrentSummary = source as BlobHighwayUISummary;
                    HighwaySummaryDisplay.CanBeUpgraded = 
                        SimulationControl.CanCreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID);
                    HighwaySummaryDisplay.UpdateDisplay();
                }
            }else if(source is MapNodeUISummary) {
                var summary = source as MapNodeUISummary;
                ConstructionPanel.Clear();
                ConstructionPanel.LocationToConstruct = summary;
                ConstructionPanel.HasPermissionForResourceDepot = SimulationControl.CanCreateResourceDepotConstructionSiteOnNode(summary.ID);
                ConstructionPanel.Activate();
            }else if(source is ConstructionZoneUISummary) {
                var summary = source as ConstructionZoneUISummary;
                ConstructionZoneSummaryDisplay.Clear();
                ConstructionZoneSummaryDisplay.SummaryToDisplay = summary;
                ConstructionZoneSummaryDisplay.Activate();
            }else if(source is HighwayUpgraderUISummary) {
                var summary = source as HighwayUpgraderUISummary;
                HighwayUpgraderSummaryDisplay.Clear();
                HighwayUpgraderSummaryDisplay.SummaryToDisplay = summary;
                HighwayUpgraderSummaryDisplay.Activate();
            }
        }

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

        private void ConstructionPanel_DepotConstructionRequested(object sender, EventArgs e) {
            if(SimulationControl.CanCreateResourceDepotConstructionSiteOnNode(ConstructionPanel.LocationToConstruct.ID)) {
                SimulationControl.CreateResourceDepotConstructionSiteOnNode(ConstructionPanel.LocationToConstruct.ID);
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
            SimulationControl.DestroyConstructionZone(ConstructionZoneSummaryDisplay.SummaryToDisplay.ID);
        }

        private void HighwayDisplay_HighwayUpgradeRequested(object sender, EventArgs e) {
            if(SimulationControl.CanCreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID)) {
                SimulationControl.CreateHighwayUpgraderOnHighway(HighwaySummaryDisplay.CurrentSummary.ID);
            }
        }

        private void HighwayUpgraderSummaryDisplay_DisplayCloseRequested(object sender, EventArgs e) {
            HighwayUpgraderSummaryDisplay.Clear();
            HighwayUpgraderSummaryDisplay.Deactivate();
        }

        private void HighwayUpgraderSummaryDisplay_UpgraderDestructionRequested(object sender, EventArgs e) {
            SimulationControl.DestroyHighwayUpgrader(HighwayUpgraderSummaryDisplay.SummaryToDisplay.ID);
        }

        #endregion

    }

}
