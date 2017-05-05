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

        public override string Name {
            get { return _name; }
        }
        public void SetName(string value) {
            _name = value;
        }
        private string _name = "DEFAULT";

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
        private IntResourceSummary _needs = null;

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
        private IntResourceSummary _production = null;

        public override IEnumerable<IntResourceSummary> Wants {
            get { return _wants; }
        }
        public void SetWants(IEnumerable<IntResourceSummary> value) {
            _wants = value;
        }
        private IEnumerable<IntResourceSummary> _wants = new List<IntResourceSummary>();

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

        public override IntResourceSummary CostToAscendInto {
            get {
                if(_costOfAscent == null) {
                    _costOfAscent = IntResourceSummary.BuildSummary(gameObject);
                }
                return _costOfAscent;
            }
        }
        public void SetCostOfAscent(IntResourceSummary value) {
            _costOfAscent = value;
        }
        private IntResourceSummary _costOfAscent = null;

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

        #endregion

        #endregion

    }

}
