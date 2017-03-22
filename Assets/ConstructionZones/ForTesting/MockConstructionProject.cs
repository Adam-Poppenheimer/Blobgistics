using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones.ForTesting {

    public class MockConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        #region ConstructionProjectBase

        public override Action<MapNodeBase> BuildAction {
            get { return _buildAction; }
        }
        public void SetBuildAction(Action<MapNodeBase> value) {
            _buildAction = value;
        }
        private Action<MapNodeBase> _buildAction = null;

        public override ResourceSummary Cost {
            get {
                if(_cost == null) {
                    _cost = ResourceSummary.BuildResourceSummary(new UnityEngine.GameObject());
                }
                return _cost;
            }
        }
        public void SetCost(ResourceSummary value) {
            _cost = value;
        }
        private ResourceSummary _cost = null;

        #endregion

        #endregion
        
    }

}
