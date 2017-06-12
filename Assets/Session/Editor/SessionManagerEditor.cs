using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.Session.Editor {

    [CustomEditor(typeof(SessionManager))]
    public class SessionManagerEditor : UnityEditor.Editor {

        #region instance fields and properties

        private SessionManager TargetedManager {
            get { return target as SessionManager; }
        }

        private string NewMapName = "";
        private string NewMapDescription = "";
        private int    NewMapPointsToWin = 0;

        private bool ShowExistingMaps;

        #endregion

        #region instance methods

        #region from Editor

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            ShowExistingMaps = EditorGUILayout.Foldout(ShowExistingMaps, "Existing Maps");
            if(ShowExistingMaps) {
                EditorGUILayout.BeginVertical();

                foreach(var session in TargetedManager.FileSystemLiaison.LoadedMaps) {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(session.Name);
                    if(GUILayout.Button("Load map")) {
                        TargetedManager.PushSessionIntoRuntime(session);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Save Current Map", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Map Name");
            NewMapName = EditorGUILayout.TextField(NewMapName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Description");
            NewMapDescription = EditorGUILayout.TextArea(NewMapDescription, EditorStyles.textArea);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Points to Win");
            NewMapPointsToWin = EditorGUILayout.IntField(NewMapPointsToWin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if(GUILayout.Button("Save current configuration as map")) {
                var sessionPulled = TargetedManager.PullSessionFromRuntime(NewMapName, NewMapDescription, NewMapPointsToWin);
                TargetedManager.FileSystemLiaison.WriteMapToFile(sessionPulled);
                AssetDatabase.Refresh();
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        #endregion

    }

}
