using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Societies {

    /// <summary>
    /// The standard implementation of ComplexityLadderBase. This defines how societies
    /// can ascend and descend from their current complexity.
    /// </summary>
    /// <remarks>
    /// This implementation asserts that any society can complexify into any complexity one
    /// above its current tier, and can descend into any complexity one below its current
    /// tier. Given this implementation, ComplexityLadder might be redundant. It might make
    /// more sense to put transition information on the complexities themselves.
    /// </remarks>
    public class ComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities {
            get { return tierOneComplexities.AsReadOnly(); }
        }
        [SerializeField] private List<ComplexityDefinitionBase> tierOneComplexities;

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities {
            get { return tierTwoComplexities.AsReadOnly(); }
        }
        [SerializeField] private List<ComplexityDefinitionBase> tierTwoComplexities;

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities {
            get { return tierThreeComplexities.AsReadOnly(); }
        }
        [SerializeField] private List<ComplexityDefinitionBase> tierThreeComplexities;

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities {
            get { return tierFourComplexities.AsReadOnly(); }
        }
        [SerializeField] private List<ComplexityDefinitionBase> tierFourComplexities;

        private List<ComplexityDefinitionBase> EmptyComplexityList = new List<ComplexityDefinitionBase>();

        #endregion

        #region instance methods

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(tierOneComplexities.Contains(currentComplexity)){
                return tierTwoComplexities.AsReadOnly();
            }else if(tierTwoComplexities.Contains(currentComplexity)) {
                return tierThreeComplexities.AsReadOnly();
            }else if(tierThreeComplexities.Contains(currentComplexity)) {
                return tierFourComplexities.AsReadOnly();
            }else {
                return EmptyComplexityList.AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public override ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(tierFourComplexities.Contains(currentComplexity)) {
                return tierThreeComplexities.AsReadOnly();
            }else if(tierThreeComplexities.Contains(currentComplexity)) {
                return tierTwoComplexities.AsReadOnly();
            }else if(tierTwoComplexities.Contains(currentComplexity)) {
                return tierOneComplexities.AsReadOnly();
            }else {
                return EmptyComplexityList.AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            return (
                tierOneComplexities.Contains  (complexity) || 
                tierTwoComplexities.Contains  (complexity) ||
                tierThreeComplexities.Contains(complexity) || 
                tierFourComplexities.Contains (complexity)
            );
        }

        /// <inheritdoc/>
        public override int GetTierOfComplexity(ComplexityDefinitionBase complexity) {
            if(tierFourComplexities.Contains(complexity)) {
                return 4;
            }else if(tierThreeComplexities.Contains(complexity)) {
                return 3;
            }else if(tierTwoComplexities.Contains(complexity)) {
                return 2;
            }else if(tierOneComplexities.Contains(complexity)){
                return 1;
            }else {
                return -1;
            }
        }

        #endregion

    }

}
