using System;

using UnityEngine;

using Assets.Core;


using UnityCustomUtilities.UI;

namespace Assets.Highways {

    public abstract class BlobHighwayPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract BlobTubeFactoryBase TubeFactory { get; }
        public abstract UIControl UIControl { get; }

        #endregion

    }

}