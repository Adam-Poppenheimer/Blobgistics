using System;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for society private data, which stores information
    /// and dependencies necessary for Society to function.
    /// </summary>
    public abstract class SocietyPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The complexity ladder the society is on.
        /// </summary>
        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        /// <summary>
        /// The blob factory the society should use to create and destroy blobs.
        /// </summary>
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        /// <summary>
        /// The location of the society.
        /// </summary>
        public abstract MapNodeBase Location { get; }

        /// <summary>
        /// The UIControl the society should send user input events to.
        /// </summary>
        public abstract UIControlBase UIControl { get; }

        /// <summary>
        /// The factory that the society should be subscribed to.
        /// </summary>
        public abstract SocietyFactoryBase ParentFactory { get; }

        #endregion

    }

}