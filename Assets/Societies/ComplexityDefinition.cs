using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using System.Collections.ObjectModel;

namespace Assets.Societies {

    public class ComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from ComplexityDefinitionBase

        public override IntPerResourceDictionary Production {
            get {
                if(_production == null) {
                    _production = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _production;
            }
        }
        public void SetProduction(IntPerResourceDictionary value) {
            _production = value;
        }
        [SerializeField] private IntPerResourceDictionary _production;

        public override IntPerResourceDictionary Needs {
            get {
                if(_needs == null) {
                    _needs = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _needs;
            }
        }
        public void SetNeeds(IntPerResourceDictionary value) {
            _needs = value;
        }
        [SerializeField] private IntPerResourceDictionary _needs;

        public override IEnumerable<IntPerResourceDictionary> Wants {
            get { return _wants; }
        }
        public void SetWants(List<IntPerResourceDictionary> value) {
            _wants = value;
        }
        [SerializeField] private List<IntPerResourceDictionary> _wants= new List<IntPerResourceDictionary>();

        public override IntPerResourceDictionary CostToAscendInto {
            get {
                if(_costToAscendInto == null) {
                    _costToAscendInto = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _costToAscendInto;
            }
        }
        public void SetCostOfAscent(IntPerResourceDictionary value) {
            _costToAscendInto = value;
        }
        [SerializeField] private IntPerResourceDictionary _costToAscendInto;

        public override uint ProductionCapacityCoefficient {
            get { return _productionCapacityCoefficient; }
        }
        public void SetProductionCapacityCoefficient(uint value) {
            _productionCapacityCoefficient = value;
        }
        [SerializeField] private uint _productionCapacityCoefficient = 1;

        public override uint NeedsCapacityCoefficient {
            get { return _needsCapacityCoefficient; }
        }
        public void SetNeedsCapacityCoefficient(uint value) {
            _needsCapacityCoefficient = value;
        }
        [SerializeField] private uint _needsCapacityCoefficient = 1;

        public override uint WantsCapacityCoefficient {
            get { return _wantsCapacityCoefficient; }
        }
        public void SetWantsCapacityCoefficient(uint value) {
            _wantsCapacityCoefficient = value;
        }
        [SerializeField] private uint _wantsCapacityCoefficient = 1;

        public override float SecondsToPerformFullProduction {
            get { return _secondsToPerformFullProduction; }
        }
        public void SetSecondsToPerformFullProduction(float value) {
            _secondsToPerformFullProduction = value;
        }
        [SerializeField] private float _secondsToPerformFullProduction = 1f;

        public override float SecondsToFullyConsumeNeeds {
            get { return _secondsToFullyConsumeNeeds; }
        }
        public void SetSecondsToFullyConsumeNeeds(float value) {
            _secondsToFullyConsumeNeeds = value;
        }
        [SerializeField] private float _secondsToFullyConsumeNeeds = 1f;

        public override float ComplexityDescentDuration {
            get { return _complexityDescentDuration; }
        }
        public void SetComplexityDescentDuration(float value) {
            _complexityDescentDuration = value;
        }
        [SerializeField] private float _complexityDescentDuration = 10f;

        public override Sprite SpriteForSociety {
            get { return _spriteForSociety; }
        }
        public void SetColorForSociety(Sprite value) {
            _spriteForSociety = value;
        }
        [SerializeField] private Sprite _spriteForSociety;

        public override Color ColorForSociety {
            get { return _colorForSociety; }
        }
        public void SetColorForSociety(Color value) {
            _colorForSociety = value;
        }
        [SerializeField] private Color _colorForSociety;

        public override Color ColorForBackground {
            get { return _colorForBackground; }
        }
        public void SetColorForBackground(Color value) {
            _colorForBackground = value;
        }
        [SerializeField] private Color _colorForBackground;

        public override ReadOnlyCollection<TerrainType> PermittedTerrains {
            get { return _permittedTerrains.AsReadOnly(); }
        }
        public void SetPermittedTerrains(List<TerrainType> value) {
            _permittedTerrains = value;
        }
        [SerializeField] private List<TerrainType> _permittedTerrains;

        #endregion

        #endregion

    }

}
