using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public abstract class SocietyBase {

        #region instance fields and properties

        public abstract ComplexityDefinitionBase CurrentComplexity  { get; protected set; }
        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        public abstract bool NeedsAreSatisfied { get; protected set; }
        public abstract float SecondsOfUnsatisfiedNeeds { get; protected set; }
        public abstract float SecondsUntilComplexityDescent { get; }

        #endregion

        #region events



        #endregion

        #region instance methods

        public abstract void AscendComplexityLadder();
        public abstract void DescendComplexityLadder();

        public abstract void TickProduction(float secondsPassed);
        public abstract void TickConsumption(float secondsPassed);

        public abstract IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent();

        #endregion

    }

}
