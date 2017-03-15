using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Societies {

    public class ComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from ComplexityDefinitionBase

        public override string Name {
            get { return _name; }
        }
        public void SetName(string value) {
            _name = value;
        }
        [SerializeField] private string _name = "Default Complexity";

        public override ResourceSummary Production {
            get { return _production; }
        }
        public void SetProduction(ResourceSummary value) {
            _production = value;
        }
        [SerializeField] private ResourceSummary _production = ResourceSummary.Empty;

        public override ResourceSummary Needs {
            get { return _needs; }
        }
        public void SetNeeds(ResourceSummary value) {
            _needs = value;
        }
        [SerializeField] private ResourceSummary _needs = ResourceSummary.Empty;

        public override IEnumerable<ResourceSummary> Wants {
            get { return _wants; }
        }
        public void SetWants(List<ResourceSummary> value) {
            _wants = value;
        }
        [SerializeField] private List<ResourceSummary> _wants= new List<ResourceSummary>();

        public override ResourceSummary CostOfAscent {
            get { return _costOfAscent; }
        }
        public void SetCostOfAscent(ResourceSummary value) {
            _costOfAscent = value;
        }
        [SerializeField] private ResourceSummary _costOfAscent = ResourceSummary.Empty;

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

        #endregion

        #endregion
        
    }

}
