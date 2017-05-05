using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Societies {

    public class ComplexityLadder : ComplexityLadderBase {

        #region instance fields and properties

        public List<ComplexityDefinitionBase> TierOneComplexities {
            get { return _tierOneComplexities; }
            set { _tierOneComplexities = value; }
        }
        [SerializeField] private List<ComplexityDefinitionBase> _tierOneComplexities;

        public List<ComplexityDefinitionBase> TierTwoComplexities {
            get { return _tierTwoComplexities; }
            set { _tierTwoComplexities = value; }
        }
        [SerializeField] private List<ComplexityDefinitionBase> _tierTwoComplexities;

        public List<ComplexityDefinitionBase> TierThreeComplexities {
            get { return _tierThreeComplexities; }
            set { _tierThreeComplexities = value; }
        }
        [SerializeField] private List<ComplexityDefinitionBase> _tierThreeComplexities;

        public List<ComplexityDefinitionBase> TierFourComplexities {
            get { return _tierFourComplexities; }
            set { _tierFourComplexities = value; }
        }
        [SerializeField] private List<ComplexityDefinitionBase> _tierFourComplexities;

        private List<ComplexityDefinitionBase> EmptyComplexityList = new List<ComplexityDefinitionBase>();

        #endregion

        #region instance methods

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetAscentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(TierOneComplexities.Contains(currentComplexity)){
                return TierTwoComplexities.AsReadOnly();
            }else if(TierTwoComplexities.Contains(currentComplexity)) {
                return TierThreeComplexities.AsReadOnly();
            }else if(TierThreeComplexities.Contains(currentComplexity)) {
                return TierFourComplexities.AsReadOnly();
            }else {
                return EmptyComplexityList.AsReadOnly();
            }
        }

        public override ReadOnlyCollection<ComplexityDefinitionBase> GetDescentTransitions(ComplexityDefinitionBase currentComplexity) {
            if(TierFourComplexities.Contains(currentComplexity)) {
                return TierThreeComplexities.AsReadOnly();
            }else if(TierThreeComplexities.Contains(currentComplexity)) {
                return TierTwoComplexities.AsReadOnly();
            }else if(TierTwoComplexities.Contains(currentComplexity)) {
                return TierOneComplexities.AsReadOnly();
            }else {
                return EmptyComplexityList.AsReadOnly();
            }
        }

        public override bool ContainsComplexity(ComplexityDefinitionBase complexity) {
            return (
                TierOneComplexities.Contains  (complexity) || 
                TierTwoComplexities.Contains  (complexity) ||
                TierThreeComplexities.Contains(complexity) || 
                TierFourComplexities.Contains (complexity)
            );
        }

        #endregion

    }

}
