using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    /// <summary>
    /// A class containing nformation that HighwayManager should pass into UIControl whenever it catches user input.
    /// </summary>
    public class HighwayManagerUISummary {

        #region instance fields and properties

        /// <summary>
        /// Equivalent to <see cref="HighwayManagerBase.ID"/>.
        /// </summary>
        public int ID {
            get { return UnderlyingManager.ID; }
        }

        /// <summary>
        /// Equivalent to <see cref="HighwayManagerBase.LastCalculatedUpkeep"/>.
        /// </summary>
        public ReadOnlyDictionary<ResourceType, int> LastUpkeep {
            get { return UnderlyingManager.LastCalculatedUpkeep; }
        }

        /// <summary>
        /// The attached Transform component of the highway manager.
        /// </summary>
        public Transform Transform {
            get { return UnderlyingManager.transform; }
        }

        private HighwayManagerBase UnderlyingManager;

        #endregion

        #region constructors

        /// <summary>
        /// Constructs an empty summary.
        /// </summary>
        public HighwayManagerUISummary() { }

        /// <summary>
        /// Constructs a summary that's just a wrapper for a given highway manager.
        /// </summary>
        /// <param name="underlyingManager">The manager this summary is wrapping</param>
        public HighwayManagerUISummary(HighwayManagerBase underlyingManager) {
            UnderlyingManager = underlyingManager;
        }

        #endregion

    }

}
