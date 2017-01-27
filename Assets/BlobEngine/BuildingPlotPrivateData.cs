using System;
using System.Collections.Generic;
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

        public BuildingSchematicRepository SchematicRepository {
            get { return _schematicRepository; }
        }
        [SerializeField] private BuildingSchematicRepository _schematicRepository;

        #endregion

    }

}
