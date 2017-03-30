using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Depots;
using Assets.Core;

namespace Assets.ConstructionZones {

    public class ConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance fields and properties

        #region from ConstructionZoneFactoryBase

        public override ConstructionProjectBase ResourceDepotProject {
            get { return _resourceDepotProject; }
        }
        [SerializeField] private ConstructionProjectBase _resourceDepotProject;

        public override ConstructionProjectBase FarmlandProject {
            get { return _farmlandProject; }
        }
        [SerializeField] private ConstructionProjectBase _farmlandProject;

        public override ConstructionProjectBase VillageProject {
            get { return _villageProject; }
        }
        [SerializeField] private ConstructionProjectBase _villageProject;

        #endregion

        [SerializeField] private GameObject ConstructionZonePrefab;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        private List<ConstructionZoneBase> InstantiatedConstructionZones = 
            new List<ConstructionZoneBase>();

        #endregion

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase GetConstructionZoneOfID(int id) {
            return InstantiatedConstructionZones.Find(zone => zone.ID == id);
        }

        public override bool HasConstructionZoneAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return InstantiatedConstructionZones.Exists(zone => zone.Location == location);
        }

        public override ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            var desiredZone = InstantiatedConstructionZones.Find(zone => zone.Location == location);
            if(desiredZone != null) {
                return desiredZone;
            }else {
                throw new ConstructionZoneException("There exists no ConstructionZone at that location");
            }
        }

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(project == null) {
                throw new ArgumentNullException("project");
            }else if(HasConstructionZoneAtLocation(location)) {
                throw new ConstructionZoneException("There already exists a ConstructionZone at that location");
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
            newConstructionZone.transform.localPosition = Vector3.zero;

            newConstructionZone.SetLocation(location);
            newConstructionZone.CurrentProject = project;
            newConstructionZone.ParentFactory = this;
            newConstructionZone.UIControl = UIControl;

            InstantiatedConstructionZones.Add(newConstructionZone);
            return newConstructionZone;
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            if(constructionZone == null) {
                throw new ArgumentNullException("constructionZone");
            }else {
                InstantiatedConstructionZones.Remove(constructionZone);
                constructionZone.Location.BlobSite.ClearContents();
                constructionZone.Location.BlobSite.ClearPermissionsAndCapacity();
                DestroyImmediate(constructionZone.gameObject);
            }
        }

        #endregion

        #endregion

    }

}
