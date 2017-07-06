using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;

namespace Assets.Societies {

    public abstract class ComplexityDefinitionBase : MonoBehaviour {

        #region instance fields and properties

        public abstract IntPerResourceDictionary Production { get; }
        public abstract IntPerResourceDictionary Needs { get; }
        public abstract IEnumerable<IntPerResourceDictionary> Wants { get; }

        public abstract IntPerResourceDictionary CostToAscendInto { get; }

        public abstract uint ProductionCapacityCoefficient { get; }
        public abstract uint NeedsCapacityCoefficient { get; }
        public abstract uint WantsCapacityCoefficient { get; }

        public abstract float SecondsToPerformFullProduction { get; }
        public abstract float SecondsToFullyConsumeNeeds { get; }

        public abstract float ComplexityDescentDuration { get; }

        public abstract ReadOnlyCollection<TerrainType> PermittedTerrains { get; }

        public abstract Sprite SpriteForSociety { get; }
        public abstract Color ColorForSociety { get; }
        public abstract Color ColorForBackground { get; }

        #endregion

    }

}