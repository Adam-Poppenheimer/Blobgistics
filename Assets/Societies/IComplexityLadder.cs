using System;
using System.Collections.Generic;

using Assets.BlobEngine;

namespace Assets.Societies {

    public interface IComplexityLadder {

        #region methods

        IComplexityDefinition GetAscentTransition(IComplexityDefinition currentComplexity);
        IComplexityDefinition GetDescentTransition(IComplexityDefinition currentComplexity);

        Dictionary<ResourceType, int> GetCostOfAscentTransition(IComplexityDefinition resultOfTransition);

        #endregion

    }

}