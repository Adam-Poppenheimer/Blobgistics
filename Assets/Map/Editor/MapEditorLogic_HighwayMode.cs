using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.Util;

namespace Assets.Map.Editor {

    public class MapEditorLogic_HighwayMode {

        #region static fields and properties

        public static MapEditorLogic_HighwayMode Instance {
            get {
                if(_instance == null) {
                    _instance = new MapEditorLogic_HighwayMode();
                }
                return _instance;
            }
        }
        private static MapEditorLogic_HighwayMode _instance;

        #endregion

        #region instance fields and properties

        private MapNodeBase FromNode = null;
        private MapNodeBase ToNode = null;

        #endregion

        #region instance methods

        public void DoOnSceneGUI(SceneView sceneView) {
            DrawAllEdges();

            var currentEvent = Event.current;

            if(currentEvent.type == EventType.Layout) {
                HandleUtility.AddDefaultControl(0);
            }

            if(FromNode != null) {
                Handles.color = Color.blue;
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
                var highwayfactory = EditorWindowDependencyPusher.HighwayFactory;
                if(highwayfactory.CanConstructHighwayBetween(FromNode, ToNode)) {
                    highwayfactory.ConstructHighwayBetween(FromNode, ToNode);
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
            if(EditorWindowDependencyPusher.MapGraph == null) {
                return;
            }
            foreach(var edge in EditorWindowDependencyPusher.MapGraph.Edges) {
                if(edge != null && edge.FirstNode != null && edge.SecondNode != null &&
                    EditorWindowDependencyPusher.HighwayFactory.CanConstructHighwayBetween(edge.FirstNode, edge.SecondNode)) {
                    Handles.color = Color.green;
                    Handles.DrawLine(edge.FirstNode.transform.position, edge.SecondNode.transform.position);
                }
            }
        }

        #endregion

    }

}
