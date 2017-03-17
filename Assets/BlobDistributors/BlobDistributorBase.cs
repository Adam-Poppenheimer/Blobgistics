using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobDistributors {

    public abstract class BlobDistributorBase : MonoBehaviour {

        #region instance fields and properties

        public abstract float SecondsToPerformDistributionTick { get; set; }

        #endregion

        #region instance methods

        public abstract void Tick(float secondsPassed);

        protected abstract void PerformDistribution();

        #endregion

    }

}
