using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.Map;
using System.Collections.ObjectModel;

namespace Assets.Core.ForTesting {

    public class MockConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance fields and properties

        #region from ConstructionZoneFactoryBase

        public override ReadOnlyCollection<ConstructionZoneBase> ConstructionZones {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance fields and properties

        public ConstructionProjectBase ResourceDepotProject {
            get {
                if(_resourceDepotProject == null) {
                    _resourceDepotProject = gameObject.AddComponent<MockConstructionProject>();
                }
                return _resourceDepotProject;
            }
        }
        private ConstructionProjectBase _resourceDepotProject;

        public ConstructionProjectBase VillageProject {
            get {
                if(_villageProject == null){
                    _villageProject = gameObject.AddComponent<MockConstructionProject>();
                }
                return _villageProject;
            }
        }
        private ConstructionProjectBase _villageProject;

        private ConstructionProjectBase FarmlandProject {
            get {
                if(_farmlandProject == null){
                    _farmlandProject = gameObject.AddComponent<MockConstructionProject>();
                }
                return _farmlandProject;
            }
        }
        private ConstructionProjectBase _farmlandProject;

        private List<ConstructionZoneBase> Zones = new List<ConstructionZoneBase>();

        #endregion

        #region events

        public event Action<MapNodeBase, ConstructionProjectBase> BuildConstructionZoneCalled;

        #endregion

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            if(BuildConstructionZoneCalled != null) {
                BuildConstructionZoneCalled(location, project);
            }

            var newZone = (new GameObject()).AddComponent<MockConstructionZone>();

            newZone.location = location;
            newZone.CurrentProject = project;

            Zones.Add(newZone);
            return newZone;
        }

        public override bool CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            return GetConstructionZoneAtLocation(location) == null;
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            Zones.Remove(constructionZone);
            DestroyImmediate(constructionZone.gameObject);
        }

        public override ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location) {
            return Zones.Where(zone => zone.Location == location).FirstOrDefault();
        }

        public override ConstructionZoneBase GetConstructionZoneOfID(int id) {
            return Zones.Where(zone => zone.ID == id).FirstOrDefault();
        }

        public override bool HasConstructionZoneAtLocation(MapNodeBase location) {
            return GetConstructionZoneAtLocation(location) != null;
        }

        public override bool TryGetProjectOfName(string projectName, out ConstructionProjectBase project) {
            switch(projectName) {
                case "Resource Depot": project = ResourceDepotProject; break;
                case "Village": project = VillageProject; break;
                case "Farmland": project = FarmlandProject; break;
                default: project = null; break;
            }
            return project != null;
        }

        public override void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ConstructionProjectBase> GetAvailableProjects() {
            return new List<ConstructionProjectBase>() {
                VillageProject, ResourceDepotProject, FarmlandProject
            };
        }

        #endregion

        #endregion

    }

}
