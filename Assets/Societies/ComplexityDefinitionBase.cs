using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public abstract class ComplexityDefinitionBase {

        #region instance fields and properties

        public abstract string Name { get; }

        public abstract ResourceSummary Production { get; }
        public abstract ResourceSummary Needs { get; }
        public abstract IEnumerable<ResourceSummary> Wants { get; }

        public abstract ResourceSummary CostOfAscent { get; }

        public abstract uint ProductionCapacityCoefficient { get; }
        public abstract uint NeedsCapacityCoefficient { get; }
        public abstract uint WantsCapacityCoefficient { get; }

        public abstract float SecondsToPerformFullProduction { get; }
        public abstract float SecondsToFullyConsumeNeeds { get; }

        public abstract float ComplexityDescentDuration { get; }

        #endregion

    }

}