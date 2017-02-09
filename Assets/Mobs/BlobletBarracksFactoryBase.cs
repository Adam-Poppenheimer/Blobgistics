using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.BlobEngine;

namespace Assets.Mobs {

    public abstract class BlobletBarracksFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public string SchematicName {
            get { return _schematicName; }
        }
        private string _schematicName = "ResourcePool";

        #endregion

        #region instance methods

        public abstract BlobletBarracksBase ConstructBlobletBarracks(MapNode location);
        public abstract Schematic BuildSchematic();

        #endregion

    }

}
