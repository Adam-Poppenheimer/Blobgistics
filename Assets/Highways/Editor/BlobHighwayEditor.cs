using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways.Editor {

    [CustomEditor(typeof(BlobHighway))]
    public class BlobHighwayEditor : UnityEditor.Editor {

        #region instance fields and properties

        private BlobHighway TargetedHighway {
            get { return target as BlobHighway; }
        }

        private SerializedObject FirstEndpointPullingPermissionObject;
        private SerializedObject SecondEndpointPullingPermissionObject;

        private SerializedProperty FirstEndpointPullingPermissionProperty;
        private SerializedProperty SecondEndpointPullingPermissionProperty;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnEnable() {
            var firstTube  = TargetedHighway.TubePullingFromFirstEndpoint as BlobTube;
            var secondTube = TargetedHighway.TubePullingFromSecondEndpoint as BlobTube;

            FirstEndpointPullingPermissionObject  = new SerializedObject(firstTube.PermissionsForBlobTypes);
            SecondEndpointPullingPermissionObject = new SerializedObject(secondTube.PermissionsForBlobTypes);

            FirstEndpointPullingPermissionProperty  = FirstEndpointPullingPermissionObject.FindProperty ("ValueList");
            SecondEndpointPullingPermissionProperty = SecondEndpointPullingPermissionObject.FindProperty("ValueList");
        }

        private void OnSceneGUI() {
            Handles.Label(TargetedHighway.FirstEndpoint.transform.position, "First Endpoint", EditorStyles.boldLabel);
            Handles.Label(TargetedHighway.SecondEndpoint.transform.position, "Second Endpoint", EditorStyles.boldLabel);
        }

        #endregion

        #region from Editor

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("First endpoint pulling permissions", EditorStyles.largeLabel);
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                EditorGUILayout.BeginHorizontal();

                var arrayProperty = FirstEndpointPullingPermissionProperty.GetArrayElementAtIndex((int)resourceType);
                arrayProperty.boolValue = EditorGUILayout.Toggle(resourceType.GetDescription(), arrayProperty.boolValue);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Second endpoint pulling permissions", EditorStyles.largeLabel);
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                EditorGUILayout.BeginHorizontal();

                var arrayProperty = SecondEndpointPullingPermissionProperty.GetArrayElementAtIndex((int)resourceType);
                arrayProperty.boolValue = EditorGUILayout.Toggle(resourceType.GetDescription(), arrayProperty.boolValue);

                EditorGUILayout.EndHorizontal();
            }

            FirstEndpointPullingPermissionObject.ApplyModifiedProperties();
            SecondEndpointPullingPermissionObject.ApplyModifiedProperties();
        }

        #endregion

        #endregion

    }

}
