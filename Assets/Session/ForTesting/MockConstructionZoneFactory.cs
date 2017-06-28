using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.Map;

namespace Assets.Session.ForTesting {

    public class MockConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance fields and properties

        #region from ConstructionZoneFactoryBase

        public override ReadOnlyCollection<ConstructionZoneBase> ConstructionZones {
            get { return constructionZones.AsReadOnly(); }
        }
        private List<ConstructionZoneBase> constructionZones = new List<ConstructionZoneBase>();

        public List<ConstructionProjectBase> AvailableProjects;

        #endregion

        #endregion

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            var newZone = (new GameObject()).AddComponent<MockConstructionZone>();
            newZone.location = location;
            newZone.CurrentProject = project;
            constructionZones.Add(newZone);
            return newZone;
        }

        public override bool CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            throw new NotImplementedException();
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            constructionZones.Remove(constructionZone);
            DestroyImmediate(constructionZone.gameObject);
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
            project = AvailableProjects.Where(candidate => candidate.name.Equals(projectName)).FirstOrDefault();
            return project != null;
        }

        public override void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
