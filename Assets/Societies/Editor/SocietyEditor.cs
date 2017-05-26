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
            var currentComplexity = TargetedSociety.CurrentComplexity;

            EditorGUILayout.LabelField("Current Complexity");

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Ascent Transitions");
            foreach(var ascentTransition in TargetedSociety.ActiveComplexityLadder.GetAscentTransitions(TargetedSociety.CurrentComplexity)) {
                EditorGUI.BeginDisabledGroup(!ascentTransition.PermittedTerrains.Contains(TargetedSociety.Location.Terrain));
                if(GUILayout.Button(ascentTransition.Name)) {
                    TargetedSociety.SetCurrentComplexity(ascentTransition);
                }
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Space();



            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Descent Transitions");
            foreach(var descentTransition in TargetedSociety.ActiveComplexityLadder.GetDescentTransitions(TargetedSociety.CurrentComplexity)) {
                EditorGUI.BeginDisabledGroup(!descentTransition.PermittedTerrains.Contains(TargetedSociety.Location.Terrain));
                if(GUILayout.Button(descentTransition.Name)) {
                    TargetedSociety.SetCurrentComplexity(descentTransition);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        #endregion

        #endregion

    }

}
