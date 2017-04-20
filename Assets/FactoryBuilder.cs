using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Misc;

using Assets.Societies;
using Assets.Editing;
using Assets.Map;
using Assets.ResourceDepots;
using Assets.Generator;

namespace Assets {

    [ExecuteInEditMode]
    public class FactoryBuilder : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private MapGraphBase MapGraph;
        [SerializeField] private SocietyFactoryBase SocietyFactory;
        [SerializeField] private ResourceDepotFactoryBase ResourceDepotFactory;
        [SerializeField] private ResourceGeneratorFactory GeneratorFactory;

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
            EditorPrefabBuilder.MapGraph = MapGraph;
            EditorPrefabBuilder.SocietyFactory = SocietyFactory;
            EditorPrefabBuilder.ResourceDepotFactory = ResourceDepotFactory;
            EditorPrefabBuilder.GeneratorFactory = GeneratorFactory;
        }

        #endregion

        #endregion

    }

}
