using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Session.ForTesting {

    public class MockComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities {
            get {
                throw new NotImplementedException();
            }
        }

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
