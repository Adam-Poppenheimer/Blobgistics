using System;
using System.Collections.Generic;

using Assets.Blobs;

namespace Assets.Societies {

    public abstract class ComplexityLadderBase {

        #region instance methods

        public abstract ComplexityDefinitionBase GetAscentTransition(ComplexityDefinitionBase currentComplexity);
        public abstract ComplexityDefinitionBase GetDescentTransition(ComplexityDefinitionBase currentComplexity);

        #endregion

    }

}