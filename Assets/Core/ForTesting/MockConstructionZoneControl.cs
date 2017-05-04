using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.ConstructionZones;

namespace Assets.Core.ForTesting {

    public class MockConstructionZoneControl : ConstructionZoneControlBase {

        #region instance fields and properties

        public List<ConstructionProjectUISummary> PermittedProjects;

        #endregion

        #region events

        public event Action<int, string> CreateConstructionZoneOnNodeCalled;
        public event Action<int> DestroyConstructionZoneCalled;

        #endregion

        #region instance methods

        #region from ConstructionZoneControlBase

        public override bool CanCreateConstructionZoneOnNode(int nodeID, string buildingName) {
            return true;
        }

        public override void CreateConstructionZoneOnNode(int nodeID, string buildingName) {
            if(CreateConstructionZoneOnNodeCalled != null) {
                CreateConstructionZoneOnNodeCalled(nodeID, buildingName);
            }
        }

        public override void DestroyConstructionZone(int zoneID) {
            if(DestroyConstructionZoneCalled != null) {
                DestroyConstructionZoneCalled(zoneID);
            }
        }

        public override IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID) {
            return PermittedProjects;
        }

        #endregion

        #endregion
        
    }

}
