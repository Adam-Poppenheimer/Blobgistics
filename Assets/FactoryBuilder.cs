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
using Assets.Mobs;

namespace Assets {

    [ExecuteInEditMode]
    public class FactoryBuilder : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private UIFSM TopLevelUIFSM;

        [SerializeField] private GameObject BuildingPlotPrefab;
        [SerializeField] private GameObject ResourcePoolPrefab;
        [SerializeField] private GameObject BlobTubePrefab;

        [SerializeField] private Transform CanvasRoot;
        [SerializeField] private MapGraph  MapGraph;

        [SerializeField] private BuildingPlotFactory    BuildingPlotFactory    = null;
        [SerializeField] private ResourcePoolFactory    ResourcePoolFactory    = null;
        [SerializeField] private BlobTubeFactory        BlobTubeFactory        = null;
        [SerializeField] private BlobGeneratorFactory   GeneratorFactory       = null;
        [SerializeField] private BlobletFactory         BlobletFactory         = null;
        [SerializeField] private BlobletBarracksFactory BlobletBarracksFactory = null;

        [SerializeField] private SchematicRepository BuildingSchematicRepository;

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
            ObjectGraphDependencyInjector.InjectDependency<BuildingPlotFactoryBase    >(BuildingPlotFactory,
                "BuildingPlotFactory"       , CanvasRoot, MapGraph.transform);
            ObjectGraphDependencyInjector.InjectDependency<ResourcePoolFactoryBase    >(ResourcePoolFactory,
                "ResourcePoolFactory"       , CanvasRoot, MapGraph.transform);
            ObjectGraphDependencyInjector.InjectDependency<BlobTubeFactoryBase        >(BlobTubeFactory,
                "BlobTubeFactory"           , CanvasRoot, MapGraph.transform);
            ObjectGraphDependencyInjector.InjectDependency<BlobGeneratorFactoryBase   >(GeneratorFactory,
                "GeneratorFactory"          , CanvasRoot, MapGraph.transform);
            ObjectGraphDependencyInjector.InjectDependency<SchematicRepository>(BuildingSchematicRepository,
                "BuildingSchematiRepository", CanvasRoot, MapGraph.transform);
            

            EditorPrefabBuilder.PlotFactory      = BuildingPlotFactory;
            EditorPrefabBuilder.PoolFactory      = ResourcePoolFactory;
            EditorPrefabBuilder.TubeFactory      = BlobTubeFactory;
            EditorPrefabBuilder.GeneratorFactory = GeneratorFactory;
            EditorPrefabBuilder.MapGraph         = MapGraph;
        }

        #endregion

        #endregion

    }

}
