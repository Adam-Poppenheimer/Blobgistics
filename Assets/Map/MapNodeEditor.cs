using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.Map {

    [CustomEditor(typeof(MapNode))]
    public class MapNodeEditor : Editor {

        #region instance fields and properties

        private MapNode TargetedNode {
            get { return target as MapNode; }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnSceneGUI() {
            foreach(var edge in TargetedNode.ParentGraph.GetEdgesAttachedToNode(TargetedNode)) {
                Handles.color = Color.white;
                Handles.DrawLine(edge.FirstNode.transform.position, edge.SecondNode.transform.position);
                var midpoint = (edge.FirstNode.transform.position + edge.SecondNode.transform.position ) / 2f;
                Handles.color = Color.red;
                if(Handles.Button(midpoint, Quaternion.identity, 0.25f, 0.25f, Handles.SphereCap)) {
                    TargetedNode.ParentGraph.DestroyUndirectedEdge(edge);
                    break;
                }
            }
        }

        #endregion

        #endregion

    }

}
