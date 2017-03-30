using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Societies;
using Assets.Map;

namespace Assets.ConstructionZones {

    public class FarmlandConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        #region from ConstructionProjectBase

        public override ResourceSummary Cost {
            get { return _cost; }
        }
        [SerializeField] private ResourceSummary _cost;

        #endregion

        [SerializeField] private SocietyFactoryBase SocietyFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            if(SocietyFactory.CanConstructSocietyAt(location)) {
                SocietyFactory.ConstructSocietyAt(location, SocietyFactory.StandardComplexityLadder);
            }
        }

        #endregion

        #endregion

    }

}
