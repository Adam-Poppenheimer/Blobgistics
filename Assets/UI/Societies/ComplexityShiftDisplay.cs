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

    public class ComplexityShiftDisplay : MonoBehaviour {

        #region instance fields and properties

        public ComplexityDefinitionBase ComplexityToDisplay { get; set; }
        public bool IsCandidateForShift { get; set; }

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

        public void RefreshDisplay() {
            if(NameField != null) {
                NameField.color = IsCandidateForShift ? IsCandidateForShiftColor : IsNotCandidateForShiftColor;
                NameField.text = ComplexityToDisplay.name;
            }
            if(PermittedTerrainsField != null) { 
                PermittedTerrainsField.color = IsCandidateForShift ? IsCandidateForShiftColor : IsNotCandidateForShiftColor;

                if(ComplexityToDisplay.PermittedTerrains.Count == 1) {
                    PermittedTerrainsField.text = ComplexityToDisplay.PermittedTerrains[0].ToString();
                }else if(ComplexityToDisplay.PermittedTerrains.Count == 2) {
                    PermittedTerrainsField.text = string.Format("{0} or {1}", ComplexityToDisplay.PermittedTerrains[0],
                        ComplexityToDisplay.PermittedTerrains[1]);
                }else {
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

            if(CostField != null) {
                CostField.PushAndDisplaySummary(ComplexityToDisplay.CostToAscendInto);
            }
        }

        #endregion

    }

}
