using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Assets.UI.Editor {

    [CustomEditor(typeof(LabeledButton))]
    public class LabeledButtonEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }

    }

}
