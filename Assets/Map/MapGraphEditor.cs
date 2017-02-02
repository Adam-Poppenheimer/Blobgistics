using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityCustomUtilities.Extensions;

namespace Assets.Map {

    [CustomEditor(typeof(MapGraph))]
    public class MapGraphEditor : Editor {

        #region instance fields and properties

        private MapNode FromNode = null;
        private MapNode ToNode = null;

        private MapGraph TargetedGraph {
            get { return target as MapGraph; }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnSceneGUI() {
            DrawAllEdges(TargetedGraph.Nodes);

            var currentEvent = Event.current;

            if(currentEvent.type == EventType.Layout) {
                HandleUtility.AddDefaultControl(0);
            }

            if(FromNode != null) {
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
                TargetedGraph.TryAddNode(FromNode);
                TargetedGraph.TryAddNode(ToNode);
                if(!TargetedGraph.HasEdge(FromNode, ToNode)) {
                    Debug.Log("Adding edge");
                    TargetedGraph.AddUndirectedEdge(FromNode, ToNode);
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

        private void DrawAllEdges(IEnumerable<MapNode> allNodes) {
            foreach(var activeNode in allNodes) {
                foreach(var neighbor in activeNode.Neighbors) {
                    Handles.DrawLine(activeNode.transform.position, neighbor.transform.position);
                }
            }
        }

        #endregion

    }

}
