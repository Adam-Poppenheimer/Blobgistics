using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for all complexity definitions. A complexity definition is
    /// essentially a type of society, and informs a given society instance how it should
    /// look and act.
    /// </summary>
    /// <remarks>
    /// ComplexityDefinitions are often referred to as Complexities for brevity. They do
    /// not represent different concepts.
    /// </remarks>
    public abstract class ComplexityDefinitionBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The production of the complexity every production cycle.
        /// </summary>
        public abstract IntPerResourceDictionary Production { get; }

        /// <summary>
        /// The needs the complexity must consume every consumption cycle to maintain itself.
        /// </summary>
        public abstract IntPerResourceDictionary Needs { get; }

        /// <summary>
        /// The wants the complexity can consume to increase their production.
        /// </summary>
        public abstract IEnumerable<IntPerResourceDictionary> Wants { get; }

        /// <summary>
        /// The cost to ascend into this society from a less complex one.
        /// </summary>
        public abstract IntPerResourceDictionary CostToAscendInto { get; }

        /// <summary>
        /// The multiple of its production a society of this complexity should store.
        /// </summary>
        public abstract uint ProductionCapacityCoefficient { get; }

        /// <summary>
        /// The multiple of its needs a society of this complexity should store.
        /// </summary>
        public abstract uint NeedsCapacityCoefficient { get; }

        /// <summary>
        /// The multiple of its wants a society of this complexity should store.
        /// </summary>
        public abstract uint WantsCapacityCoefficient { get; }

        /// <summary>
        /// The amount of time it takes to perform a full production cycle, which
        /// incudes the consumption of wants.
        /// </summary>
        public abstract float SecondsToPerformFullProduction { get; }

        /// <summary>
        /// The amount of time it takes to perform a full consumption cycle.
        /// </summary>
        public abstract float SecondsToFullyConsumeNeeds { get; }

        /// <summary>
        /// The amount of time a society of this complexity must have unsatisfied needs
        /// before it decomplexifies.
        /// </summary>
        public abstract float ComplexityDescentDuration { get; }

        /// <summary>
        /// The terrains this complexity is permitted to occupy.
        /// </summary>
        public abstract ReadOnlyCollection<TerrainType> PermittedTerrains { get; }

        /// <summary>
        /// The sprite that should be used to display a society of this complexity.
        /// </summary>
        public abstract Sprite SpriteForSociety { get; }

        /// <summary>
        /// The color that should be applied to the sprite.
        /// </summary>
        public abstract Color ColorForSociety { get; }

        /// <summary>
        /// The color that should be painted into the background of the sprite.
        /// </summary>
        public abstract Color ColorForBackground { get; }

        #endregion

    }

}