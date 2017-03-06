using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;

namespace Assets.Highways {

    [ExecuteInEditMode]
    public class BlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        [SerializeField] private GameObject TubePrefab;

        #endregion

        #region instance methods

        #region from BlobTubeFactoryBase

        public override BlobTubeBase ConstructTube(Vector3 pullLocation, Vector3 pushLocation) {
            throw new NotImplementedException();
        }

        public override void DestroyTube(BlobTubeBase tube) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
