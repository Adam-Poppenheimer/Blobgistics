using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

namespace Assets.UI.Highways {

    /// <summary>
    /// Base class for a highway ghost, which helps show the player where the highway they
    /// are drawing will go and whether it's valid.
    /// </summary>
    public abstract class BlobHighwayGhostBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// Whether the ghost is currently being displayed.
        /// </summary>
        public abstract bool IsActivated { get; }

        /// <summary>
        /// The first endpoint the ghost is anchored to.
        /// </summary>
        public abstract MapNodeUISummary FirstEndpoint { get; set; }

        /// <summary>
        /// The second endpoint the ghost is going to, which may be null.
        /// </summary>
        public abstract MapNodeUISummary SecondEndpoint { get; set; }

        /// <summary>
        /// Whether or not the ghost should tell the player that the highway they've
        /// drawn is buildable.
        /// </summary>
        public abstract bool GhostIsBuildable { get; set; }

        #endregion

        #region instance methods

        /// <summary>
        /// Activates the highway ghost.
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// Deactivates the highway ghost.
        /// </summary>
        public abstract void Deactivate();

        /// <summary>
        /// Clears the highway ghost of all non-initialization data.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Updates the highway ghost with some amount of event data. This data usually comes
        /// from a drag operation, though that is not required.
        /// </summary>
        /// <param name="eventData"></param>
        public abstract void UpdateWithEventData(PointerEventData eventData);

        #endregion

    }

}
