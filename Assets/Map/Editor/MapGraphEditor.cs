using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Map.Editor {

    [CustomEditor(typeof(MapGraph))]
    public class MapGraphEditor : UnityEditor.Editor {

        #region instance fields and properties

        private MapNode FromNode = null;
        private MapNode ToNode = null;

        private string NewAssetName = "";

        private MapAsset MapAssetToLoad;

        private MapGraph TargetedGraph {
            get { return target as MapGraph; }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnSceneGUI() {
            DrawAllEdges();

            var currentEvent = Event.current;

            if(currentEvent.type == EventType.Layout) {
                HandleUtility.AddDefaultControl(0);
            }

            if(FromNode != null) {
                Handles.color = Color.white;
                if(ToNode != null) {
                    Handles.DrawLine(FromNode.transform.position, ToNode.transform.position);
                }else {
                    var mouseRayOrigin = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;
                    Handles.DrawLine(FromNode.transform.position, 
                        new Vector3(mouseRayOrigin.x, mouseRayOrigin.y, FromNode.transform.position.z));
                }
                
            }

            switch(Event.current.type) {
                case EventType.MouseDown: HandleMouseDown(currentEvent, GetCandidateNode(currentEvent)); break;
                case EventType.MouseDrag: HandleMouseDrag(currentEvent, GetCandidateNode(currentEvent)); break;
                case EventType.MouseUp:   HandleMouseUp  (currentEvent, GetCandidateNode(currentEvent)); break;
            }
            
            HandleUtility.Repaint();
        }

        #endregion

        #region from Editor

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            NewAssetName = GUILayout.TextField(NewAssetName);
            if(GUILayout.Button("Save Configuration to Asset")) {
                if(string.IsNullOrEmpty(NewAssetName) || NewAssetName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) {
                    Debug.LogErrorFormat("Failed to save configuration: '{0}' is not a valid asset name", NewAssetName);
                }else {
                    var newMapAsset = CreateInstance<MapAsset>();
                    newMapAsset.LoadMapGraphInto(TargetedGraph);
                    AssetDatabase.CreateAsset(newMapAsset, "Assets/Map/Configurations/" + NewAssetName + ".asset");
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            MapAssetToLoad = EditorGUILayout.ObjectField("Asset to load", MapAssetToLoad, typeof(MapAsset), true) as MapAsset;
            if(GUILayout.Button("Load Configuration From Asset")) {
                if(MapAssetToLoad == null){
                    Debug.LogError("Failed to load configuration from asset: Configuration to load was null");
                }else {
                    TargetedGraph.LoadFromMapAsset(MapAssetToLoad);
                    MapAssetToLoad = null;
                }
            }
        }

        #endregion

        private void HandleMouseDown(Event evnt, MapNode candidateNode) {
            FromNode = null;
            ToNode = null;

            if(candidateNode != null) {
                FromNode = candidateNode;
                evnt.Use();
            }
        }

        private void HandleMouseDrag(Event evnt, MapNode candidateNode) {
            if(candidateNode != null && candidateNode != FromNode) {
                ToNode = candidateNode;
                evnt.Use();
            }else {
                ToNode = null;
            }
        }

        private void HandleMouseUp(Event evnt, MapNode candidateNode) {
            if(FromNode != null && ToNode != null) {
                if(TargetedGraph.GetEdge(FromNode, ToNode) == null) {
                    TargetedGraph.BuildMapEdge(FromNode, ToNode);
                }
                evnt.Use();
            }
            
            FromNode = null;
            ToNode = null;

        }

        private MapNode GetCandidateNode(Event evnt) {
            var mouseRay = HandleUtility.GUIPointToWorldRay(evnt.mousePosition);
            MapNode candidateNode = null;

            foreach(var raycastHit in Physics2D.GetRayIntersectionAll(mouseRay)) {
                candidateNode = raycastHit.transform.GetComponent<MapNode>();
                if(candidateNode != null) break;
            }
            return candidateNode;
        }

        private void DrawAllEdges() {
            if(TargetedGraph == null) {
                return;
            }
            foreach(var edge in TargetedGraph.Edges) {
                if(edge != null && edge.FirstNode != null && edge.SecondNode != null) {
                    Handles.color = Color.white;
                    Handles.DrawLine(edge.FirstNode.transform.position, edge.SecondNode.transform.position);
                    var midpoint = (edge.FirstNode.transform.position + edge.SecondNode.transform.position ) / 2f;
                    Handles.color = Color.red;
                    if(Handles.Button(midpoint, Quaternion.identity, 0.25f, 0.25f, Handles.SphereCap)) {
                        TargetedGraph.DestroyMapEdge(edge);
                        break;
                    }
                }
                
            }
        }

        #endregion

    }

}
