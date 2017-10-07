using System;

namespace Assets.Highways {

    /// <summary>
    /// An EventArgs class for events involving blob highways.
    /// </summary>
    [Serializable]
    public class BlobHighwayEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The highway involved in the event.
        /// </summary>
        public readonly BlobHighwayBase Highway;

        #endregion

        #region constructors

        /// <summary>
        /// Creates an event involving the given highway.
        /// </summary>
        /// <param name="highway">The highway involved in the event</param>
        public BlobHighwayEventArgs(BlobHighwayBase highway) {
            Highway = highway;
        }

        #endregion

    }

}