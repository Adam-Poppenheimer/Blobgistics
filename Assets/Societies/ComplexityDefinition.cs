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

        public override string Name {
            get { return name; }
        }

        public override IntResourceSummary Production {
            get {
                if(_production == null) {
                    _production = IntResourceSummary.BuildSummary(gameObject);
                }
                return _production;
            }
        }
        public void SetProduction(IntResourceSummary value) {
            _production = value;
        }
        [SerializeField] private IntResourceSummary _production;

        public override IntResourceSummary Needs {
            get {
                if(_needs == null) {
                    _needs = IntResourceSummary.BuildSummary(gameObject);
                }
                return _needs;
            }
        }
        public void SetNeeds(IntResourceSummary value) {
            _needs = value;
        }
        [SerializeField] private IntResourceSummary _needs;

        public override IEnumerable<IntResourceSummary> Wants {
            get { return _wants; }
        }
        public void SetWants(List<IntResourceSummary> value) {
            _wants = value;
        }
        [SerializeField] private List<IntResourceSummary> _wants= new List<IntResourceSummary>();

        public override IntResourceSummary CostToAscendInto {
            get {
                if(_costToAscendInto == null) {
                    _costToAscendInto = IntResourceSummary.BuildSummary(gameObject);
                }
                return _costToAscendInto;
            }
        }
        public void SetCostOfAscent(IntResourceSummary value) {
            _costToAscendInto = value;
        }
        [SerializeField] private IntResourceSummary _costToAscendInto;

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

        public override Material MaterialForSociety {
            get { return _materialForSociety; }
        }
        public void SetMaterialForSociety(Material value) {
            _materialForSociety = value;
        }
        [SerializeField] private Material _materialForSociety;

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
