using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public interface ISociety : IBlobSite {

        #region properties

        ISocietyComplexityDefinition CurrentComplexity { get; }
        ISocietyComplexityDefinition ComplexityBelow { get; }
        ISocietyComplexityDefinition ComplexityAbove { get; } 

        bool NeedsAreSatisfied { get; }
        float SecondsUntilComplexityDescent { get; }

        #endregion

        #region events



        #endregion

        #region methods

        void AscendComplexityLadder();
        void DescendComplexityLadder();

        void PerformProduction();
        void PerformConsumption();

        IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent();

        #endregion

    }

}
