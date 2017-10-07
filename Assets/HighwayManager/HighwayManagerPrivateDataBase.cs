using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;

namespace Assets.HighwayManager {

    /// <summary>
    /// The abstract base class for highway manager private data, which stores dependency and configuration
    /// data for highway manager.
    /// </summary>
    public abstract class HighwayManagerPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The multiple of the last calculated consumption that the highway manager should attempt to stockpile.
        /// </summary>
        public abstract int NeedStockpileCoefficient { get; }

        /// <summary>
        /// The number of seconds between each consumption and upkeep recalculation.
        /// </summary>
        public abstract float SecondsToPerformConsumption { get; }

        /// <summary>
        /// The UIControlBase that the highway manager will need to send user input events to.
        /// </summary>
        public abstract UIControlBase UIControl { get; }

        /// <summary>
        /// The resource blob factory that highway manager will use to destroy its consumption.
        /// </summary>
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        /// <summary>
        /// The factory the highway manager is subscribed to and must use to determine its managed highways.
        /// </summary>
        public abstract HighwayManagerFactoryBase ParentFactory { get; }

        /// <summary>
        /// A dictionary defining the amount of efficiency that consuming a single unit of a resource will
        /// give a blob highway.
        /// </summary>
        public abstract IntPerResourceDictionary EfficiencyGainFromResource { get; }

        #endregion

    }

}
