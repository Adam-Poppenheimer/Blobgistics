using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public class SocietyUISummary {

        #region instance fields and properties

        public int ID {
            get { return SocietyToSummarize.ID; }
        }

        public MapNodeUISummary Location {
            get { return new MapNodeUISummary(SocietyToSummarize.Location); }
        }

        public ComplexityDefinitionBase CurrentComplexity {
            get { return SocietyToSummarize.CurrentComplexity; }
        }

        public ReadOnlyCollection<ComplexityDefinitionBase> AscentComplexities {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetAscentTransitions(SocietyToSummarize.CurrentComplexity);
            }
        }

        public ReadOnlyCollection<ComplexityDefinitionBase> DescentComplexities {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetDescentTransitions(SocietyToSummarize.CurrentComplexity);
            }
        }

        public bool NeedsAreSatisfied {
            get { return SocietyToSummarize.NeedsAreSatisfied; }
        }

        public float SecondsOfUnsatisfiedNeeds {
            get { return SocietyToSummarize.SecondsOfUnsatisfiedNeeds; }
        }

        public float SecondsUntilComplexityDescent {
            get { return SocietyToSummarize.SecondsUntilComplexityDescent; }
        }

        public Transform Transform {
            get { return SocietyToSummarize.transform; }
        }

        public bool AscensionIsPermitted {
            get { return SocietyToSummarize.AscensionIsPermitted; }
        }

        private readonly SocietyBase SocietyToSummarize;

        #endregion

        #region constructors

        public SocietyUISummary(SocietyBase societyToSummarize) {
            SocietyToSummarize = societyToSummarize;
        }

        #endregion

        #region instance methods

        public bool GetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity) {
            return SocietyToSummarize.GetAscensionPermissionForComplexity(complexity);
        }

        #endregion

    }

}
