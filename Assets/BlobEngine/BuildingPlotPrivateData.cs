using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    public class BuildingPlotPrivateData : MonoBehaviour {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get { return _topLevelUIFSM; }
        }
        [SerializeField] private UIFSM _topLevelUIFSM;

        public BlobTubeFactoryBase TubeFactory {
            get { return _tubeFactory; }
        }
        [SerializeField] private BlobTubeFactoryBase _tubeFactory;

        public ReadOnlyCollection<Schematic> AvailableSchematics {
            get {
                if(_availableSchematics == null) {
                    _availableSchematics = new List<Schematic>();
                    foreach(var name in NamesOfAvailableSchematics) {
                        _availableSchematics.Add(SchematicRepository.GetSchematicOfName(name));
                    }
                }
                return _availableSchematics.AsReadOnly();
            }
        }
        private List<Schematic> _availableSchematics = null;

        [SerializeField] private List<string> NamesOfAvailableSchematics;
        [SerializeField] private SchematicRepository SchematicRepository;

        #endregion

    }

}
