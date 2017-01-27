using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BuildingSchematicRepository : MonoBehaviour {

        #region instance fields and properties

        private Dictionary<string, BuildingSchematic> SchematicOfName =
            new Dictionary<string, BuildingSchematic>();

        private bool SchematicsAreLoaded = false;

        [SerializeField] private ResourcePoolFactoryBase ResourcePoolFactory;

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
            SchematicOfName["ResourcePool"] = new BuildingSchematic(
                new BlobPileCapacity(
                    new Dictionary<ResourceType, int>() {
                        { ResourceType.Red, 10 },
                    }
                ),
                delegate(BuildingPlot plotToBuildOn) {
                    ResourcePoolFactory.BuildResourcePool(plotToBuildOn.transform.localPosition, 
                        plotToBuildOn.transform.parent);
                }
            );

            SchematicsAreLoaded = true;
        }

        #endregion

    }

}
