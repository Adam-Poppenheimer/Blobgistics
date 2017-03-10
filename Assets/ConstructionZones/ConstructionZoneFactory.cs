using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Depots;

namespace Assets.ConstructionZones {

    public class ConstructionZoneFactory : ConstructionZoneFactoryBase {

        #region instance fields and properties

        #region from ConstructionZoneFactoryBase

        public override ConstructionProjectBase ResourceDepotProject {
            get {
                if(_resourceDepotProject == null) {
                    _resourceDepotProject = new ConstructionProject(ResourceDepotCost, BuildResourceDepot);
                }
                return _resourceDepotProject;
            }
        }
        private ConstructionProjectBase _resourceDepotProject;

        #endregion

        [SerializeField] private GameObject ConstructionZonePrefab;

        [SerializeField] private ResourceDepotFactoryBase ResourceDepotFactory;

        [SerializeField] private ResourceSummary ResourceDepotCost = new ResourceSummary(
            new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)
        );

        #endregion

        #region instance methods

        #region from ConstructionZoneFactoryBase

        public override ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(project == null) {
                throw new ArgumentNullException("project");
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
            newConstructionZone.SetLocation(location);
            newConstructionZone.CurrentProject = project;
            newConstructionZone.ParentFactory = this;
            return newConstructionZone;
        }

        public override void DestroyConstructionZone(ConstructionZoneBase constructionZone) {
            if(constructionZone == null) {
                throw new ArgumentNullException("constructionZone");
            }else {
                DestroyImmediate(constructionZone.gameObject);
            }
        }

        #endregion

        private void BuildResourceDepot(MapNodeBase location) {
            ResourceDepotFactory.ConstructDepot(location);
        }

        #endregion

    }

}
