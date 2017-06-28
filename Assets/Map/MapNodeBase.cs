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

    public abstract class MapNodeBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapGraphBase ParentGraph { get; set; }

        public abstract BlobSiteBase BlobSite { get; }

        public abstract IEnumerable<MapNodeBase> Neighbors { get; }

        public abstract TerrainType Terrain { get; set; }

        public abstract UIControlBase UIControl { get; set; }

        public abstract TerrainMaterialRegistry TerrainMaterialRegistry { get; set; }

        public abstract TerrainGridBase TerrainGrid { get; set; }

        public abstract ReadOnlyCollection<TerrainHexTile> AssociatedTiles { get; }

        #endregion

        #region events

        public event EventHandler<EventArgs> TransformChanged;

        protected void RaiseTransformChanged() {
            if(TransformChanged != null) {
                TransformChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return "MapNodeBase " + name;
        }

        #endregion

        public abstract void ClearAssociatedTiles();

        public abstract void AddAssociatedTile(TerrainHexTile tile);

        public abstract void RefreshOutline();
          

        #endregion

    }

}
