using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Core;

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

        #endregion

    }

}
