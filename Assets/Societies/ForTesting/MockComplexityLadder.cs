using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using System.Collections.ObjectModel;

namespace Assets.Societies.ForTesting {

    public class MockComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        #region from ComplexityLadderBase

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

        public List<ComplexityDefinitionBase> AscentChain;
        public int StartingIndex;

        #endregion

        #region instance methods

        #region from IComplexityLadder

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity) {
            int indexOfCurrent = AscentChain.IndexOf(currentComplexity);
            if(indexOfCurrent < 0 || indexOfCurrent == AscentChain.Count - 1) {
                return new List<ComplexityDefinitionBase>().AsReadOnly();
            }else {
                return new List<ComplexityDefinitionBase>() { AscentChain[indexOfCurrent + 1] }.AsReadOnly();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity) {
            int indexOfCurrent = AscentChain.IndexOf(currentComplexity);
            if(indexOfCurrent < 0 || indexOfCurrent == 0) {
                return new List<ComplexityDefinitionBase>().AsReadOnly();
            }else {
                return new List<ComplexityDefinitionBase>() { AscentChain[indexOfCurrent - 1] }.AsReadOnly();
            }
        }

        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            return AscentChain.Contains(complexity);
        }

        public override int GetTierOfComplexity(ComplexityDefinitionBase complexity) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
