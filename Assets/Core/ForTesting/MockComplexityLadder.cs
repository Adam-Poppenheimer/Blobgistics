using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Core.ForTesting {

    public class MockComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        private ComplexityDefinitionBase StartingComplexity {
            get {
                if(_startingComplexity == null) {
                    _startingComplexity = gameObject.AddComponent<MockComplexityDefinition>();
                }
                return _startingComplexity;
            }
        }
        private ComplexityDefinitionBase _startingComplexity;

        private ComplexityDefinitionBase AscensionComplexity {
            get {
                if(_ascensionComplexity == null) {
                    _ascensionComplexity = gameObject.AddComponent<MockComplexityDefinition>();
                }
                return _ascensionComplexity;
            }
        }
        private ComplexityDefinitionBase _ascensionComplexity;

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierOneComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierTwoComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierThreeComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> TierFourComplexities {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region instance methods

        #region from ComplexityLadderBase

        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            return complexity == StartingComplexity || complexity == AscensionComplexity;
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(currentComplexity == StartingComplexity) {
                return new List<ComplexityDefinitionBase>() { AscensionComplexity }.AsReadOnly();
            }else {
                return null;
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(currentComplexity == AscensionComplexity) {
                return new List<ComplexityDefinitionBase>() { StartingComplexity }.AsReadOnly();
            }else {
                return null;
            }
        }

        public override int GetTierOfComplexity(ComplexityDefinitionBase complexity) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
