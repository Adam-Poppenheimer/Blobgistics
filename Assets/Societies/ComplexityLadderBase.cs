using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for all complexity ladders, which define how societies
    /// can ascend and descend from their current complexity.
    /// </summary>
    public abstract class ComplexityLadderBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All the tier 1 complexities in the ladder.
        /// </summary>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities   { get; }

        /// <summary>
        /// All the tier 2 complexities in the ladder.
        /// </summary>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities   { get; }

        /// <summary>
        /// All the tier 3 complexities in the ladder.
        /// </summary>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities { get; }

        /// <summary>
        /// All the tier 4 complexities in the ladder.
        /// </summary>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities  { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets all of the complexities that a society of the given complexity can ascend into.
        /// </summary>
        /// <param name="currentComplexity">The complexity to find the transitions for</param>
        /// <returns>All of the complexities that a society of the given complexity can ascend into</returns>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity);

        /// <summary>
        /// Gets all of the complexities that a society of the given complexity can descend into.
        /// </summary>
        /// <param name="currentComplexity">The complexity to find the transitions for</param>
        /// <returns>All of the complexities that a society of the given complexity can descend into</returns>
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity);

        /// <summary>
        /// Determines whether the given complexity is present in this complexity ladder.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <returns>Where it's in the ladder</returns>
        public abstract bool ContainsComplexity(ComplexityDefinitionBase complexity);

        /// <summary>
        /// Get the complexity tier of the given complexity.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <returns>Its tier in this ladder</returns>
        public abstract int GetTierOfComplexity(ComplexityDefinitionBase complexity);

        #endregion

    }

}