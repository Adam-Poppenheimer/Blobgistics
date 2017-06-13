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

        private string CurrentSessionName;
        private string CurrentSessionDescription;
        private int CurrentSessionScoreToWin;

        #endregion

        #region static methods

        [MenuItem("Strategy Blobs/Open Map Editor")]
        public static void ShowWindow() {
            GetWindow<MapEditorWindow>();
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnEnable() {
            SceneView.onSceneGUIDelegate -= DoOnSceneGUI;
            SceneView.onSceneGUIDelegate += DoOnSceneGUI;
            ResetCurrentSessionData();
        }

        private void OnDisable() {
            SceneView.onSceneGUIDelegate -= DoOnSceneGUI;
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

        private void OnGUI_MapSerialization() {
            EditorGUILayout.BeginVertical();

            EditorGUI.BeginDisabledGroup(EditorWindowDependencyPusher.SessionManager.CurrentSession == null);

            CurrentSessionName        = EditorGUILayout.DelayedTextField("Name", CurrentSessionName);
            CurrentSessionDescription = EditorGUILayout.TextArea(CurrentSessionDescription);
            CurrentSessionScoreToWin  = EditorGUILayout.DelayedIntField("Score to Win", CurrentSessionScoreToWin);

            if(GUILayout.Button("Save current map to file")) {
                var sessionPulled = EditorWindowDependencyPusher.SessionManager.PullSessionFromRuntime(CurrentSessionName,
                    CurrentSessionDescription, CurrentSessionScoreToWin);
                EditorWindowDependencyPusher.FileSystemLiaison.WriteMapToFile(sessionPulled);
                AssetDatabase.Refresh();
                ResetCurrentSessionData();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        private void DoOnSceneGUI(SceneView sceneView) {
            Handles.BeginGUI();

            switch(CurrentInteractionMode) {
                case SceneViewInteractionMode.Viewing:  break;
                case SceneViewInteractionMode.MapEdges: DoOnSceneGUI_MapEdges(sceneView); break;
                case SceneViewInteractionMode.Highways: DoOnSceneGUI_Highways(sceneView); break;
                default: break;
            }

            Handles.EndGUI();
        }

        private void DoOnSceneGUI_MapEdges(SceneView sceneView) {

        }

        private void DoOnSceneGUI_Highways(SceneView sceneView) {
            DrawAllMapEdges();
            MapEditorLogic_HighwayMode.Instance.DoOnSceneGUI(sceneView);
        }

        private void DrawAllMapEdges() {
            var mapGraph = EditorWindowDependencyPusher.MapGraph;
            if(mapGraph == null) {
                return;
            }
            foreach(var edge in mapGraph.Edges) {
                if(edge != null && edge.FirstNode != null && edge.SecondNode != null) {
                    Handles.color = Color.white;
                    Handles.DrawLine(edge.FirstNode.transform.position, edge.SecondNode.transform.position);
                }
            }
        }

        private void ResetCurrentSessionData() {
            var currentSession = EditorWindowDependencyPusher.SessionManager.CurrentSession;
            if(currentSession != null) {
                CurrentSessionName = currentSession.Name;
                CurrentSessionDescription = currentSession.Description;
                CurrentSessionScoreToWin = currentSession.ScoreToWin;
            }            
        }

        #endregion

    }

}
