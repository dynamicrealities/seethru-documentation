// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace SeeThru.Inspectors
{
    [CustomEditor(typeof(OrbitingCamera))]
    public class OrbitingCameraInspector : SeeThruInspector
    {
        SerializedProperty m_Target;
        SerializedProperty m_HorizontalAxis;
        SerializedProperty m_VerticalAxis;
        SerializedProperty m_HorizontalSensitivity;
        SerializedProperty m_VerticalSensitivity;
        SerializedProperty m_TargetDistance;
        SerializedProperty m_MinTargetDistance;
        SerializedProperty m_MaxTargetDistance;
        SerializedProperty m_DistanceAxis;
        SerializedProperty m_DistanceStep;
        SerializedProperty m_MultiplyByDistanceAxis;
        SerializedProperty m_CollideWithEnvironment;
        SerializedProperty m_ObstructionMask;
        SerializedProperty m_OrbitSpeed;
        SerializedProperty m_MinVerticalAngle;
        SerializedProperty m_MaxVerticalAngle;
        SerializedProperty m_StayBehindTarget;
        SerializedProperty m_StayBehindDelay;
        SerializedProperty m_AlignSmoothRange;
        SerializedProperty m_UseTargetDeviation;
        SerializedProperty m_TargetDeviation;
        SerializedProperty m_TargetCentering;

        public override void OnEnable()
        {
            base.OnEnable();
            m_Target = serializedObject.FindProperty("Target");
            m_HorizontalAxis = serializedObject.FindProperty("HorizontalAxis");
            m_VerticalAxis = serializedObject.FindProperty("VerticalAxis");
            m_HorizontalSensitivity = serializedObject.FindProperty("HorizontalSensitivity");
            m_VerticalSensitivity = serializedObject.FindProperty("VerticalSensitivity");
            m_TargetDistance = serializedObject.FindProperty("TargetDistance");
            m_MinTargetDistance = serializedObject.FindProperty("MinTargetDistance");
            m_MaxTargetDistance = serializedObject.FindProperty("MaxTargetDistance");
            m_DistanceAxis = serializedObject.FindProperty("DistanceAxis");
            m_DistanceStep = serializedObject.FindProperty("DistanceStep");
            m_MultiplyByDistanceAxis = serializedObject.FindProperty("MultiplyByDistanceAxis");
            m_CollideWithEnvironment = serializedObject.FindProperty("CollideWithEnvironment");
            m_ObstructionMask = serializedObject.FindProperty("ObstructionMask");
            m_OrbitSpeed = serializedObject.FindProperty("OrbitSpeed");
            m_MinVerticalAngle = serializedObject.FindProperty("MinVerticalAngle");
            m_MaxVerticalAngle = serializedObject.FindProperty("MaxVerticalAngle");
            m_StayBehindTarget = serializedObject.FindProperty("StayBehindTarget");
            m_StayBehindDelay = serializedObject.FindProperty("StayBehindDelay");
            m_AlignSmoothRange = serializedObject.FindProperty("AlignSmoothRange");
            m_UseTargetDeviation = serializedObject.FindProperty("UseTargetDeviation");
            m_TargetDeviation = serializedObject.FindProperty("TargetDeviation");
            m_TargetCentering = serializedObject.FindProperty("TargetCentering");
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
            OrbitingCamera camera = serializedObject.targetObject as OrbitingCamera;
            EditorGUILayout.PropertyField(m_Target);
            EditorGUILayout.PropertyField(m_HorizontalAxis);
            EditorGUILayout.PropertyField(m_VerticalAxis);
            EditorGUILayout.PropertyField(m_HorizontalSensitivity);
            EditorGUILayout.PropertyField(m_VerticalSensitivity);
            EditorGUILayout.PropertyField(m_TargetDistance);
            EditorGUILayout.PropertyField(m_MinTargetDistance);
            EditorGUILayout.PropertyField(m_MaxTargetDistance);
            EditorGUILayout.PropertyField(m_DistanceAxis);
            EditorGUILayout.PropertyField(m_DistanceStep);
            EditorGUILayout.PropertyField(m_MultiplyByDistanceAxis);
            EditorGUILayout.PropertyField(m_CollideWithEnvironment);
            if(camera.CollideWithEnvironment)
            {
                EditorGUILayout.PropertyField(m_ObstructionMask);
            }
            EditorGUILayout.PropertyField(m_OrbitSpeed);
            EditorGUILayout.PropertyField(m_MinVerticalAngle);
            EditorGUILayout.PropertyField(m_MaxVerticalAngle);
            EditorGUILayout.PropertyField(m_StayBehindTarget);
            if(camera.StayBehindTarget)
            {
                EditorGUILayout.PropertyField(m_StayBehindDelay);
                EditorGUILayout.PropertyField(m_AlignSmoothRange);
            }
            EditorGUILayout.PropertyField(m_UseTargetDeviation);
            if(camera.UseTargetDeviation)
            {
                EditorGUILayout.PropertyField(m_TargetDeviation);
                EditorGUILayout.PropertyField(m_TargetCentering);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
