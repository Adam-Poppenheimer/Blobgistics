using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public abstract class ComplexityLadderBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities   { get; }
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities   { get; }
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities { get; }
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities  { get; }

        #endregion

        #region instance methods

        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity);
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity);

        public abstract bool ContainsComplexity(ComplexityDefinitionBase complexity);

        public abstract int GetTierOfComplexity(ComplexityDefinitionBase complexity);

        #endregion

    }

}