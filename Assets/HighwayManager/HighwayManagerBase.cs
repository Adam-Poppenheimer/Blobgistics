using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;

namespace Assets.HighwayManager {

    public abstract class HighwayManagerBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract uint ManagementRadius { get; }

        public abstract int NeedStockpileCoefficient { get; }

        public abstract float SecondsToPerformConsumption { get; }

        public abstract MapNodeBase Location { get; }

        public abstract float LastCalculatedEfficiency { get; }

        #endregion

        #region instance methods

        public abstract void TickConsumption(float secondsPassed);

        #endregion

    }

}
