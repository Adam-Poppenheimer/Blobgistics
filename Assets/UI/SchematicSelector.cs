using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Assets.BlobEngine;

using UnityCustomUtilities.Misc;
using UnityCustomUtilities.UI;

namespace Assets.UI {

    public class SchematicSelector : MonoBehaviour, IInjectionTarget<SchematicRepository>{

        #region instance fields and properties

        private Dictionary<string, Button> ButtonOfLabelName {
            get {
                if(_buttonOfLabelName == null) {
                    _buttonOfLabelName = LoadButtons();
                }
                return _buttonOfLabelName;
            }
        }
        private Dictionary<string, Button> _buttonOfLabelName;

        private SchematicRepository SchematicRepository;

        private IBuildingPlot SelectedPlot;

        #endregion

        #region instance methods

        #region from IInjectionTarget

        public void InjectDependency(SchematicRepository dependency, string tag) {
            SchematicRepository = dependency;
        }

        #endregion

        public void Activate(IBuildingPlot newSelectedPlot) {
            SelectedPlot = newSelectedPlot;
            foreach(var schematic in SelectedPlot.AvailableSchematics) {
                Button buttonForSchematic;
                ButtonOfLabelName.TryGetValue(schematic.Name, out buttonForSchematic);
                if(buttonForSchematic != null) {
                    buttonForSchematic.onClick.AddListener(BuildSchematicListener(schematic));
                    buttonForSchematic.gameObject.SetActive(true);
                }
            }
            gameObject.SetActive(true);
        }

        public void Deactivate() {
            SelectedPlot = null;
            foreach(var button in ButtonOfLabelName.Values) {
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        private Dictionary<string, Button> LoadButtons() {
            var retval = new Dictionary<string, Button>();
            foreach(var button in GetComponentsInChildren<Button>(true)) {
                var label = button.GetComponent<Label>();
                if(label != null && SchematicRepository.HasSchematicOfName(label.Text)) {
                    retval[label.Text] = button;
                }
            }
            return retval;
        }

        private UnityAction BuildSchematicListener(Schematic schematic) {
            return delegate() {
                SelectedPlot.ActiveSchematic = schematic;
                Deactivate();
            };
        }

        #endregion

    }

}
