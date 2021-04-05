// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SeeThru.Menu
{
    public class SeeThruMenuItems
    {
        [MenuItem("GameObject/SeeThru/Free Flying Camera", false, 12)]
        private static void CreateNewFreeFlyingCamera()
        {
            EditorUtility.CreateGameObjectWithHideFlags("FreeFlyingCamera", HideFlags.None, typeof(FreeFlyingCamera));
        }

        [MenuItem("GameObject/SeeThru/Target Framing Camera", false, 13)]
        private static void CreateNewTargetFramingCamera()
        {
            EditorUtility.CreateGameObjectWithHideFlags("TargetFramingCamera", HideFlags.None, typeof(TargetFramingCamera));
        }

        [MenuItem("GameObject/SeeThru/Orbiting Camera", false, 14)]
        private static void CreateNewOrbitingCamera()
        {
            EditorUtility.CreateGameObjectWithHideFlags("OrbitingCamera", HideFlags.None, typeof(OrbitingCamera));
        }

        [MenuItem("GameObject/SeeThru/RTS Camera", false, 15)]
        private static void CreateNewRTSCamera()
        {
            EditorUtility.CreateGameObjectWithHideFlags("RTSCamera", HideFlags.None, typeof(RTSCamera));
        }
    }
}