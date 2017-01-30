using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BlobGeneratorFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public string SchematicName {
            get { return _schematicName; }
        }
        private string _schematicName = "ResourcePool";

        #endregion

        #region instance methods

        public abstract IBlobGenerator ConstructGenerator(Transform parent, Vector3 localPosition, ResourceType blobTypeGenerated);
        public abstract IBlobGenerator ConstructGeneratorOnGyser(IResourceGyser gyser);
        public abstract Schematic BuildSchematic();

        #endregion

    }

}
