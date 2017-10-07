using System;

namespace Assets.Societies {

    /// <summary>
    /// An EventArgs class for events that involve societies.
    /// </summary>
    public class SocietyEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The society that triggered the event.
        /// </summary>
        public readonly SocietyBase Society;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new event args from the given society.
        /// </summary>
        /// <param name="society">The society that triggered the event</param>
        public SocietyEventArgs(SocietyBase society) {
            Society = society;
        }

        #endregion

    }

}