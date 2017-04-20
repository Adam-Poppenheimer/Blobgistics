using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;
using Assets.Map;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    public abstract class HighwayManagerBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract MapNodeBase Location { get; }

        public abstract ReadOnlyDictionary<ResourceType, int> LastCalculatedUpkeep { get; }

        #endregion

        #region instance methods

        public abstract void TickConsumption(float secondsPassed);

        #endregion

    }

}
