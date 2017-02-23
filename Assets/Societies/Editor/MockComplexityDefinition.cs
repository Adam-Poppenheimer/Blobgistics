using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobEngine;
using UnityCustomUtilities.Extensions;

namespace Assets.Societies.Editor {

    public class MockComplexityDefinition : IComplexityDefinition {

        #region instance fields and properties

        #region from IComplexityDefinition

        public float ComplexityDescentDuration {
            get { return _complexityDescentDuration; }
            set { _complexityDescentDuration = value; }
        }
        private float _complexityDescentDuration = 1f;

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        private string _name = "DEFAULT";

        public ResourceSummary Needs {
            get { return _needs; }
            set { _needs = value; }
        }
        private ResourceSummary _needs = ResourceSummary.Empty;

        public ResourceSummary Production {
            get { return _production; }
            set { _production = value; }
        }
        private ResourceSummary _production = ResourceSummary.Empty;

        public IEnumerable<ResourceSummary> Wants {
            get { return _wants; }
            set { _wants = value; }
        }
        private IEnumerable<ResourceSummary> _wants = new List<ResourceSummary>();

        public uint ProductionCapacityCoefficient {
            get { return _productionCapacityCoefficient; }
            set { _productionCapacityCoefficient = value; }
        }
        private uint _productionCapacityCoefficient = 1;

        public uint NeedsCapacityCoefficient {
            get { return _needsCapacityCoefficient; }
            set { _needsCapacityCoefficient = value; }
        }
        private uint _needsCapacityCoefficient = 1;

        public uint WantsCapacityCoefficient {
            get { return _wantsCapacityCoefficient; }
            set { _wantsCapacityCoefficient = value; }
        }
        private uint _wantsCapacityCoefficient;

        public float SecondsToPerformFullProduction {
            get { return _secondsToPerformFullProduction; }
            set { _secondsToPerformFullProduction = value; }
        }
        private float _secondsToPerformFullProduction = 1f;

        public float SecondsToFullyConsumeNeeds {
            get { return _secondsToFullyConsumeNeeds; }
            set { _secondsToFullyConsumeNeeds = value; }
        }
        private float _secondsToFullyConsumeNeeds = 1f;

        #endregion

        #endregion

        #region constructors

        public MockComplexityDefinition() { }

        #endregion

    }

}
