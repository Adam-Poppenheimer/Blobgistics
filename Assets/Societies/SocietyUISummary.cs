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

        public string CurrentComplexity {
            get { return SocietyToSummarize.CurrentComplexity.Name; }
        }

        public string AscentComplexity {
            get {
                var actualAscent = SocietyToSummarize.ActiveComplexityLadder.GetAscentTransition(SocietyToSummarize.CurrentComplexity);
                return actualAscent  != null ? actualAscent.Name  : "--";
            }
        }

        public string DescentComplexity {
            get {
                var actualDescent = SocietyToSummarize.ActiveComplexityLadder.GetDescentTransition(SocietyToSummarize.CurrentComplexity);
                return actualDescent != null ? actualDescent.Name : "--";
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

        private readonly SocietyBase SocietyToSummarize;

        #endregion

        #region constructors

        public SocietyUISummary(SocietyBase societyToSummarize) {
            SocietyToSummarize = societyToSummarize;
        }

        #endregion

    }

}
