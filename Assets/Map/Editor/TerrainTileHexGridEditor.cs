using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.Map.Editor {

    [CustomEditor(typeof(TerrainGrid))]
    public class TerrainTileHexGridEditor : UnityEditor.Editor {

        #region instance fields and properties

        SerializedObject HexGridSerializedObject;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnEnable() {
            HexGridSerializedObject = new SerializedObject(target);
        }

        #endregion

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            HexGridSerializedObject.Update();

            if(GUILayout.Button("Rebuild Map")) {
                var hexGrid = target as TerrainGrid;
                hexGrid.ClearMap();
                hexGrid.CreateMap();
            }

            if(GUILayout.Button("Refresh Map Terrains")) {
                var hexGrid = target as TerrainGrid;
                hexGrid.RefreshMapTerrains();
            }

            HexGridSerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        #endregion

    }

}
