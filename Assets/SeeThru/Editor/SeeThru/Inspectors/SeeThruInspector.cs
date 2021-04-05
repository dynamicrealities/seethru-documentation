// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using UnityEditor;

namespace SeeThru.Inspectors
{
    [CustomEditor(typeof(SeeThruCamera))]
    public abstract class SeeThruInspector : Editor
    {
        internal SerializedProperty m_IsCameraActive;
        internal SerializedProperty m_UseUnscaledTime;
        internal SerializedProperty m_CursorLockMode;
        internal SerializedProperty m_IsCursorVisible;

        public virtual void OnEnable()
        {
            m_IsCameraActive = serializedObject.FindProperty("IsCameraActive");
            m_UseUnscaledTime = serializedObject.FindProperty("UseUnscaledTime");
            m_CursorLockMode = serializedObject.FindProperty("cursorLockMode");
            m_IsCursorVisible = serializedObject.FindProperty("IsCursorVisible");
        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_IsCameraActive);
            EditorGUILayout.PropertyField(m_UseUnscaledTime);
            EditorGUILayout.PropertyField(m_CursorLockMode);
            EditorGUILayout.PropertyField(m_IsCursorVisible);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
