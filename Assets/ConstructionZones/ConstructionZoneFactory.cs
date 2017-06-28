using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.ResourceDepots;
using Assets.Core;
using System.Collections.ObjectModel;

namespace Assets.ConstructionZones {

    public class ConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance fields and properties

        #region from ConstructionZoneFactoryBase

        public List<ConstructionProjectBase> AvailableProjects {
            get { return _availableProjects; }
            set { _availableProjects = value; }
        }
        [SerializeField] private List<ConstructionProjectBase> _availableProjects =
            new List<ConstructionProjectBase>();

        public override ReadOnlyCollection<ConstructionZoneBase> ConstructionZones {
            get { return constructionZones.AsReadOnly(); }
        }
        private List<ConstructionZoneBase> constructionZones = 
            new List<ConstructionZoneBase>();

        #endregion

        [SerializeField] private GameObject ConstructionZonePrefab;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;        

        #endregion

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase GetConstructionZoneOfID(int id) {
            return constructionZones.Find(zone => zone.ID == id);
        }

        public override bool HasConstructionZoneAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return constructionZones.Exists(zone => zone.Location == location);
        }

        public override ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            var desiredZone = constructionZones.Find(zone => zone.Location == location);
            if(desiredZone != null) {
                return desiredZone;
            }else {
                throw new ConstructionZoneException("There exists no ConstructionZone at that location");
            }
        }

        public override bool CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(project == null) {
                throw new ArgumentNullException("project");
            }else if(HasConstructionZoneAtLocation(location)) {
                return false;
            }else {
                return project.IsValidAtLocation(location);
            }
        }

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            if(!CanBuildConstructionZone(location, project)) {
                throw new ConstructionZoneException("Cannot build a construction zone on this location with this project");
            }

            ConstructionZone newConstructionZone = null;

            if(ConstructionZonePrefab != null) {
                var newPrefabInstance = Instantiate<GameObject>(ConstructionZonePrefab);
                newConstructionZone = newPrefabInstance.GetComponent<ConstructionZone>();
                if(newConstructionZone == null) {
                    throw new ConstructionZoneException("The ConstructionZonePrefab lacks a ConstructionZone component");
                }
            }else {
                var hostingObject = new GameObject();
                newConstructionZone = hostingObject.AddComponent<ConstructionZone>();
            }

            newConstructionZone.transform.SetParent(location.transform, false);

            newConstructionZone.SetLocation(location);
            newConstructionZone.CurrentProject = project;
            newConstructionZone.ParentFactory = this;
            newConstructionZone.UIControl = UIControl;

            newConstructionZone.name = "ConstructionZone for " + project.name;
            newConstructionZone.gameObject.SetActive(true);

            constructionZones.Add(newConstructionZone);
            return newConstructionZone;
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            if(constructionZone == null) {
                throw new ArgumentNullException("constructionZone");
            }else {
                UnsubsribeConstructionZone(constructionZone);
                if(Application.isPlaying) {
                    Destroy(constructionZone.gameObject);
                }else {
                    DestroyImmediate(constructionZone.gameObject);
                }
            }
        }

        public override void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone) {
            if(constructionZone != null) {
                constructionZones.Remove(constructionZone);
                if(constructionZone.Location != null && !constructionZone.ProjectHasBeenCompleted) {
                    constructionZone.Location.BlobSite.ClearContents();
                    constructionZone.Location.BlobSite.ClearPermissionsAndCapacity();
                }
            }
        }

        public override bool TryGetProjectOfName(string projectName, out ConstructionProjectBase project) {
            project = AvailableProjects.Find(candidate => candidate.name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
            return project != null;
        }

        public override IEnumerable<ConstructionProjectBase> GetAvailableProjects() {
            return AvailableProjects;
        }

        #endregion

        #endregion

    }

}
