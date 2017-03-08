using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public abstract class SocietyBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ComplexityDefinitionBase CurrentComplexity  { get; }
        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        public abstract bool  NeedsAreSatisfied { get; }
        public abstract float SecondsOfUnsatisfiedNeeds { get; }
        public abstract float SecondsUntilComplexityDescent { get; }

        public abstract MapNodeBase Location { get; }

        #endregion

        #region instance methods

        public abstract void TickProduction(float secondsPassed);
        public abstract void TickConsumption(float secondsPassed);

        public abstract IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent();

        #endregion

    }

}
