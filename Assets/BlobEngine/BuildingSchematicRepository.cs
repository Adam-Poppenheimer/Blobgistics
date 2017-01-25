using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BuildingSchematicRepository {

        #region static fields and properties

        public static BuildingSchematicRepository Instance {
            get {
                if(_instance == null) {
                    _instance = new BuildingSchematicRepository();
                }
                return _instance;
            }
        }
        private static BuildingSchematicRepository _instance;

        #endregion

        #region instance fields and properties

        private Dictionary<string, BuildingSchematic> SchematicOfName =
            new Dictionary<string, BuildingSchematic>();

        private bool SchematicsAreLoaded = false;

        private readonly Dictionary<string, BlobPileCapacity> BuildableSchematicData =
            new Dictionary<string, BlobPileCapacity>() {
                { "ResourcePool", new BlobPileCapacity(new Dictionary<ResourceType, int>() {
                    { ResourceType.Red, 10 },
                })},
        };

        #endregion

        #region constructors

        private BuildingSchematicRepository() { }

        #endregion

        #region instance methods

        public BuildingSchematic GetSchematicOfName(string name) {
            if(!SchematicsAreLoaded) {
                LoadSchematics();
            }
            BuildingSchematic retval;
            SchematicOfName.TryGetValue(name, out retval);
            if(retval != null) {
                return retval;
            }else {
                throw new BlobException("There exists no schematic of the requested name in the BuildingSchematicRepository");
            }
        }

        private void LoadSchematics() {
            foreach(var schematicName in BuildableSchematicData.Keys) {
                var schematicPrefab = Resources.Load<GameObject>(schematicName + "Prefab");
                if(schematicPrefab == null) {
                    throw new BlobException("Could not load prefab for schematic of name " + schematicName);
                }
                SchematicOfName[schematicName] = new BuildingSchematic(
                    BuildableSchematicData[schematicName],
                    ConstructFromPrefabFactory(schematicPrefab)
                );
            }

            SchematicsAreLoaded = true;
        }

        private Action<BuildingPlot> ConstructFromPrefabFactory(GameObject prefab) {
            return delegate(BuildingPlot plotToBuildOn) {
                var buildingGameObject = GameObject.Instantiate(prefab);
                buildingGameObject.transform.position = plotToBuildOn.transform.position;
                buildingGameObject.SetActive(true);
            };
        }

        #endregion

    }

}
