using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.Session.Editor {

    [CustomEditor(typeof(FileSystemLiaison))]
    public class FileSystemLiaisonEditor : UnityEditor.Editor {

        #region instance fields and properties

        private FileSystemLiaison TargetedLiaison {
            get { return target as FileSystemLiaison; }
        }

        #endregion

        #region instance methods

        #region from Editor

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(
                string.IsNullOrEmpty(TargetedLiaison.SavedGameStoragePath) || 
                string.IsNullOrEmpty(TargetedLiaison.SessionExtension)
            );

            if(GUILayout.Button("Refresh Loaded Saved Games")) {
                TargetedLiaison.RefreshLoadedSavedGames();
            }

            if(GUILayout.Button("Refresh Loaded Maps")) {
                TargetedLiaison.RefreshLoadedMaps();
            }

            EditorGUI.EndDisabledGroup();
        }

        #endregion

        #endregion

    }

}
