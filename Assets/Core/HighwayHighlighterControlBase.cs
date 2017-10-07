using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class for a simulation facade that gives the UI the ability to highlight and unhighlight highways.
    /// </summary>
    public abstract class HighwayHighlighterControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Highlights the highway of the given ID, if it exists.
        /// </summary>
        /// <remarks>
        /// A highway will remain highlighted until it is explicitly unhighlighted.
        /// </remarks>
        /// <param name="highwayID">The ID of the highway to highlight</param>
        public abstract void HighlightHighway(int highwayID);

        /// <summary>
        /// Unhighlights the highway of the given ID, if it exists and is highlighted.
        /// </summary>
        /// <param name="highwayID">The ID of the highway to unhighlight</param>
        public abstract void UnhighlightHighway(int highwayID);

        /// <summary>
        /// Unhighlights all highlighted highways, if any exist.
        /// </summary>
        public abstract void UnhighlightAllHighways();

        #endregion

    }

}
