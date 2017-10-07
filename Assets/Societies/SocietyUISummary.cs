using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    /// <summary>
    /// A class containing information that Society should pass into UIControl whenever
    /// it catches user unput.
    /// </summary>
    public class SocietyUISummary {

        #region instance fields and properties

        /// <summary>
        /// Equivalent to <see cref="Society.ID"/>.
        /// </summary>
        public int ID {
            get { return SocietyToSummarize.ID; }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.Location"/>.
        /// </summary>
        public MapNodeUISummary Location {
            get { return new MapNodeUISummary(SocietyToSummarize.Location); }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.CurrentComplexity"/>.
        /// </summary>
        public ComplexityDefinitionBase CurrentComplexity {
            get { return SocietyToSummarize.CurrentComplexity; }
        }

        /// <summary>
        /// All of the ascent transitions available to the society from its current complexity.
        /// </summary>
        public ReadOnlyCollection<ComplexityDefinitionBase> AscentComplexities {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetAscentTransitions(SocietyToSummarize.CurrentComplexity);
            }
        }

        /// <summary>
        /// All of the descent transitions available to the society from its current complexity.
        /// </summary>
        public ReadOnlyCollection<ComplexityDefinitionBase> DescentComplexities {
            get {
                return SocietyToSummarize.ActiveComplexityLadder.GetDescentTransitions(SocietyToSummarize.CurrentComplexity);
            }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.NeedsAreSatisfied"/>.
        /// </summary>
        public bool NeedsAreSatisfied {
            get { return SocietyToSummarize.NeedsAreSatisfied; }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.SecondsOfUnsatisfiedNeeds"/>.
        /// </summary>
        public float SecondsOfUnsatisfiedNeeds {
            get { return SocietyToSummarize.SecondsOfUnsatisfiedNeeds; }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.SecondsUntilComplexityDescent"/>.
        /// </summary>
        public float SecondsUntilComplexityDescent {
            get { return SocietyToSummarize.SecondsUntilComplexityDescent; }
        }

        /// <summary>
        /// The transform attached to the society.
        /// </summary>
        public Transform Transform {
            get { return SocietyToSummarize.transform; }
        }

        /// <summary>
        /// Equivalent to <see cref="Society.AscensionIsPermitted"/>.
        /// </summary>
        public bool AscensionIsPermitted {
            get { return SocietyToSummarize.AscensionIsPermitted; }
        }

        private readonly SocietyBase SocietyToSummarize;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a UI summary for the given society.
        /// </summary>
        /// <param name="societyToSummarize">The society to summarize</param>
        public SocietyUISummary(SocietyBase societyToSummarize) {
            SocietyToSummarize = societyToSummarize;
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the ascension permissions for the given society in the society that's being summarized.
        /// </summary>
        /// <param name="complexity">The complexity to consider</param>
        /// <returns>Whether it's permitted to ascend in the summarized society</returns>
        public bool GetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity) {
            return SocietyToSummarize.GetAscensionPermissionForComplexity(complexity);
        }

        #endregion

    }

}
