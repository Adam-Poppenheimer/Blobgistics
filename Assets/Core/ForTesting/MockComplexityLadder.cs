using System;
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

        #endregion

        #region instance methods

        #region from ComplexityLadderBase

        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            return complexity == StartingComplexity || complexity == AscensionComplexity;
        }

        public override ComplexityDefinitionBase GetAscentTransition(ComplexityDefinitionBase currentComplexity) {
            if(currentComplexity == StartingComplexity) {
                return AscensionComplexity;
            }else {
                return null;
            }
        }

        public override ComplexityDefinitionBase GetDescentTransition(ComplexityDefinitionBase currentComplexity) {
            if(currentComplexity == AscensionComplexity) {
                return StartingComplexity;
            }else {
                return null;
            }
        }

        public override ComplexityDefinitionBase GetStartingComplexity() {
            return StartingComplexity;
        }

        #endregion

        #endregion
        
    }

}
