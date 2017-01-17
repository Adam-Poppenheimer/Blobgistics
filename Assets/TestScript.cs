using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobEngine;

namespace Assets {

    public class TestScript : MonoBehaviour {

        #region instance fields and properties

        public BlobGenerator Generator;
        public ResourcePool ResourcePool;

        public BlobTube Tube;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Tube.SetEndpoints(Generator, ResourcePool);
        }

        #endregion

        #endregion

    }

}