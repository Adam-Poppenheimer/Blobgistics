using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Societies;
using Assets.Map;
using Assets.UI.Blobs;

namespace Assets.UI.Societies {

    /// <summary>
    /// A class that displays information and commands about a particular
    /// complexity definition, assuming it to be either an ascent or descent
    /// transition. Also provides the player an interface for setting ascension
    /// permissions.
    /// </summary>
    public class ComplexityShiftDisplay : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The complexity definition to display.
        /// </summary>
        public ComplexityDefinitionBase ComplexityToDisplay { get; set; }

        /// <summary>
        /// Whether or not the complexity is a candidate for either
        /// ascent or descent, depending on the context.
        /// </summary>
        public bool IsCandidateForShift { get; set; }

        /// <summary>
        /// The toggle that players can use to control ascension permission
        /// for the complexity.
        /// </summary>
        public Toggle AscensionPermissionToggle {
            get { return _ascensionPermissionToggle; }
        }
        [SerializeField] private Toggle _ascensionPermissionToggle;

        [SerializeField] private Text NameField;
        [SerializeField] private Text PermittedTerrainsField;
        [SerializeField] private ResourceDisplay CostField;

        [SerializeField] private Color IsCandidateForShiftColor;
        [SerializeField] private Color IsNotCandidateForShiftColor;

        #endregion

        #region instance methods

        /// <summary>
        /// Refreshes the display, plugging information from ComplexityToDisplay
        /// into appropriate fields.
        /// </summary>
        public void RefreshDisplay() {
            //The complexity's name
            if(NameField != null) {
                NameField.color = IsCandidateForShift ? IsCandidateForShiftColor : IsNotCandidateForShiftColor;
                NameField.text = ComplexityToDisplay.name;
            }

            //Its permitted terrains
            if(PermittedTerrainsField != null) { 
                PermittedTerrainsField.color = IsCandidateForShift ? IsCandidateForShiftColor : IsNotCandidateForShiftColor;

                
                if(ComplexityToDisplay.PermittedTerrains.Count == 1) {
                    //If there is one terrain
                    PermittedTerrainsField.text = ComplexityToDisplay.PermittedTerrains[0].ToString();
                }else if(ComplexityToDisplay.PermittedTerrains.Count == 2) {
                    //If there are two
                    PermittedTerrainsField.text = string.Format("{0} or {1}", ComplexityToDisplay.PermittedTerrains[0],
                        ComplexityToDisplay.PermittedTerrains[1]);
                }else {
                    //If there are more than two
                    foreach(var terrain in ComplexityToDisplay.PermittedTerrains) {
                        if(terrain == ComplexityToDisplay.PermittedTerrains.First()) {
                            PermittedTerrainsField.text = terrain.ToString();
                        }else if(terrain == ComplexityToDisplay.PermittedTerrains.Last()) {
                            PermittedTerrainsField.text += ", or " + terrain.ToString();
                        }else {
                            PermittedTerrainsField.text += ", " + terrain.ToString();
                        }
                    }
                }
            }

            //The complexity's CostToAscendInto
            if(CostField != null) {
                CostField.PushAndDisplaySummary(ComplexityToDisplay.CostToAscendInto);
            }
        }

        #endregion

    }

}
