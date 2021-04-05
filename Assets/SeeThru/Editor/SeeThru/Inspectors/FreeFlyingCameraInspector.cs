// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using UnityEditor;

namespace SeeThru.Inspectors
{
    [CustomEditor(typeof(FreeFlyingCamera))]
    public class FreeFlyingCameraInspector : SeeThruInspector
    {
        SerializedProperty m_buttonSetup;
        SerializedProperty m_UseCage;
        SerializedProperty m_Cage;
        SerializedProperty m_InvertedRotation;
        SerializedProperty m_UseAxisForMovement;
        SerializedProperty m_HorizontalMovementAxis;
        SerializedProperty m_VerticalMovementAxis;
        SerializedProperty m_HorizontalRotationAxis;
        SerializedProperty m_VerticalRotationAxis;
        SerializedProperty m_HorizontalSensitivity;
        SerializedProperty m_VerticalSensitivity;
        SerializedProperty m_MovementSpeed;
        SerializedProperty m_UseSmoothing;
        SerializedProperty m_SmoothTime;
        SerializedProperty m_UseAxisForSpeed;
        SerializedProperty m_SpeedStepAxis;
        SerializedProperty m_SpeedStep;

        public override void OnEnable()
        {
            base.OnEnable();
            m_buttonSetup = serializedObject.FindProperty("buttonSetup");
            m_UseCage = serializedObject.FindProperty("UseCage");
            m_Cage = serializedObject.FindProperty("Cage");
            m_InvertedRotation = serializedObject.FindProperty("InvertedRotation");
            m_UseAxisForMovement = serializedObject.FindProperty("UseAxisForMovement");
            m_HorizontalMovementAxis = serializedObject.FindProperty("HorizontalMovementAxis");
            m_VerticalMovementAxis = serializedObject.FindProperty("VerticalMovementAxis");
            m_HorizontalRotationAxis = serializedObject.FindProperty("HorizontalRotationAxis");
            m_VerticalRotationAxis = serializedObject.FindProperty("VerticalRotationAxis");
            m_HorizontalSensitivity = serializedObject.FindProperty("HorizontalSensitivity");
            m_VerticalSensitivity = serializedObject.FindProperty("VerticalSensitivity");
            m_MovementSpeed = serializedObject.FindProperty("MovementSpeed");
            m_UseSmoothing = serializedObject.FindProperty("UseSmoothing");
            m_SmoothTime = serializedObject.FindProperty("SmoothTime");
            m_UseAxisForSpeed = serializedObject.FindProperty("UseAxisForSpeed");
            m_SpeedStepAxis = serializedObject.FindProperty("SpeedStepAxis");
            m_SpeedStep = serializedObject.FindProperty("SpeedStep");
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
            FreeFlyingCamera camera = serializedObject.targetObject as FreeFlyingCamera;
            EditorGUILayout.PropertyField(m_UseAxisForMovement);
            if (camera.UseAxisForMovement == true)
            {
                EditorGUILayout.PropertyField(m_HorizontalMovementAxis);
                EditorGUILayout.PropertyField(m_VerticalMovementAxis);
            }
            else
            {
                EditorGUILayout.PropertyField(m_buttonSetup);
            }
            EditorGUILayout.PropertyField(m_UseCage);
            if (camera.UseCage == true)
            {
                EditorGUILayout.PropertyField(m_Cage);
            }
            EditorGUILayout.PropertyField(m_InvertedRotation);
            EditorGUILayout.PropertyField(m_HorizontalRotationAxis);
            EditorGUILayout.PropertyField(m_VerticalRotationAxis);
            EditorGUILayout.PropertyField(m_HorizontalSensitivity);
            EditorGUILayout.PropertyField(m_VerticalSensitivity);
            EditorGUILayout.PropertyField(m_MovementSpeed);
            EditorGUILayout.PropertyField(m_UseSmoothing);
            if (camera.UseSmoothing == true)
            {
                EditorGUILayout.PropertyField(m_SmoothTime);
            }
            EditorGUILayout.PropertyField(m_UseAxisForSpeed);
            if (camera.UseAxisForSpeed == true)
            {
                EditorGUILayout.PropertyField(m_SpeedStepAxis);
            }
            EditorGUILayout.PropertyField(m_SpeedStep);
            serializedObject.ApplyModifiedProperties();
        }
    }
}