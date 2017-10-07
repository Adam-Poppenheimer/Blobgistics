using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Core;

using UnityCustomUtilities.Grids;

namespace Assets.Map {

    /// <summary>
    /// The abstract base class that defines a map node. Maps in the game are undirected graphs,
    /// with highways situated along edges and all other features situated on nodes.
    /// </summary>
    public abstract class MapNodeBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// An ID number unique across all other map nodes. This value is not guaranteed to be a
        /// unique key against other object types.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The MapGraphBase that's supposed to be managing this edge.
        /// </summary>
        /// <remarks>
        /// Because of complications arising from prefab instantiating and design-time
        /// copying, it's sometimes necessary for MapNodeBases to be subscribed upon their
        /// creation. Since MapGraphBase has no way of knowing when a new MapEdgeBase
        /// has been created, it's up to the MapNodeBase itself to subscribe itself.
        /// </remarks>
        public abstract MapGraphBase ParentGraph { get; set; }

        /// <summary>
        /// The BlobSite that defines the resource contents of this node.
        /// </summary>
        public abstract BlobSiteBase BlobSite { get; }

        /// <summary>
        /// The neighbors directly connected to this node by a single edge.
        /// </summary>
        /// <remarks>
        /// This method is present primarily for convenience. Neighbor calculations are
        /// expected to be performed by MapGraphBase and its subclasses. It's not clear
        /// that this is a clean or efficient implementation, but that is how the codebase
        /// currently operates.
        /// </remarks>
        public abstract IEnumerable<MapNodeBase> Neighbors { get; }

        /// <summary>
        /// The terrain of the node, which affects appearance, society complexification,
        /// and construction project placement.
        /// </summary>
        public abstract TerrainType Terrain { get; set; }

        /// <summary>
        /// The UIControlBase that this node should send input events (like PointerDown) to.
        /// </summary>
        public abstract UIControlBase UIControl { get; set; }

        /// <summary>
        /// The TerrainMaterialRegistry that the MapNodeBase should use to inform its appearance.
        /// </summary>
        public abstract TerrainMaterialRegistry TerrainMaterialRegistry { get; set; }

        /// <summary>
        /// The TerrainGridBase that the MapNodeBase should use to define its region in the world.
        /// </summary>
        public abstract TerrainGridBase TerrainGrid { get; set; }

        /// <summary>
        /// The tiles in the TerrainGridBase that are associated with this MapNodeBase. Association
        /// is determined by distance.
        /// </summary>
        public abstract ReadOnlyCollection<TerrainHexTile> AssociatedTiles { get; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever the transform changes. This should only ever occur
        /// during design time, since MapNodeBases are intended to be static objects.
        /// </summary>
        public event EventHandler<EventArgs> TransformChanged;

        /// <summary>
        /// Calls the TransformChanged event.
        /// </summary>
        protected void RaiseTransformChanged() {
            if(TransformChanged != null) {
                TransformChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        /// <inheritdoc/>
        /// <returns></returns>
        public override string ToString() {
            return "MapNodeBase " + name;
        }

        #endregion

        /// <summary>
        /// Clears all associated tiles from the MapNodeBase.
        /// </summary>
        /// <remarks>
        /// This method is intended to be called from within TerrainGridBase in order to control
        /// what tiles the MapNodeBase should manipulate when its terrain changes.
        /// </remarks>
        public abstract void ClearAssociatedTiles();

        /// <summary>
        /// Tells the MapNodeBase to associate itself with a new tile.
        /// </summary>
        /// <remarks>
        /// This method is intended to be called from within TerrainGridBase in order to control
        /// what tiles the MapNodeBase should manipulate when its terrain changes.
        /// </remarks>
        /// <param name="tile">The tile the MapNodeBase should associate itself with</param>
        public abstract void AddAssociatedTile(TerrainHexTile tile);

        /// <summary>
        /// Refreshes the outline drawn around the contiguous region of tiles associated with this MapNodeBase.
        /// </summary>
        public abstract void RefreshOutline();
          

        #endregion

    }

}
