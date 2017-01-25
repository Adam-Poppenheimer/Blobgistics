using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BlobEngine {

    public interface IBlobTubeFactory{

        #region methods

        IEnumerable<ITubableObject> GetObjectsTubedToObject(ITubableObject obj);

        bool TubeExistsBetweenObjects(ITubableObject obj1, ITubableObject obj2);

        bool CanBuildTubeBetween(ITubableObject obj1, ITubableObject obj2);
        bool CanBuildTubeBetween(IBlobSource source,  IBlobTarget target);

        BlobTube BuildTubeBetween(IBlobSource source, IBlobTarget target);

        void DestroyAllTubesConnectingTo(ITubableObject obj);

        #endregion

    }

}
