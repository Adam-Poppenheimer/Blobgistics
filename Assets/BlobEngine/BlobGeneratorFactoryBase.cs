using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public abstract class BlobGeneratorFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public string SchematicName {
            get { return _schematicName; }
        }
        private string _schematicName = "Generator";

        #endregion

        #region instance methods

        public abstract IBlobGenerator ConstructGenerator(MapNode parent, ResourceType blobTypeGenerated);
        public abstract IBlobGenerator ConstructGeneratorOnGyser(IResourceGyser gyser);
        public abstract Schematic BuildSchematic();

        #endregion

    }

}
