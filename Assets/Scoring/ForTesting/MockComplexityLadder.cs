using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Scoring.ForTesting {

    public class MockComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        #region from ComplexityLadderBase

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities {
            get {
                return tierOneComplexities.AsReadOnly();
            }
        }
        public List<ComplexityDefinitionBase> tierOneComplexities = new List<ComplexityDefinitionBase>();

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities {
            get {
                return tierTwoComplexities.AsReadOnly();
            }
        }
        public List<ComplexityDefinitionBase> tierTwoComplexities = new List<ComplexityDefinitionBase>();

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities {
            get {
                return tierThreeComplexities.AsReadOnly();
            }
        }
        public List<ComplexityDefinitionBase> tierThreeComplexities = new List<ComplexityDefinitionBase>();

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities {
            get {
                return tierFourComplexities.AsReadOnly();
            }
        }
        public List<ComplexityDefinitionBase> tierFourComplexities = new List<ComplexityDefinitionBase>();

        #endregion

        #endregion

        #region instance methods

        #region from ComplexityLadderBase

        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity) {
            throw new NotImplementedException();
        }

        public override int GetTierOfComplexity(ComplexityDefinitionBase complexity) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
