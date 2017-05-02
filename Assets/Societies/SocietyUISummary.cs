using System;
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

        public ComplexityDefinitionBase AscentComplexity {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetAscentTransition(SocietyToSummarize.CurrentComplexity);
            }
        }

        public ComplexityDefinitionBase DescentComplexity {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetDescentTransition(SocietyToSummarize.CurrentComplexity);
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

    }

}
