using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.UI.Blobs {

    /// <summary>
    /// Abstract base class for resource display, which is designed to take resource
    /// summaries and display them with text and the corresponding colors of their
    /// materials.
    /// </summary>
    public abstract class ResourceDisplayBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Displays the resources specified by the given IntPerResourceDictionary.
        /// </summary>
        /// <param name="summary">The dictionary whose contents should be displayed</param>
        public abstract void PushAndDisplaySummary(IntPerResourceDictionary summary);

        /// <summary>
        /// Displays the resources specified by the given IDictionary.
        /// </summary>
        /// <param name="summaryDictionary">The dictionary whose contents should be displayed</param>
        public abstract void PushAndDisplaySummary(IDictionary<ResourceType, int> summaryDictionary);

        /// <summary>
        /// Displays the resources as a given number of resources required and a
        /// list of valid types that can satisfy that count.
        /// </summary>
        /// <param name="typesAccepted">A list of types to display as valid</param>
        /// <param name="countNeeded">The number of resources required</param>
        public abstract void PushAndDisplaySummary(IEnumerable<ResourceType> typesAccepted, int countNeeded);

        /// <summary>
        /// Displays the information within the given ResourceDisplayInfo.
        /// </summary>
        /// <param name="infoToDisplay">The information to be displayed in the summary</param>
        public abstract void PushAndDisplayInfo(ResourceDisplayInfo infoToDisplay);

        #endregion

    }

}
