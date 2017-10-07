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

    /// <summary>
    /// The standard implementation of MapNodeBase. Maps in the game are undirected graphs,
    /// with highways situated along edges and all other features situated on nodes.
    /// </summary>
    [ExecuteInEditMode]
    public class MapNode : MapNodeBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region static fields and properties

        private static List<MapNodeBase> EmptyNeighborList = new List<MapNodeBase>();

        #endregion

        #region instance fields and properties

        #region from MapNodeBase

        /// <inheritdoc/>
        public override int ID {
            get { return GetInstanceID(); }
        }

        /// <inheritdoc/>
        public override MapGraphBase ParentGraph {
            get { return _parentGraph; }
            set { _parentGraph = value; }
        }
        [SerializeField] private MapGraphBase _parentGraph;

        /// <inheritdoc/>
        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        /// <summary>
        /// The externalized Set function for BlobSite.
        /// </summary>
        /// <param name="value">The new value for BlobSite</param>
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField] private BlobSiteBase _blobSite;

        /// <inheritdoc/>
        public override IEnumerable<MapNodeBase> Neighbors {
            get {
                if(ParentGraph != null) {
                    return ParentGraph.GetNeighborsOfNode(this);
                }else {
                    return EmptyNeighborList;
                }
            }
        }

        /// <inheritdoc/>
        public override TerrainType Terrain {
            get { return _terrain; }
            set {
                _terrain = value;
                RefreshAppearance();
            }
        }
        [SerializeField] private TerrainType _terrain;

        /// <inheritdoc/>
        public override UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <inheritdoc/>
        public override TerrainGridBase TerrainGrid {
            get { return _terrainGrid; }
            set { _terrainGrid = value; }
        }
        [SerializeField] private TerrainGridBase _terrainGrid;

        /// <inheritdoc/>
        /// <remarks>
        /// MapNode subscribes to TerrainMaterialRegistry's OnTerrainMaterialChanged events in order to
        /// make editing maps at design time easier.
        /// </remarks>
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

        /// <inheritdoc/>
        public override ReadOnlyCollection<TerrainHexTile> AssociatedTiles {
            get { return associatedTiles.AsReadOnly(); }
        }
        [SerializeField] private List<TerrainHexTile> associatedTiles = new List<TerrainHexTile>();

        /// <summary>
        /// The LineRenderer used to create the thick outlines around the MapNode's associated terrain tiles.
        /// </summary>
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
        
        //The Start and OnValidate methods exist to make sure that the MapNode
        //is always subscribed to its ParentGraph.
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

        //The MapNode publishes TransformChanged events on the update cycle in order to
        //improve designer experience at design-time. It's expected that MapNodes will not
        //modify their transforms during play.
        private void Update() {
            if(transform.hasChanged) {
                RaiseTransformChanged();
            }
        }

        //The MapNode makes sure to unsubscribe itself from its parent graph when it's destroyed,
        //since there's no guarantee that all object destruction will pass through MapGraph.
        private void OnDestroy() {
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeNode(this);
            }
        }

        #endif

        #endregion

        #region EventSystem interface implementations
        /*
            The general strategy of all UI in this game is to push all player commands into UIControl and handle it
            there. This has the effect of centralizing where UI is performed and helps establish the UI as a separate
            layer independent of the implementation. Thus all user-driven events on MapNode are simply pushed into
            UIControl. The one exception to this use is the OnPointerClick event, which hacks Unity's notion of selection
            (normally reserved for GUI objects) to make it easier to select gameplay elements.
            It's not clear that this is a good paradigm, and I need to do more work to establish a proper exchange of
            information between the main simulation code and the UI.
        */
        /// <inheritdoc/>
        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new MapNodeUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new MapNodeUISummary(this), eventData);
        }

        /// <inheritdoc/>
        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new MapNodeUISummary(this), eventData);
        }

        #endregion

        #region from Object

        /// <inheritdoc/>
        public override string ToString() {
            return name;
        }

        #endregion  

        /// <inheritdoc/>
        public override void ClearAssociatedTiles() {
            associatedTiles.Clear();
        }

        /// <inheritdoc/>
        public override void AddAssociatedTile(TerrainHexTile tile) {
            associatedTiles.Add(tile);
            tile.Terrain = Terrain;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The RefreshOutline algorithm works in the following way. It calls GetFirstExtrenalTileAndNeighborIndex
        /// to find some tile with an external edge, and also the edge that is external. If it finds no external
        /// tile, that means there are no associated titles and there's nothing to do. If it does find an external
        /// tile, it has work to do.
        /// 
        /// The main loop continuously considers the active tile and the active neighbor index. The active neighbor
        /// index is a number, from 0 to 5, that are used by the internals of TerrainGrid to pull neighboring hexes.
        /// Each side of the hexagon is numbered from 0 to 5, as well, and the ith neighbor index refers to the neighboring
        /// hex bordering the current hex along its ith edge.
        /// 
        /// First, the main loop checks the neighbor at the active neighbor index of the active tile. If the neighbor doesn't
        /// exist or isn't associated with this MapNode, it considers it an external edge and draws a line along it (by defining
        /// the line's endpoints). Then it increments activeNeighborIndex by 1 (which rotates us around the hex counterclockwise)
        /// and repeat the process. If the neighbor it finds at the active neighbor index is associated with this MapNode, then
        /// the edge is internal. It then sets the active tile to the neighbor it found, sets the active neighbor
        /// index to the opposite side of the hex (the edge touching the hex we just left) and repeats the process.
        /// 
        /// The loop continues to run until the active tile equals the starting tile and the active neighbor index equals the
        /// starting neighbor index. This has the effect of running along the outside of the associated hexes of the MapNode
        /// counterclockwise, drawing lines along all the external edges. This algorithm is sufficient because it can guarantee
        /// a contiguous block of adjacent hexes with no gaps or holes based on how associated tiles are assigned.
        /// 
        /// It might be wise to gather this algorithm and place it in TerrainGrid entirely, as the algorithm
        /// relies on facts established by TerrainGrid in order to function.
        /// </remarks>
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

        //Finds some tile associated with the MapNode that has an external edge. An external edge is some edge where the
        //neighboring tile either doesn't exist or isn't associated with the MapNode.
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
