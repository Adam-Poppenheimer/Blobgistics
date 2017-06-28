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

        public abstract int ID { get; }

        public abstract ComplexityDefinitionBase CurrentComplexity  { get; }
        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        public abstract bool  NeedsAreSatisfied { get; }
        public abstract float SecondsOfUnsatisfiedNeeds { get; set; }
        public abstract float SecondsUntilComplexityDescent { get; }

        public abstract bool AscensionIsPermitted { get; set; }

        public abstract MapNodeBase Location { get; }

        #endregion

        #region events

        public event EventHandler<ComplexityDefinitionEventArgs> CurrentComplexityChanged;

        protected void RaiseCurrentComplexityChanged(ComplexityDefinitionBase newComplexity) {
            if(CurrentComplexityChanged != null) {
                CurrentComplexityChanged(this, new ComplexityDefinitionEventArgs(newComplexity));
            }
        }

        #endregion

        #region instance methods

        public abstract void TickProduction(float secondsPassed);
        public abstract void TickConsumption(float secondsPassed);

        public abstract IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent();

        #endregion

    }

}
