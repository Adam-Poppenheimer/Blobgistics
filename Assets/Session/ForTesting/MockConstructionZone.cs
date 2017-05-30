using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;
using Assets.Map;

namespace Assets.Session.ForTesting {

    public class MockConstructionZone : ConstructionZoneBase {

        #region instance fields and properties

        #region from ConstructionZoneBase

        public override ConstructionProjectBase CurrentProject { get; set; }

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase Location {
            get { return location; }
        }
        public MapNodeBase location;

        public override bool ProjectHasBeenCompleted {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion
        
    }

}
