using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Core;
using Assets.BlobSites;

using UnityCustomUtilities.Grids;

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
            set { _parentGraph = value; }
        }
        [SerializeField] private MapGraphBase _parentGraph;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField] private BlobSiteBase _blobSite;

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

        public override UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public override TerrainGridBase TerrainGrid {
            get { return _terrainGrid; }
            set { _terrainGrid = value; }
        }
        [SerializeField] private TerrainGridBase _terrainGrid;

        public override TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set {
                if(_terrainMaterialRegistry != null) {
                    _terrainMaterialRegistry.TerrainMaterialChanged -= TerrainMaterialRegistry_TerrainMaterialChanged;
                }
                _terrainMaterialRegistry = value;
                if(_terrainMaterialRegistry != null) {
                    RefreshAppearance();
                    _terrainMaterialRegistry.TerrainMaterialChanged += TerrainMaterialRegistry_TerrainMaterialChanged;
                }
            }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        public override ReadOnlyCollection<TerrainHexTile> AssociatedTiles {
            get { return associatedTiles.AsReadOnly(); }
        }
        [SerializeField] private List<TerrainHexTile> associatedTiles = new List<TerrainHexTile>();

        public LineRenderer TerrainOutlineRenderer {
            get { return _terrainOutlineRenderer; }
            set { _terrainOutlineRenderer = value; }
        }
        [SerializeField] private LineRenderer _terrainOutlineRenderer;

        #endregion        

        #endregion

        #region instance methods

        #region Unity event methods

        #if UNITY_EDITOR

        private void Start() {
            var graphAbove = GetComponentInParent<MapGraphBase>();
            if(graphAbove != null) {
                graphAbove.SubscribeNode(this);
            }
        }

        private void OnValidate() {
            RefreshAppearance();
            if(ParentGraph != null && ParentGraph.GetNodeOfID(ID) == null) {
                ParentGraph.SubscribeNode(this);
            }
        }

        private void Update() {
            if(transform.hasChanged) {
                RaiseTransformChanged();
            }
        }

        private void OnDestroy() {
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeNode(this);
            }
        }

        #endif

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

        #region from Object

        public override string ToString() {
            return name;
        }

        #endregion  

        public override void ClearAssociatedTiles() {
            associatedTiles.Clear();
        }

        public override void AddAssociatedTile(TerrainHexTile tile) {
            associatedTiles.Add(tile);
            tile.Terrain = Terrain;
        }

        public override void RefreshOutline() {
            var pointList = new List<Vector3>();

            TerrainHexTile startingTile, activeTile;
            int startingNeighborIndex, activeNeighborIndex;
                       
            startingTile = GetFirstExternalTileAndNeighborIndex(associatedTiles, out startingNeighborIndex);
            if(startingTile != null) {
                activeTile = startingTile;
                activeNeighborIndex = startingNeighborIndex;

                pointList.Add(HexGridLayout.GetHexCornerOffset(TerrainGrid.Layout, activeNeighborIndex - 1) + (Vector2)activeTile.transform.localPosition);
                pointList.Add(HexGridLayout.GetHexCornerOffset(TerrainGrid.Layout, activeNeighborIndex) + (Vector2)activeTile.transform.localPosition);

                activeNeighborIndex = (activeNeighborIndex + 1) % 6;

                while(activeTile != startingTile || activeNeighborIndex != startingNeighborIndex) {
                    TerrainHexTile neighborInDirection;
                    TerrainGrid.TryGetNeighborInDirection(activeTile, activeNeighborIndex, out neighborInDirection);
                    if(neighborInDirection == null || !AssociatedTiles.Contains(neighborInDirection)) {
                        pointList.Add(HexGridLayout.GetHexCornerOffset(TerrainGrid.Layout, activeNeighborIndex) + (Vector2)activeTile.transform.localPosition);
                        activeNeighborIndex = (activeNeighborIndex + 1) % 6;
                    }else {
                        activeTile = neighborInDirection;
                        activeNeighborIndex = (activeNeighborIndex + 4) % 6;
                    }
                }
            }

            var pointArray = pointList.ToArray();
            TerrainOutlineRenderer.positionCount = pointArray.Length;
            TerrainOutlineRenderer.SetPositions(pointArray);
        }

        private TerrainHexTile GetFirstExternalTileAndNeighborIndex(List<TerrainHexTile> internalTiles, out int externalEdgeIndex) {
            foreach(var edgeCandidate in internalTiles) {
                for(int i = 0; i < 6; ++i) {
                    TerrainHexTile neighborInDirection;
                    bool neighborExists = TerrainGrid.TryGetNeighborInDirection(edgeCandidate, i, out neighborInDirection);
                    if(neighborExists) {
                        if(!internalTiles.Contains(neighborInDirection)){
                            externalEdgeIndex = i;
                            return edgeCandidate;
                        }
                    }
                }
            }
            externalEdgeIndex = -1;
            return null;
        }

        private void RefreshAppearance() {
            foreach(var tile in AssociatedTiles) {
                tile.Terrain = Terrain;
            }
        }

        private void TerrainMaterialRegistry_TerrainMaterialChanged(object sender, EventArgs e) {
            RefreshAppearance();
        }

        #endregion

    }

}
