using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Societies {

    /// <summary>
    /// The abstract base class for the society display, which gives players information
    /// and commands regarding societies.
    /// </summary>
    public abstract class SocietyUISummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        /// <summary>
        /// The Society summary to be displayed.
        /// </summary>
        public abstract SocietyUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires when the player requests the destruction of the current society.
        /// </summary>
        public event EventHandler<EventArgs> DestructionRequested;

        /// <summary>
        /// Fires when the player requests an ascension permission change on the whole society.
        /// </summary>
        public event EventHandler<BoolEventArgs> AscensionPermissionChangeRequested;

        /// <summary>
        /// Fires when the player requests an ascension permission change for a specific complexity.
        /// </summary>
        public event EventHandler<ComplexityAscentPermissionEventArgs> ComplexityAscentPermissionChangeRequested;

        /// <summary>
        /// Raises a DestructionRequested event.
        /// </summary>
        protected void RaiseDestructionRequested() {
            if(DestructionRequested != null) {
                DestructionRequested(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises a AscensionPermissionChangeRequested event.
        /// </summary>
        /// <param name="ascensionPermitted">Whether the society should be permitted to ascend</param>
        protected void RaiseAscensionPermissionChangeRequested(bool ascensionPermitted) {
            if(AscensionPermissionChangeRequested != null) {
                AscensionPermissionChangeRequested(this, new BoolEventArgs(ascensionPermitted));
            }
        }

        /// <summary>
        /// Raises a ComplexityAscentPermissionChangeRequested event.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <param name="permittedToAscend">Whether the complexity should be given ascension permission</param>
        protected void RaiseComplexityAscentPermissionChangeRequested(ComplexityDefinitionBase complexity, bool permittedToAscend) {
            if(ComplexityAscentPermissionChangeRequested != null) {
                ComplexityAscentPermissionChangeRequested(this,
                    new ComplexityAscentPermissionEventArgs(complexity, permittedToAscend)
                );
            }
        }

        #endregion

    }

}
