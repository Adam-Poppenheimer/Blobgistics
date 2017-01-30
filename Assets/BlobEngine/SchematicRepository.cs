using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class SchematicRepository : MonoBehaviour {

        #region instance fields and properties

        private Dictionary<string, Schematic> SchematicOfName =
            new Dictionary<string, Schematic>();

        [SerializeField] private ResourcePoolFactoryBase  PoolFactory;
        [SerializeField] private BlobGeneratorFactoryBase GeneratorFactory;

        private bool SchematicsAreLoaded = false;

        #endregion

        #region instance methods

        public bool HasSchematicOfName(string name) {
            if(!SchematicsAreLoaded) {
                LoadSchematics();
            }
            return SchematicOfName.ContainsKey(name);
        }

        public Schematic GetSchematicOfName(string name) {
            if(!SchematicsAreLoaded) {
                LoadSchematics();
            }
            Schematic retval;
            SchematicOfName.TryGetValue(name, out retval);
            if(retval != null) {
                return retval;
            }else {
                throw new BlobException("There exists no schematic of the requested name in the BuildingSchematicRepository");
            }
        }

        private void LoadSchematics() {
            SchematicOfName[PoolFactory.SchematicName     ] = PoolFactory.BuildSchematic();
            SchematicOfName[GeneratorFactory.SchematicName] = GeneratorFactory.BuildSchematic();

            SchematicsAreLoaded = true;
        }

        #endregion

    }

}
