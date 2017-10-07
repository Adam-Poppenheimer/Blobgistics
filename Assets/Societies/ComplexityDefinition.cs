using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using System.Collections.ObjectModel;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for all complexity definitions. A complexity definition is
    /// essentially a type of society, and informs a given society instance how it should
    /// look and act.
    /// </summary>
    public class ComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from ComplexityDefinitionBase

        /// <inheritdoc/>
        public override IntPerResourceDictionary Production {
            get {
                if(_production == null) {
                    _production = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _production;
            }
        }
        [SerializeField] private IntPerResourceDictionary _production;

        /// <inheritdoc/>
        public override IntPerResourceDictionary Needs {
            get {
                if(_needs == null) {
                    _needs = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _needs;
            }
        }
        [SerializeField] private IntPerResourceDictionary _needs;

        /// <inheritdoc/>
        public override IEnumerable<IntPerResourceDictionary> Wants {
            get { return _wants; }
        }
        [SerializeField] private List<IntPerResourceDictionary> _wants= new List<IntPerResourceDictionary>();

        /// <inheritdoc/>
        public override IntPerResourceDictionary CostToAscendInto {
            get {
                if(_costToAscendInto == null) {
                    _costToAscendInto = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _costToAscendInto;
            }
        }
        [SerializeField] private IntPerResourceDictionary _costToAscendInto;

        /// <inheritdoc/>
        public override uint ProductionCapacityCoefficient {
            get { return _productionCapacityCoefficient; }
        }
        [SerializeField] private uint _productionCapacityCoefficient = 1;

        /// <inheritdoc/>
        public override uint NeedsCapacityCoefficient {
            get { return _needsCapacityCoefficient; }
        }
        [SerializeField] private uint _needsCapacityCoefficient = 1;

        /// <inheritdoc/>
        public override uint WantsCapacityCoefficient {
            get { return _wantsCapacityCoefficient; }
        }
        [SerializeField] private uint _wantsCapacityCoefficient = 1;

        /// <inheritdoc/>
        public override float SecondsToPerformFullProduction {
            get { return _secondsToPerformFullProduction; }
        }
        [SerializeField] private float _secondsToPerformFullProduction = 1f;

        /// <inheritdoc/>
        public override float SecondsToFullyConsumeNeeds {
            get { return _secondsToFullyConsumeNeeds; }
        }
        [SerializeField] private float _secondsToFullyConsumeNeeds = 1f;

        /// <inheritdoc/>
        public override float ComplexityDescentDuration {
            get { return _complexityDescentDuration; }
        }
        [SerializeField] private float _complexityDescentDuration = 10f;

        /// <inheritdoc/>
        public override Sprite SpriteForSociety {
            get { return _spriteForSociety; }
        }
        [SerializeField] private Sprite _spriteForSociety;

        /// <inheritdoc/>
        public override Color ColorForSociety {
            get { return _colorForSociety; }
        }
        [SerializeField] private Color _colorForSociety;

        /// <inheritdoc/>
        public override Color ColorForBackground {
            get { return _colorForBackground; }
        }
        [SerializeField] private Color _colorForBackground;

        /// <inheritdoc/>
        public override ReadOnlyCollection<TerrainType> PermittedTerrains {
            get { return _permittedTerrains.AsReadOnly(); }
        }
        [SerializeField] private List<TerrainType> _permittedTerrains;

        #endregion

        #endregion

    }

}
