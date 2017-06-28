using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public abstract class ComplexityLadderBase : MonoBehaviour {

        #region instance methods

        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity);
        public abstract ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity);

        public abstract bool ContainsComplexity(ComplexityDefinitionBase complexity);

        #endregion

    }

}