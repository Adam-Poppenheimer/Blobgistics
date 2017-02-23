using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

namespace Assets.Societies.Editor {

    public class MockComplexityLadder : IComplexityLadder {

        #region instance fields and properties

        public List<IComplexityDefinition> AscentChain {
            get { return _ascentChain; }
            set { _ascentChain = value; }
        }
        private List<IComplexityDefinition> _ascentChain = new List<IComplexityDefinition>() { null };

        public Dictionary<IComplexityDefinition, ResourceSummary> CostsToReach {
            get { return _costsToReach; }
            set { _costsToReach = value; }
        }
        private Dictionary<IComplexityDefinition, ResourceSummary> _costsToReach =
            new Dictionary<IComplexityDefinition, ResourceSummary>();

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
                return new AscentSummary(currentComplexity, null, ResourceSummary.Empty);
            }else {
                var nextComplexity = AscentChain[indexOfCurrent + 1];
                ResourceSummary costsForTransition;
                CostsToReach.TryGetValue(nextComplexity, out costsForTransition);

                return new AscentSummary(currentComplexity, nextComplexity,
                    costsForTransition ?? ResourceSummary.Empty);
            }
        }

        public IComplexityDefinition GetDescentTransition(IComplexityDefinition currentComplexity) {
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

        #endregion

        #endregion
        
    }

}
