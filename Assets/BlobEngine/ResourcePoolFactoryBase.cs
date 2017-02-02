using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public abstract class ResourcePoolFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public string SchematicName {
            get { return _schematicName; }
        }
        private string _schematicName = "ResourcePool";

        #endregion

        #region instance methods

        public abstract IResourcePool BuildResourcePool(MapNode location);
        public abstract Schematic BuildSchematic();

        #endregion

    }

}