using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Core;
using Assets.BlobSites;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class MapNode : MapNodeBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region static fields and properties

        private static List<MapNodeBase> EmptyNeighborList = new List<MapNodeBase>();

        #endregion

        #region instance fields and properties

        #region from MapNodeBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapGraphBase ParentGraph {
            get { return _parentGraph; }
        }
        public void SetParentGraph(MapGraphBase value) {
            _parentGraph = value;
        }
        [SerializeField, HideInInspector] private MapGraphBase _parentGraph;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField, HideInInspector] private BlobSiteBase _blobSite;

        public override IEnumerable<MapNodeBase> Neighbors {
            get {
                if(ParentGraph != null) {
                    return ParentGraph.GetNeighborsOfNode(this);
                }else {
                    return EmptyNeighborList;
                }
            }
        }

        public override TerrainType Terrain {
            get { return _terrain; }
            set {
                _terrain = value;
                RefreshAppearance();
            }
        }
        [SerializeField] private TerrainType _terrain;

        #endregion

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set {
                if(_terrainMaterialRegistry != null) {
                    _terrainMaterialRegistry.TerrainMaterialChanged -= TerrainMaterialRegistry_TerrainMaterialChanged;
                }
                _terrainMaterialRegistry = value;
                if(_terrainMaterialRegistry != null) {
                    _terrainMaterialRegistry.TerrainMaterialChanged += TerrainMaterialRegistry_TerrainMaterialChanged;
                }
            }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        [SerializeField] private MeshRenderer TerrainSlateRenderer;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ParentGraph != null && ParentGraph.GetNodeOfID(ID) == null) {
                ParentGraph.SubscribeNode(this);
            }
        }

        private void OnDestroy() {
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeNode(this);
            }
        }

        private void OnValidate() {
            RefreshAppearance();
        }

        #endregion

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new MapNodeUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new MapNodeUISummary(this), eventData);
        }

        #endregion

        private void RefreshAppearance() {
            if(TerrainSlateRenderer != null && TerrainMaterialRegistry != null) {
                TerrainSlateRenderer.sharedMaterial = TerrainMaterialRegistry.GetMaterialForTerrain(Terrain);
            }
        }

        private void TerrainMaterialRegistry_TerrainMaterialChanged(object sender, EventArgs e) {
            RefreshAppearance();
        }

        #endregion

    }

}
