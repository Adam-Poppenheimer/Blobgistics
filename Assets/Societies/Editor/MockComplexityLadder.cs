using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobEngine;

namespace Assets.Societies.Editor {

    public class MockComplexityLadder : IComplexityLadder {

        #region instance fields and properties

        private readonly List<IComplexityDefinition> AscentChain;
        private readonly Dictionary<IComplexityDefinition, Dictionary<ResourceType, int>> Costs;

        #endregion

        #region constructors

        public MockComplexityLadder(List<IComplexityDefinition> ascentChain,
            Dictionary<IComplexityDefinition, Dictionary<ResourceType, int>> costs) {
            AscentChain = ascentChain;
            Costs = costs;
        }

        #endregion

        #region instance methods

        #region from IComplexityLadder

        public IComplexityDefinition GetAscentTransition(IComplexityDefinition currentComplexity) {
            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            if(indexOfCurrent == -1 || indexOfCurrent == AscentChain.Count - 1) {
                return AscentChain[0];
            }else {
                return AscentChain[indexOfCurrent + 1];
            }
        }

        public Dictionary<ResourceType, int> GetCostOfAscentTransition(IComplexityDefinition resultOfTransition) {
            throw new NotImplementedException();
        }

        public IComplexityDefinition GetDescentTransition(IComplexityDefinition currentComplexity) {
            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            return AscentChain[Math.Max(indexOfCurrent, 0)];
        }

        #endregion

        #endregion
        
    }

}
