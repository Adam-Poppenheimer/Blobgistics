using System;

using Assets.BlobEngine;

namespace Assets.Societies {

    public class AscentSummary {

        #region instance fields and properties

        public readonly IComplexityDefinition CurrentComplexity;
        public readonly IComplexityDefinition ComplexityAbove;
        public readonly ResourceSummary Cost;

        #endregion

        #region constructors

        public AscentSummary(IComplexityDefinition currentComplexity,
            IComplexityDefinition complexityAbove, ResourceSummary cost) {
            CurrentComplexity = currentComplexity;
            ComplexityAbove = complexityAbove;
            Cost = cost;
        }

        #endregion

    }

}