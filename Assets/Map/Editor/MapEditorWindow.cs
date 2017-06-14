using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.Highways;
using Assets.Session;
using Assets.Util;

namespace Assets.Map.Editor {

    public class MapEditorWindow : EditorWindow {

        #region internal types

        private enum SceneViewInteractionMode {
            Viewing,
            MapEdges,
            Highways
        }

        #endregion

        #region instance fields and properties 

        private SceneViewInteractionMode CurrentInteractionMode = SceneViewInteractionMode.Highways;

        #endregion

        #region static methods

        [MenuItem("Strategy Blobs/Open Map Editor")]
        public static void ShowWindow() {
            GetWindow<MapEditorWindow>("Map Editor");
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnEnable() {
            SceneView.onSceneGUIDelegate -= DoOnSceneGUI;
            SceneView.onSceneGUIDelegate += DoOnSceneGUI;
        }

        private void OnDisable() {
            SceneView.onSceneGUIDelegate -= DoOnSceneGUI;
        }

        private void OnFocus() {
            if(EditorWindowDependencyPusher.SessionManager.CurrentSession == null) {
                EditorWindowDependencyPusher.SessionManager.CurrentSession = new SerializableSession(
                    "New Map", "Place a description here", 42);
            }
            Refresh();
        }

        private void OnDestroy() {
            SceneView.onSceneGUIDelegate -= DoOnSceneGUI;
        }

        private void OnGUI() {
            GUILayout.Label("Map Editor", EditorStyles.largeLabel);
            CurrentInteractionMode = (SceneViewInteractionMode)EditorGUILayout.EnumPopup("Interaction Mode", CurrentInteractionMode);

            EditorGUILayout.Space();

            OnGUI_MapSerialization();
        }

        #endregion

        #region from Object

        public override string ToString() {
            return "Map Editor";
        }

        #endregion

        private void OnGUI_MapSerialization() {
            EditorGUILayout.BeginVertical();

            var currentSession = EditorWindowDependencyPusher.SessionManager.CurrentSession;

            EditorGUI.BeginDisabledGroup(currentSession == null || string.IsNullOrEmpty(currentSession.Name));

            currentSession.Name        = EditorGUILayout.DelayedTextField("Name", currentSession.Name);
            currentSession.Description = EditorGUILayout.TextArea(currentSession.Description, EditorStyles.textArea);
            currentSession.ScoreToWin  = EditorGUILayout.DelayedIntField("Score to Win", currentSession.ScoreToWin);

            if(GUILayout.Button("Save current map to file")) {
                EditorWindowDependencyPusher.SessionManager.PushRuntimeIntoCurrentSession();
                EditorWindowDependencyPusher.FileSystemLiaison.WriteMapToFile(currentSession);
                AssetDatabase.Refresh();
                Refresh();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Existing maps", EditorStyles.largeLabel);

            foreach(var session in EditorWindowDependencyPusher.FileSystemLiaison.LoadedMaps) {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(session.Name);
                if(GUILayout.Button("Load map")) {
                    EditorWindowDependencyPusher.SessionManager.CurrentSession = session;
                    EditorWindowDependencyPusher.SessionManager.PullRuntimeFromCurrentSession();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DoOnSceneGUI(SceneView sceneView) {
            switch(CurrentInteractionMode) {
                case SceneViewInteractionMode.Viewing:  break;
                case SceneViewInteractionMode.MapEdges: DoOnSceneGUI_MapEdges(sceneView); break;
                case SceneViewInteractionMode.Highways: DoOnSceneGUI_Highways(sceneView); break;
                default: break;
            }
        }

        private void DoOnSceneGUI_MapEdges(SceneView sceneView) {
            MapEditorLogic_MapEdgesMode.Instance.DoOnSceneGUI(sceneView);
        }

        private void DoOnSceneGUI_Highways(SceneView sceneView) {
            MapEditorLogic_HighwayMode.Instance.DoOnSceneGUI(sceneView);
        }
        private void Refresh() {
            EditorWindowDependencyPusher.FileSystemLiaison.RefreshLoadedMaps();
            EditorWindowDependencyPusher.FileSystemLiaison.RefreshLoadedSavedGames();
        }

        #endregion

    }

}
