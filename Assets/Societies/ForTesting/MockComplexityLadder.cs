using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Societies.ForTesting {

    public class MockComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        public List<ComplexityDefinitionBase> AscentChain {
            get { return _ascentChain; }
            set { _ascentChain = value; }
        }
        private List<ComplexityDefinitionBase> _ascentChain = new List<ComplexityDefinitionBase>() { null };

        #endregion

        #region constructors

        public MockComplexityLadder() { }

        #endregion

        #region instance methods

        #region from IComplexityLadder

        public override ComplexityDefinitionBase GetAscentTransition(ComplexityDefinitionBase currentComplexity) {
            if(AscentChain == null) {
                return null;
            }
            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            if(indexOfCurrent == -1 || indexOfCurrent == AscentChain.Count - 1) {
                return currentComplexity;
            }else {
                return AscentChain[indexOfCurrent + 1];
            }
        }

        public override ComplexityDefinitionBase GetDescentTransition(ComplexityDefinitionBase currentComplexity) {
            if(AscentChain == null) {
                return null;
            }
            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            if(indexOfCurrent > 0) {
                return AscentChain[indexOfCurrent - 1];
            }else {
                return null;
            }
        }

        public override ComplexityDefinitionBase GetStartingComplexity() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
