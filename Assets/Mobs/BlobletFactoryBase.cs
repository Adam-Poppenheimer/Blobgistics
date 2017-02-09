using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Mobs {

    public abstract class BlobletFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract BlobletBase ConstructBloblet(Vector3 positionInMap);

        #endregion

    }

}
