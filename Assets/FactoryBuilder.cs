using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Misc;

using Assets.BlobEngine;
using Assets.Editing;
using Assets.Map;
namespace Assets {

    [ExecuteInEditMode]
    public class FactoryBuilder : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private UIFSM TopLevelUIFSM;

        [SerializeField] private GameObject BlobTubePrefab;

        [SerializeField] private Transform CanvasRoot;
        [SerializeField] private MapGraph  MapGraph;

        [SerializeField] private BlobTubeFactory        BlobTubeFactory        = null;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
            PushData();
        }

        private void Awake() {
            PushData();
        }

        private void PushData() {
            ObjectGraphDependencyInjector.InjectDependency<BlobTubeFactoryBase        >(BlobTubeFactory,
                "BlobTubeFactory"           , CanvasRoot, MapGraph.transform);

            EditorPrefabBuilder.TubeFactory      = BlobTubeFactory;
            EditorPrefabBuilder.MapGraph         = MapGraph;
        }

        #endregion

        #endregion

    }

}
