using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public interface ISociety : IBlobSite {

        #region properties

        IComplexityDefinition CurrentComplexity  { get; }
        IComplexityLadder ActiveComplexityLadder { get; }

        bool NeedsAreSatisfied { get; }
        float SecondsOfUnsatisfiedNeeds { get; }
        float SecondsUntilComplexityDescent { get; }

        #endregion

        #region events



        #endregion

        #region methods

        void AscendComplexityLadder();
        void DescendComplexityLadder();

        void TickProduction(float secondsPassed);
        void TickConsumption(float secondsPassed);

        IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent();

        #endregion

    }

}
