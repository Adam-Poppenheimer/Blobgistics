using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Societies {

    public class ComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        [SerializeField] private List<ComplexityDefinitionBase> ComplexityHierarchy;

        #endregion

        #region instance methods

        public override ComplexityDefinitionBase GetStartingComplexity() {
            if(ComplexityHierarchy.Count > 0) {
                return ComplexityHierarchy[0];
            }else {
                return null;
            }
        }

        public override ComplexityDefinitionBase GetAscentTransition(ComplexityDefinitionBase currentComplexity) {
            int indexOfCurrent = ComplexityHierarchy.IndexOf(currentComplexity);
            if(indexOfCurrent < 0 || indexOfCurrent == ComplexityHierarchy.Count - 1) {
                return null;
            }else {
                return ComplexityHierarchy[indexOfCurrent + 1];
            }
        }

        public override ComplexityDefinitionBase GetDescentTransition(ComplexityDefinitionBase currentComplexity) {
            int indexOfCurrent = ComplexityHierarchy.IndexOf(currentComplexity);
            if(indexOfCurrent <= 0) {
                return null;
            }else {
                return ComplexityHierarchy[indexOfCurrent - 1];
            }
        }

        #endregion
        
    }

}
