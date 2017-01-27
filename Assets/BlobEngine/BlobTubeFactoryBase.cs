using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BlobTubeFactoryBase : MonoBehaviour{

        #region instance methods

        public abstract IEnumerable<ITubableObject> GetObjectsTubedToObject(ITubableObject obj);

        public abstract bool TubeExistsBetweenObjects(ITubableObject obj1, ITubableObject obj2);

        public abstract bool CanBuildTubeBetween(ITubableObject obj1, ITubableObject obj2);
        public abstract bool CanBuildTubeBetween(IBlobSource source,  IBlobTarget target);

        public abstract BlobTube BuildTubeBetween(IBlobSource source, IBlobTarget target);

        public abstract void DestroyAllTubesConnectingTo(ITubableObject obj);

        #endregion

    }

}
