using System;
using System.Collections.Generic;

using Assets.Blobs;
using Assets.Map;


namespace Assets.ConstructionZones {

    internal class ConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        public override ResourceSummary Cost {
            get { return _cost; }
        }
        private ResourceSummary _cost;

        public  override Action<MapNodeBase> BuildAction {
            get { return _buildAction; }
        }
        private Action<MapNodeBase> _buildAction;

        #endregion

        #region constructors

        public ConstructionProject(ResourceSummary cost, Action<MapNodeBase> buildAction) {
            _cost = cost;
            _buildAction = buildAction;
        }

        #endregion

    }

}