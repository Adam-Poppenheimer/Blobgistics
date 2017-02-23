using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public interface IComplexityDefinition {

        #region properties

        string Name { get; }

        ResourceSummary Production { get; }
        ResourceSummary Needs { get; }
        IEnumerable<ResourceSummary> Wants { get; }

        uint ProductionCapacityCoefficient { get; }
        uint NeedsCapacityCoefficient { get; }
        uint WantsCapacityCoefficient { get; }

        float SecondsToPerformFullProduction { get; }
        float SecondsToFullyConsumeNeeds { get; }

        float ComplexityDescentDuration { get; }

        #endregion

    }

}