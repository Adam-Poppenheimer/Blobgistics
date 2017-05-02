using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;
using Assets.Map;

namespace Assets.Core.ForTesting {

    public class MockConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            throw new NotImplementedException();
        }

        public override bool CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ConstructionProjectBase> GetAvailableProjects() {
            throw new NotImplementedException();
        }

        public override ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override ConstructionZoneBase GetConstructionZoneOfID(int id) {
            throw new NotImplementedException();
        }

        public override bool HasConstructionZoneAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool TryGetProjectOfName(string projectName, out ConstructionProjectBase project) {
            throw new NotImplementedException();
        }

        public override void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
