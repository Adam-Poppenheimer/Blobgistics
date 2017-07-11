using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs.Editor {

    public abstract class ResourceSummaryEditorBase : UnityEditor.Editor {

        #region instance fields and properties

        private SerializedProperty CountListProperty;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnEnable() {
            CountListProperty = serializedObject.FindProperty("ValueList");
        }

        #endregion

        #region from Editor

        public override void OnInspectorGUI() {
            serializedObject.Update();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(CountListProperty.arraySize > (int)resourceType) {
                    EditorGUILayout.PropertyField(
                        CountListProperty.GetArrayElementAtIndex((int)resourceType),
                        new GUIContent(resourceType.GetDescription())
                    );
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #endregion

    }
}
