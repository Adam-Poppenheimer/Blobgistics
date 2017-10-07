using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for all societies, which are the primary agent of the game.
    /// Societies produce and consume resources and can complexify or decomplexify into 
    /// different kinds of societies. They are also the metric for success in the game.
    /// </summary>
    public abstract class SocietyBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The SocietyBase-unique ID of the society.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The current complexity that informs the society's behavior.
        /// </summary>
        public abstract ComplexityDefinitionBase CurrentComplexity  { get; }

        /// <summary>
        /// The complexity ladder the society is currently on.
        /// </summary>
        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        /// <summary>
        /// Whether or not the society's needs are currently satisfied.
        /// </summary>
        public abstract bool  NeedsAreSatisfied { get; protected set; }

        /// <summary>
        /// How long the society has had unsatisfied needs.
        /// </summary>
        public abstract float SecondsOfUnsatisfiedNeeds { get; set; }

        /// <summary>
        /// How long until the society's lack of satisfied needs causes it to descend
        /// the complexity ladder.
        /// </summary>
        public abstract float SecondsUntilComplexityDescent { get; }

        /// <summary>
        /// Whether or not the society is permitted to ascend.
        /// </summary>
        public abstract bool AscensionIsPermitted { get; set; }

        /// <summary>
        /// The location of the society.
        /// </summary>
        public abstract MapNodeBase Location { get; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever the society's current complexity changes.
        /// </summary>
        public event EventHandler<ComplexityDefinitionEventArgs> CurrentComplexityChanged;

        /// <summary>
        /// Fires whenever the society's needs become satisfied or unsatisfied.
        /// </summary>
        public event EventHandler<BoolEventArgs> NeedsAreSatisfiedChanged;

        /// <summary>
        /// Fires the CurrentComplexityChanged event.
        /// </summary>
        /// <param name="newComplexity">The society's new complexity</param>
        protected void RaiseCurrentComplexityChanged(ComplexityDefinitionBase newComplexity) {
            if(CurrentComplexityChanged != null) {
                CurrentComplexityChanged(this, new ComplexityDefinitionEventArgs(newComplexity));
            }
        }

        /// <summary>
        /// Fires the NeedsAreSatisfiedChanged event.
        /// </summary>
        /// <param name="needsAreSatisfied">Whether the society's needs are now satisfied</param>
        protected void RaiseNeedsAreSatisfiedChanged(bool needsAreSatisfied) {
            if(NeedsAreSatisfiedChanged != null) {
                NeedsAreSatisfiedChanged(this, new BoolEventArgs(needsAreSatisfied));
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Increments the production timer of the society by the given number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds to advance the timer by</param>
        public abstract void TickProduction(float secondsPassed);

        /// <summary>
        /// Increments the consumption timer of the society by the given number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds to advance the timer by</param>
        public abstract void TickConsumption(float secondsPassed);

        /// <summary>
        /// Gets whether the given society is a valid ascension target.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <returns>Whether the society is permitting ascension to that complexity</returns>
        public abstract bool GetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity);

        /// <summary>
        /// Sets whether the given society is a valid ascension target.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <param name="isPermitted">Whether ascension into it is now permitted</param>
        public abstract void SetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity, bool isPermitted);

        #endregion

    }

}
