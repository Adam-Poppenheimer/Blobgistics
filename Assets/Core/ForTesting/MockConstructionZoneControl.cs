using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockConstructionZoneControl : ConstructionZoneControlBase {

        #region events



        #endregion

        #region instance methods

        #region from ConstructionZoneControlBase

        public override bool CanCreateConstructionZoneOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override void CreateConstructionZoneOnNode(int nodeID, string buildingName) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(int zoneID) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
