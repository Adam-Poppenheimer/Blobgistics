using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Highways {

    public abstract class BlobTubeFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract BlobTubeBase ConstructTube(Vector3 pullLocation, Vector3 pushLocation);
        public abstract void DestroyTube(BlobTubeBase tube);

        #endregion

    }

}
