using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;
using UnityCustomUtilities.Extensions;
using UnityEngine;

namespace Assets.Societies.ForTesting {

    public class MockComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from IComplexityDefinition

        public override float ComplexityDescentDuration {
            get { return _complexityDescentDuration; }
        }
        public void SetComplexityDescentDuration(float value) {
            _complexityDescentDuration = value;
        }
        private float _complexityDescentDuration = 1f;

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
        private IntPerResourceDictionary _needs = null;

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
        private IntPerResourceDictionary _production = null;

        public override IEnumerable<IntPerResourceDictionary> Wants {
            get { return _wants; }
        }
        public void SetWants(IEnumerable<IntPerResourceDictionary> value) {
            _wants = value;
        }
        private IEnumerable<IntPerResourceDictionary> _wants = new List<IntPerResourceDictionary>();

        public override uint ProductionCapacityCoefficient {
            get { return _productionCapacityCoefficient; }
        }
        public void SetProductionCapacityCoefficient(uint value) {
            _productionCapacityCoefficient = value;
        }
        private uint _productionCapacityCoefficient = 1;

        public override uint NeedsCapacityCoefficient {
            get { return _needsCapacityCoefficient; }
        }
        public void SetNeedsCapacityCoefficient(uint value) {
            _needsCapacityCoefficient = value;
        }
        public uint _needsCapacityCoefficient = 1;

        public override uint WantsCapacityCoefficient {
            get { return _wantsCapacityCoefficient; }
        }
        public void SetWantsCapacityCoefficient(uint value) {
            _wantsCapacityCoefficient = value;
        }
        private uint _wantsCapacityCoefficient = 1;

        public override float SecondsToPerformFullProduction {
            get { return _secondsToPerformFullProduction; }
        }
        public void SetSecondsToPerformFullProduction(float value) {
            _secondsToPerformFullProduction = value;
        }
        private float _secondsToPerformFullProduction = 1f;

        public override float SecondsToFullyConsumeNeeds {
            get { return _secondsToFullyConsumeNeeds; }
        }
        public void SetSecondsToFullyConsumeNeeds(float value) {
            _secondsToFullyConsumeNeeds = value;
        }
        private float _secondsToFullyConsumeNeeds = 1f;

        public override IntPerResourceDictionary CostToAscendInto {
            get {
                if(_costOfAscent == null) {
                    _costOfAscent = IntPerResourceDictionary.BuildSummary(gameObject);
                }
                return _costOfAscent;
            }
        }
        public void SetCostOfAscent(IntPerResourceDictionary value) {
            _costOfAscent = value;
        }
        private IntPerResourceDictionary _costOfAscent = null;

        public override Material MaterialForSociety {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<TerrainType> PermittedTerrains {
            get { return _permittedTerrains.AsReadOnly(); }
        }
        public void SetPermittedTerrains(List<TerrainType> value) {
            _permittedTerrains = value;
        }
        private List<TerrainType> _permittedTerrains = new List<TerrainType>();

        public override int Score {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
