using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Assets.Highways;
using Assets.Map;
using Assets.Societies;
using Assets.ResourceDepots;

namespace Assets.Util {

    /// <summary>
    /// A class that facilitates the creation of various gameplay elements during design time.
    /// It provides menu and context options for creating societies, resource depots, and map
    /// graphs.
    /// </summary>
    /// <remarks>
    /// The structure of this class results from how menu items are implemented. A menu item
    /// requires a pair of static methods in order to function. These commands require information
    /// from the scene in order to function properly, which means that this must transfer factories
    /// and complexities from some component into static variables before they can be used to
    /// create gameplay elements. That's why there are instance duplicates of all the static
    /// properties in this class.
    /// 
    /// There should never be more than one EditorPrefabBuilder in a given scene at a time.
    /// </remarks>
    [ExecuteInEditMode]
    public class EditorPrefabBuilder : MonoBehaviour {

        #if UNITY_EDITOR

        #region static fields and properties

        private static MapGraphBase             StaticMapGraph             { get; set; }
        private static SocietyFactoryBase       StaticSocietyFactory       { get; set; }
        private static ResourceDepotFactoryBase StaticResourceDepotFactory { get; set; }

        private static ComplexityDefinitionBase StaticFarmlandComplexity         { get; set; }
        private static ComplexityDefinitionBase StaticLumberCampComplexity       { get; set; }
        private static ComplexityDefinitionBase StaticCottonPlantationComplexity { get; set; }
        private static ComplexityDefinitionBase StaticMineComplexity             { get; set; }

        private static ComplexityDefinitionBase StaticSawmillVillageComplexity     { get; set; }
        private static ComplexityDefinitionBase StaticSteelMillVillageComplexity   { get; set; }
        private static ComplexityDefinitionBase StaticTextileMillVillageComplexity { get; set; }

        private static ComplexityDefinitionBase StaticSuburbanTownComplexity { get; set; }
        private static ComplexityDefinitionBase StaticFactoryTownComplexity  { get; set; }

        private static ComplexityDefinitionBase StaticCityComplexity { get; set; }

        #endregion

        #region instance fields and properties

        [SerializeField] private MapGraphBase             MapGraph;
        [SerializeField] private SocietyFactoryBase       SocietyFactory;
        [SerializeField] private ResourceDepotFactoryBase ResourceDepotFactory;

        [SerializeField] private ComplexityDefinitionBase FarmlandComplexity;
        [SerializeField] private ComplexityDefinitionBase LumberCampComplexity;
        [SerializeField] private ComplexityDefinitionBase CottonPlantationComplexity;
        [SerializeField] private ComplexityDefinitionBase MineComplexity;

        [SerializeField] private ComplexityDefinitionBase SawmillVillageComplexity;
        [SerializeField] private ComplexityDefinitionBase SteelMillVillageComplexity;
        [SerializeField] private ComplexityDefinitionBase TextileMillVillageComplexity;

        [SerializeField] private ComplexityDefinitionBase SuburbanTownComplexity;
        [SerializeField] private ComplexityDefinitionBase FactoryTown;

        [SerializeField] private ComplexityDefinitionBase CityComplexity;

        #endregion

        #region static methods

        #region Tier 1 Society construction

        [MenuItem("CONTEXT/MapNodeBase/Construct Farmland", false, 10)]
        private static void ConstructFarmlandAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticFarmlandComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Farmland", true, 10)]
        private static bool ValidateConstructFarmlandAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticFarmlandComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Cotton Plantation", false, 10)]
        private static void ConstructCottonPlantationAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticCottonPlantationComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Cotton Plantation", true, 10)]
        private static bool ValidateConstructCottonPlantationAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticCottonPlantationComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Lumber Camp", false, 10)]
        private static void ConstructLumberCampAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticLumberCampComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Lumber Camp", true, 10)]
        private static bool ValidateConstructLumberCampAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticLumberCampComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Mine", false, 10)]
        private static void ConstructMineAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticMineComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Mine", true, 10)]
        private static bool ValidateConstructMineAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticMineComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        #endregion

        #region Tier 2 Society construction

        [MenuItem("CONTEXT/MapNodeBase/Construct Steel Mill Village", false, 11)]
        private static void ConstructSteelMillVillageAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticSteelMillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Steel Mill Village", true, 11)]
        private static bool ValidateConstructSteelMillVillageAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticSteelMillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Sawmill Village", false, 11)]
        private static void ConstructSawmillVillageAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticSawmillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Sawmill Village", true, 11)]
        private static bool ValidateConstructSawmillVillageAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticSawmillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Textile Mill Village", false, 11)]
        private static void ConstructTextileMillVillageAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticTextileMillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Textile Mill Village", true, 11)]
        private static bool ValidateConstructTextileMillVillageAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticTextileMillVillageComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        #endregion

        #region Tier 3 Society construction

        [MenuItem("CONTEXT/MapNodeBase/Construct Factory Town", false, 12)]
        private static void ConstructFactoryTownAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticFactoryTownComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct Factory Town", true, 12)]
        private static bool ValidateConstructFactoryTownAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticFactoryTownComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct SuburbanTown", false, 12)]
        private static void ConstrucSuburbanTownAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticSuburbanTownComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct SuburbanTown", true, 12)]
        private static bool ValidateConstructSuburbanTownAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticSuburbanTownComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        #endregion

        #region Tier 4 Society construction

        [MenuItem("CONTEXT/MapNodeBase/Construct City", false, 13)]
        private static void ConstructCityAtLocation(MenuCommand command) {
            ConstructSocietyOfComplexityAndLadder(StaticCityComplexity, StaticSocietyFactory.StandardComplexityLadder, command);
        }

        [MenuItem("CONTEXT/MapNodeBase/Construct City", true, 13)]
        private static bool ValidateConstructCityAtLocation() {
            return ValidateSocietyOfComplexityAndLadder(StaticCityComplexity, StaticSocietyFactory.StandardComplexityLadder);
        }

        #endregion

        private static void ConstructSocietyOfComplexityAndLadder(ComplexityDefinitionBase complexity,
            ComplexityLadderBase ladder, MenuCommand command) {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            var newSociety = StaticSocietyFactory.ConstructSocietyAt(locationToConstruct, ladder, complexity);
            HandleContext(newSociety.gameObject, command);
        }

        private static bool ValidateSocietyOfComplexityAndLadder(ComplexityDefinitionBase complexity,
            ComplexityLadderBase ladder) {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return (
                    locationToBuild != null &&
                    StaticSocietyFactory.CanConstructSocietyAt(locationToBuild, ladder, complexity)
                );
            }else {
                return false;
            }
        }

        [MenuItem("Strategy Blobs/Construct Resource Depot At Location")]
        private static void ConstructResourceDepotAtLocation(MenuCommand command) {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            var newDepot = StaticResourceDepotFactory.ConstructDepotAt(locationToConstruct);
            HandleContext(newDepot.gameObject, command);
        }

        [MenuItem("Strategy Blobs/Construct Resource Depot At Location", true)]
        private static bool ValidateConstructResourceDepotAtLocation() {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return locationToBuild != null && !StaticResourceDepotFactory.HasDepotAtLocation(locationToBuild);
            }else {
                return false;
            }
        }

        [MenuItem("GameObject/Strategy Blobs/Map Node", false, 10)]
        private static void CreateMapNode(MenuCommand command) {
            Vector3 positionOfNode;
            var commandGameObject = command.context as GameObject;
            if(commandGameObject != null) {
                positionOfNode = commandGameObject.transform.position;
            }else {
                positionOfNode = Vector3.zero;
            }
            var newNode = StaticMapGraph.BuildNode(positionOfNode);
            HandleContext(newNode.gameObject, command);
        }

        private static void HandleContext(GameObject objectToManage, MenuCommand issuingCommand) {
            Undo.RegisterCreatedObjectUndo(objectToManage, "Create " + objectToManage.name);
            Selection.activeObject = objectToManage;
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnValidate() {
            StaticMapGraph             = MapGraph;
            StaticSocietyFactory       = SocietyFactory;
            StaticResourceDepotFactory = ResourceDepotFactory;

            StaticFarmlandComplexity         = FarmlandComplexity;
            StaticLumberCampComplexity       = LumberCampComplexity;
            StaticCottonPlantationComplexity = CottonPlantationComplexity;
            StaticMineComplexity       = MineComplexity;

            StaticSawmillVillageComplexity  = SawmillVillageComplexity;
            StaticTextileMillVillageComplexity = TextileMillVillageComplexity;
            StaticSteelMillVillageComplexity   = SteelMillVillageComplexity;

            StaticFactoryTownComplexity = FactoryTown;
            StaticSuburbanTownComplexity   = SuburbanTownComplexity;

            StaticCityComplexity = CityComplexity;
        }

        #endregion

        #endregion

        #endif
    }

}
