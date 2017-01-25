using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Misc;

using Assets.BlobEngine;
using Assets.Editor;

namespace Assets {

    [ExecuteInEditMode]
    public class FactoryBuilder : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private UIFSM TopLevelUIFSM;

        [SerializeField] private GameObject BuildingPlotPrefab;
        [SerializeField] private GameObject ResourcePoolPrefab;
        [SerializeField] private GameObject BlobTubePrefab;

        [SerializeField] private Transform CanvasRoot;
        [SerializeField] private Transform MapRoot;

        private BuildingPlotFactory BuildingPlotFactory;
        private ResourcePoolFactory ResourcePoolFactory;
        private BlobTubeFactory     BlobTubeFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            BuildingPlotFactory = new BuildingPlotFactory();
            ResourcePoolFactory = new ResourcePoolFactory();
            BlobTubeFactory     = new BlobTubeFactory();;

            BuildingPlotFactory.TopLevelUIFSM = TopLevelUIFSM;
            BuildingPlotFactory.TubeFactory   = BlobTubeFactory;
            BuildingPlotFactory.PlotPrefab    = BuildingPlotPrefab;

            ResourcePoolFactory.TopLevelUIFSM = TopLevelUIFSM;
            ResourcePoolFactory.PoolPrefab    = ResourcePoolPrefab;

            BlobTubeFactory.MapRoot    = MapRoot;
            BlobTubeFactory.TubePrefab = BlobTubePrefab;

            ObjectGraphDependencyInjector.InjectDependency<IBuildingPlotFactory>(BuildingPlotFactory,
                "BuildingPlotFactory", CanvasRoot, MapRoot);
            ObjectGraphDependencyInjector.InjectDependency<IResourcePoolFactory>(ResourcePoolFactory,
                "ResourcePoolFactory", CanvasRoot, MapRoot);
            ObjectGraphDependencyInjector.InjectDependency<IBlobTubeFactory    >(BlobTubeFactory,
                "BlobTubeFactory"    , CanvasRoot, MapRoot);

            EditorPrefabBuilder.PlotFactory = BuildingPlotFactory;
            EditorPrefabBuilder.PoolFactory = ResourcePoolFactory;
            EditorPrefabBuilder.TubeFactory = BlobTubeFactory;
            EditorPrefabBuilder.MapRoot     = MapRoot;
        }

        #endregion

        #endregion

    }

}
