using System;

namespace Assets.Societies {

    /// <summary>
    /// An EventArgs class for events involving complexity definitions.
    /// </summary>
    public class ComplexityDefinitionEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The complexity that caused the event.
        /// </summary>
        public readonly ComplexityDefinitionBase Complexity;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new event args involving the given complexity.
        /// </summary>
        /// <param name="complexity">The complexity that caused the event</param>
        public ComplexityDefinitionEventArgs(ComplexityDefinitionBase complexity) {
            Complexity = complexity;
        }

        #endregion

    }

}