using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobDistributors {

    /// <summary>
    /// The abstract base class that manages the distribution of blobs from blob sites into
    /// blob highways.
    /// </summary>
    public abstract class BlobDistributorBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Increments the blob distribution simulation by some number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds to advance the simulation by</param>
        public abstract void Tick(float secondsPassed);

        #endregion

    }

}
