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
        [SerializeField] private string _name;

        public override ResourceSummary Production {
            get { return _production; }
        }
        [SerializeField] private ResourceSummary _production;

        public override ResourceSummary Needs {
            get { return _needs; }
        }
        [SerializeField] private ResourceSummary _needs;

        public override IEnumerable<ResourceSummary> Wants {
            get { return _wants; }
        }
        [SerializeField] private List<ResourceSummary> _wants;

        public override ResourceSummary CostOfAscent {
            get { return _costOfAscent; }
        }
        [SerializeField] private ResourceSummary _costOfAscent;

        public override uint ProductionCapacityCoefficient {
            get { return _productionCapacityCoefficient; }
        }
        [SerializeField] private uint _productionCapacityCoefficient;

        public override uint NeedsCapacityCoefficient {
            get { return _needsCapacityCoefficient; }
        }
        [SerializeField] private uint _needsCapacityCoefficient;

        public override uint WantsCapacityCoefficient {
            get { return _wantsCapacityCoefficient; }
        }
        [SerializeField] private uint _wantsCapacityCoefficient;

        public override float SecondsToPerformFullProduction {
            get { return _secondsToPerformFullProduction; }
        }
        [SerializeField] private float _secondsToPerformFullProduction;

        public override float SecondsToFullyConsumeNeeds {
            get { return _secondsToFullyConsumeNeeds; }
        }
        [SerializeField] private float _secondsToFullyConsumeNeeds;


        public override float ComplexityDescentDuration {
            get { return _complexityDescentDuration; }
        }
        [SerializeField] private float _complexityDescentDuration;

        #endregion

        #endregion
        
    }

}
