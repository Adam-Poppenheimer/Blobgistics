using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using UnityCustomUtilities.Extensions;

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

        public override ResourceSummary Needs {
            get { return _needs; }
        }
        public void SetNeeds(ResourceSummary value) {
            _needs = value;
        }
        private ResourceSummary _needs = ResourceSummary.Empty;

        public override ResourceSummary Production {
            get { return _production; }
        }
        public void SetProduction(ResourceSummary value) {
            _production = value;
        }
        private ResourceSummary _production = ResourceSummary.Empty;

        public override IEnumerable<ResourceSummary> Wants {
            get { return _wants; }
        }
        public void SetWants(IEnumerable<ResourceSummary> value) {
            _wants = value;
        }
        private IEnumerable<ResourceSummary> _wants = new List<ResourceSummary>();

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

        public override ResourceSummary CostOfAscent {
            get { return _costOfAscent; }
        }
        public void SetCostOfAscent(ResourceSummary value) {
            _costOfAscent = value;
        }
        private ResourceSummary _costOfAscent = ResourceSummary.Empty;

        #endregion

        #endregion

    }

}
