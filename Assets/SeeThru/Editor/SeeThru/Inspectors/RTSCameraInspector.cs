// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using UnityEditor;

namespace SeeThru.Inspectors
{
    [CustomEditor(typeof(RTSCamera))]
    public class RTSCameraInspector : SeeThruInspector
    {
        SerializedProperty m_AxisSetup;
        SerializedProperty m_UseMouseForMovement;
        SerializedProperty m_MouseBoundaries;
        SerializedProperty m_ZoomAxis;
        SerializedProperty m_ButtonSetup;
        SerializedProperty m_ZoomStep;
        SerializedProperty m_Speed;
        SerializedProperty m_UseSmoothing;
        SerializedProperty m_SmoothTime;
        SerializedProperty m_CanRotate;
        SerializedProperty m_RotationStep;
        SerializedProperty m_RotateClockWise;
        SerializedProperty m_RotateCounterClockwise;
        SerializedProperty m_RotationLerpTime;
        SerializedProperty m_ShowMouseBoundary;
        SerializedProperty m_ShowDirectionLines;
        SerializedProperty m_ForwardColor;
        SerializedProperty m_BackwardsColor;
        SerializedProperty m_LeftColor;
        SerializedProperty m_RightColor;

        public override void OnEnable()
        {
            base.OnEnable();
            m_AxisSetup = serializedObject.FindProperty("AxisSetup");
            m_UseMouseForMovement = serializedObject.FindProperty("UseMouseForMovement");
            m_MouseBoundaries = serializedObject.FindProperty("MouseBoundaries");
            m_ZoomAxis = serializedObject.FindProperty("ZoomAxis");
            m_ButtonSetup = serializedObject.FindProperty("ButtonSetup");
            m_ZoomStep = serializedObject.FindProperty("ZoomStep");
            m_Speed = serializedObject.FindProperty("Speed");
            m_UseSmoothing = serializedObject.FindProperty("UseSmoothing");
            m_SmoothTime = serializedObject.FindProperty("SmoothTime");
            m_CanRotate = serializedObject.FindProperty("CanRotate");
            m_RotationStep = serializedObject.FindProperty("RotationStep");
            m_RotateClockWise = serializedObject.FindProperty("RotateClockwise");
            m_RotateCounterClockwise = serializedObject.FindProperty("RotateCounterClockwise");
            m_RotationLerpTime = serializedObject.FindProperty("RotationLerpTime");
            m_ShowMouseBoundary = serializedObject.FindProperty("ShowMouseBoundary");
            m_ShowDirectionLines = serializedObject.FindProperty("ShowDirectionLines");
            m_ForwardColor = serializedObject.FindProperty("ForwardColor");
            m_BackwardsColor = serializedObject.FindProperty("BackwardsColor");
            m_LeftColor = serializedObject.FindProperty("LeftColor");
            m_RightColor = serializedObject.FindProperty("RightColor");
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
            base.OnInspectorGUI();
            serializedObject.Update();
            RTSCamera camera = serializedObject.targetObject as RTSCamera;
            EditorGUILayout.PropertyField(m_AxisSetup);
            EditorGUILayout.PropertyField(m_UseMouseForMovement);
            if (camera.UseMouseForMovement)
            {
                EditorGUILayout.PropertyField(m_MouseBoundaries);
                EditorGUILayout.PropertyField(m_ZoomAxis);
            }
            else
            {
                EditorGUILayout.PropertyField(m_ButtonSetup);
            }
            EditorGUILayout.PropertyField(m_ZoomStep);
            EditorGUILayout.PropertyField(m_Speed);
            EditorGUILayout.PropertyField(m_UseSmoothing);
            if (camera.UseSmoothing)
            {
                EditorGUILayout.PropertyField(m_SmoothTime);
            }
            EditorGUILayout.PropertyField(m_CanRotate);
            if (camera.CanRotate)
            {
                EditorGUILayout.PropertyField(m_RotationStep);
                EditorGUILayout.PropertyField(m_RotateClockWise);
                EditorGUILayout.PropertyField(m_RotateCounterClockwise);
                EditorGUILayout.PropertyField(m_RotationLerpTime);
            }
            EditorGUILayout.PropertyField(m_ShowDirectionLines);
            if (camera.ShowDirectionLines)
            {
                EditorGUILayout.PropertyField(m_ForwardColor);
                EditorGUILayout.PropertyField(m_BackwardsColor);
                EditorGUILayout.PropertyField(m_LeftColor);
                EditorGUILayout.PropertyField(m_RightColor);
            }
            if (camera.UseMouseForMovement)
            {
                EditorGUILayout.PropertyField(m_ShowMouseBoundary);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
