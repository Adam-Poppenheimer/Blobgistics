using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    [SelectionBase]
    public abstract class MapEdgeBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapNodeBase FirstNode  { get; }
        public abstract MapNodeBase SecondNode { get; }

        public abstract BlobSiteBase BlobSite { get; }

        public abstract MapGraphBase ParentGraph { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> OrientationRefreshed;

        protected void RaiseOrientationRefreshed() {
            if(OrientationRefreshed != null) {
                OrientationRefreshed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return name;
        }

        #endregion

        #endregion

    }

}
