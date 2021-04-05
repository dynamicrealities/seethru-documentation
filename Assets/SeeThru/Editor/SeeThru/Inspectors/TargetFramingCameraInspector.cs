// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using UnityEditor;

namespace SeeThru.Inspectors
{
    [CustomEditor(typeof(TargetFramingCamera))]
    public class TargetFramingCameraInspector : SeeThruInspector
    {
        SerializedProperty m_DefaultTarget;
        SerializedProperty m_Targets;
        SerializedProperty m_Zoom;
        SerializedProperty m_CamMinDistance;
        SerializedProperty m_UseSmoothing;
        SerializedProperty m_SmoothTime;

        public override void OnEnable()
        {
            base.OnEnable();
            m_DefaultTarget = serializedObject.FindProperty("DefaultTarget");
            m_Targets = serializedObject.FindProperty("Targets");
            m_Zoom = serializedObject.FindProperty("Zoom");
            m_CamMinDistance = serializedObject.FindProperty("CamMinDistance");
            m_UseSmoothing = serializedObject.FindProperty("UseSmoothing");
            m_SmoothTime = serializedObject.FindProperty("SmoothTime");
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            TargetFramingCamera camera = serializedObject.targetObject as TargetFramingCamera;
            EditorGUILayout.PropertyField(m_IsCameraActive);
            if(camera.UseSmoothing == true)
            {
                EditorGUILayout.PropertyField(m_UseUnscaledTime);
            }
            EditorGUILayout.PropertyField(m_CursorLockMode);
            EditorGUILayout.PropertyField(m_IsCursorVisible);

            EditorGUILayout.PropertyField(m_DefaultTarget);
            EditorGUILayout.PropertyField(m_Targets);
            EditorGUILayout.PropertyField(m_Zoom);
            EditorGUILayout.PropertyField(m_CamMinDistance);
            EditorGUILayout.PropertyField(m_UseSmoothing);
            if(camera.UseSmoothing == true)
            {
                EditorGUILayout.PropertyField(m_SmoothTime);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
