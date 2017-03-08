using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Societies {

    public abstract class ComplexityLadderBase : MonoBehaviour {

        #region instance methods

        public abstract ComplexityDefinitionBase GetStartingComplexity();

        public abstract ComplexityDefinitionBase GetAscentTransition(ComplexityDefinitionBase currentComplexity);
        public abstract ComplexityDefinitionBase GetDescentTransition(ComplexityDefinitionBase currentComplexity);

        #endregion

    }

}