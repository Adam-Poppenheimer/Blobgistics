using System;
using System.Collections.Generic;

using Assets.BlobEngine;

namespace Assets.Societies {

    public interface IComplexityLadder {

        #region methods

        AscentSummary GetAscentTransition(IComplexityDefinition currentComplexity);
        IComplexityDefinition GetDescentTransition(IComplexityDefinition currentComplexity);

        #endregion

    }

}