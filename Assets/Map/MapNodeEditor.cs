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
            foreach(var neighbor in TargetedNode.Neighbors) {
                Handles.DrawLine(TargetedNode.transform.position, neighbor.transform.position);
            }
        }

        #endregion

        #endregion

    }

}
