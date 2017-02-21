using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public interface ISocietyComplexityDefinition {

        #region properties

        string Name { get; }

        IReadOnlyDictionary<ResourceType, int> Production { get; }
        IReadOnlyDictionary<ResourceType, int> Needs { get; }
        IReadOnlyDictionary<ResourceType, int> Wants { get; }

        float ComplexityDescentDuration { get; }

        #endregion

    }

}