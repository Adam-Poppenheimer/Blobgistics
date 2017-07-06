using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.Societies;

namespace Assets.Societies.Editor {

    [CustomEditor(typeof(Society))]
    public class SocietyEditor : UnityEditor.Editor {

        #region instance fields and properties

        private Society TargetedSociety {
            get { return target as Society; }
        }

        #endregion

        #region instance methods

        #region from Editor

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            var currentComplexity = TargetedSociety.CurrentComplexity;

            EditorGUILayout.LabelField(string.Format("Current Complexity: {0}",
                currentComplexity != null ? currentComplexity.name : "None"));

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if(TargetedSociety.ActiveComplexityLadder != null) {
                EditorGUILayout.LabelField("Ascent Transitions");
                foreach(var ascentTransition in TargetedSociety.ActiveComplexityLadder.GetAscentTransitions(TargetedSociety.CurrentComplexity)) {
                    EditorGUI.BeginDisabledGroup(!ascentTransition.PermittedTerrains.Contains(TargetedSociety.Location.Terrain));
                    if(GUILayout.Button(ascentTransition.name)) {
                        TargetedSociety.SetCurrentComplexity(ascentTransition);
                    }
                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.Space();



                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Descent Transitions");
                foreach(var descentTransition in TargetedSociety.ActiveComplexityLadder.GetDescentTransitions(TargetedSociety.CurrentComplexity)) {
                    EditorGUI.BeginDisabledGroup(!descentTransition.PermittedTerrains.Contains(TargetedSociety.Location.Terrain));
                    if(GUILayout.Button(descentTransition.name)) {
                        TargetedSociety.SetCurrentComplexity(descentTransition);
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        #endregion

        #endregion

    }

}
