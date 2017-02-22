using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

namespace Assets.Societies.Editor {

    public class MockComplexityLadder : IComplexityLadder {

        #region instance fields and properties

        public List<IComplexityDefinition> AscentChain { get; set; }
        public Dictionary<IComplexityDefinition, ResourceSummary> CostsToReach { get; set; }

        #endregion

        #region constructors

        public MockComplexityLadder() { }

        #endregion

        #region instance methods

        #region from IComplexityLadder

        public AscentSummary GetAscentTransition(IComplexityDefinition currentComplexity) {
            if(AscentChain == null) {
                return new AscentSummary(currentComplexity, null, ResourceSummary.Empty);
            }

            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            if(indexOfCurrent == -1 || indexOfCurrent == AscentChain.Count - 1) {
                return new AscentSummary(currentComplexity, AscentChain[0], ResourceSummary.Empty);
            }else {
                var nextComplexity = AscentChain[indexOfCurrent + 1];
                ResourceSummary costsForTransition = ResourceSummary.Empty;
                CostsToReach.TryGetValue(nextComplexity, out costsForTransition);

                return new AscentSummary(currentComplexity, nextComplexity, costsForTransition);
            }
        }

        public IComplexityDefinition GetDescentTransition(IComplexityDefinition currentComplexity) {
            if(AscentChain == null) {
                return null;
            }
            var indexOfCurrent = AscentChain.FindIndex(x => x == currentComplexity);
            return AscentChain[Math.Max(indexOfCurrent, 0)];
        }

        #endregion

        #endregion
        
    }

}
