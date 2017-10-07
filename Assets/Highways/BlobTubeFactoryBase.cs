using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Highways {

    /// <summary>
    /// The abstract base class for all tube factories.
    /// </summary>
    /// <remarks>
    /// Note that this factory does not need the subscribe/unsubscribe pattern because it doesn't
    /// manage or keep track of its tubes after they've been constructed.
    /// </remarks>
    public abstract class BlobTubeFactoryBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Constructs a tube with the given source and target locations.
        /// </summary>
        /// <param name="sourceLocation">The source location of the new tube</param>
        /// <param name="targetLocation">The target location of the new tube</param>
        /// <returns>The new tube with the desired properties</returns>
        public abstract BlobTubeBase ConstructTube(Vector3 sourceLocation, Vector3 targetLocation);

        /// <summary>
        /// Destroys the given tube.
        /// </summary>
        /// <param name="tube">The tube to destroy</param>
        public abstract void DestroyTube(BlobTubeBase tube);

        #endregion

    }

}
