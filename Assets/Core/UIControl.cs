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

using Assets.ResourceDepots;
using Assets.UI.ResourceDepots;

using Assets.HighwayManager;
using Assets.UI.HighwayManager;

using UnityCustomUtilities.Extensions;

namespace Assets.Core {

    public class UIControl : UIControlBase {

        #region instance fields and properties

        public SimulationControlBase SimulationControl {
            get { return _simulationControl; }
            set { _simulationControl = value; }
        }
        [SerializeField] private SimulationControlBase _simulationControl;

        public BlobHighwayGhostBase HighwayGhost {
            get { return _highwayGhost; }
            set { _highwayGhost = value; }
        }
        [SerializeField] private BlobHighwayGhostBase _highwayGhost;

        public BlobHighwaySummaryDisplayBase HighwaySummaryDisplay {
            get { return _highwayDisplay; }
            set {
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
        [SerializeField] private BlobHighwaySummaryDisplayBase _highwayDisplay;

        public ConstructionZoneSummaryDisplayBase ConstructionZoneSummaryDisplay {
            get { return _constructionZoneSummaryDisplay; }
            set {
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
        [SerializeField] private ConstructionZoneSummaryDisplayBase _constructionZoneSummaryDisplay;

        public SocietyUISummaryDisplayBase SocietySummaryDisplay {
            get { return _societySummaryDisplay; }
            set {
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested -= SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested -= SocietySummaryDisplay_AscensionPermissionChangeRequested;
                }
                _societySummaryDisplay = value;
                if(_societySummaryDisplay != null) {
                    _societySummaryDisplay.DestructionRequested += SocietySummaryDisplay_DestructionRequested;
                    _societySummaryDisplay.AscensionPermissionChangeRequested += SocietySummaryDisplay_AscensionPermissionChangeRequested;
                }
            }
        }
        [SerializeField] private SocietyUISummaryDisplayBase _societySummaryDisplay;

        public ResourceDepotSummaryDisplayBase DepotSummaryDisplay {
            get { return _depotSummaryDisplay; }
            set {
                if(_depotSummaryDisplay != null) {
                    _depotSummaryDisplay.DestructionRequested -= DepotSummaryDisplay_DestructionRequested;
                }
                _depotSummaryDisplay = value;
                if(_depotSummaryDisplay != null) {
                    _depotSummaryDisplay.DestructionRequested += DepotSummaryDisplay_DestructionRequested;
                }
            }
        }
        [SerializeField] private ResourceDepotSummaryDisplayBase _depotSummaryDisplay;

        public ConstructionPanelBase ConstructionPanel {
            get { return _constructionPanel; }
            set {
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
        [SerializeField] private ConstructionPanelBase _constructionPanel;

        public HighwayManagerSummaryDisplayBase HighwayManagerSummaryDisplay {
            get { return _highwayManagerDisplay; }
            set {
                if(_highwayManagerDisplay != null) {
                    _highwayManagerDisplay.DestructionRequested -= HighwayManagerDisplay_DestructionRequested;
                }
                _highwayManagerDisplay = value;
                if(_highwayManagerDisplay != null) {
                    _highwayManagerDisplay.DestructionRequested += HighwayManagerDisplay_DestructionRequested;
                }
            }
        }
        [SerializeField] private HighwayManagerSummaryDisplayBase _highwayManagerDisplay;

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
                SocietySummaryDisplay.DestructionRequested               -= SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested -= SocietySummaryDisplay_AscensionPermissionChangeRequested;

                SocietySummaryDisplay.DestructionRequested               += SocietySummaryDisplay_DestructionRequested;
                SocietySummaryDisplay.AscensionPermissionChangeRequested += SocietySummaryDisplay_AscensionPermissionChangeRequested;
            }

            if(DepotSummaryDisplay != null) {
                DepotSummaryDisplay.DestructionRequested -= DepotSummaryDisplay_DestructionRequested;

                DepotSummaryDisplay.DestructionRequested += DepotSummaryDisplay_DestructionRequested;
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
            if(source is BlobHighwayUISummary) {
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
            }else if(source is ResourceDepotUISummary) {
                if(DepotSummaryDisplay != null) {
                    DepotSummaryDisplay.CurrentSummary = source as ResourceDepotUISummary;
                    DepotSummaryDisplay.Activate();
                }
            }else if(source is MapNodeUISummary) {
                var summary = source as MapNodeUISummary;
                ConstructionPanel.LocationToConstruct = summary;
                ConstructionPanel.SetPermittedProjects(SimulationControl.GetAllPermittedConstructionZoneProjectsOnNode(summary.ID));
                ConstructionPanel.Activate();
            }else if(source is HighwayManagerUISummary) {
                var summary = source as HighwayManagerUISummary;
                if(HighwayManagerSummaryDisplay != null) {
                    HighwayManagerSummaryDisplay.CurrentSummary = summary;
                    HighwayManagerSummaryDisplay.Activate();
                }
            }
        }

        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {}

        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {}

        public override void PushObjectDestroyedEvent<T>(T source) {
            if(source is BlobHighwayUISummary) {
                if(source == HighwaySummaryDisplay.CurrentSummary) {
                    HighwaySummaryDisplay.Deactivate();
                }
            }else if(source is SocietyUISummary) {
                if(source == SocietySummaryDisplay.CurrentSummary) {
                    SocietySummaryDisplay.Deactivate();
                }
                
            }else if(source is ConstructionZoneUISummary) {
                if(source == ConstructionZoneSummaryDisplay.CurrentSummary) {
                    ConstructionZoneSummaryDisplay.Deactivate();
                }
            }else if(source is ResourceDepotUISummary) {
                if(source == DepotSummaryDisplay.CurrentSummary) {
                    DepotSummaryDisplay.Deactivate();
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

        private void SocietySummaryDisplay_AscensionPermissionChangeRequested(object sender, BoolEventArgs e) {
            SimulationControl.SetAscensionPermissionForSociety(SocietySummaryDisplay.CurrentSummary.ID, e.Value);
        }

        private void DepotSummaryDisplay_DestructionRequested(object sender, EventArgs e) {
            SimulationControl.DestroyResourceDepotOfID(DepotSummaryDisplay.CurrentSummary.ID);
            DepotSummaryDisplay.Deactivate();
        }

        private void HighwayManagerDisplay_DestructionRequested(object sender, EventArgs e) {
            SimulationControl.DestroyHighwayManagerOfID(HighwayManagerSummaryDisplay.CurrentSummary.ID);
            HighwayManagerSummaryDisplay.Deactivate();
        }

        #endregion

    }

}
