using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public interface ITubableObject {

        #region properties

        Transform transform { get; }

        Vector3 NorthTubeConnectionPoint { get; }
        Vector3 SouthTubeConnectionPoint { get; }
        Vector3 EastTubeConnectionPoint  { get; }
        Vector3 WestTubeConnectionPoint  { get; }

        #endregion

        #region methods

        Vector3 GetConnectionPointInDirection(ManhattanDirection direction);

        #endregion

    }

}
