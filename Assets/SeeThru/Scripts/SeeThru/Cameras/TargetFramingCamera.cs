// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using System.Collections.Generic;
using UnityEngine;

namespace SeeThru.Cameras
{
    [AddComponentMenu("SeeThru/Target Framing Camera")]
    public class TargetFramingCamera : SeeThruCamera
    {
        [Header("TargetFramingCamera - Targets")]
        [Tooltip("If there are 0 targets, the camera will look here.")]
        public GameObject DefaultTarget;
        [Tooltip("The targets to follow.")]
        public List<GameObject> Targets;
        [Header("TargetFramingCamera - Distance")]
        [Tooltip("How much the camera should be zoomed in.")]
        public float Zoom = 1.5f;
        [Tooltip("The minimum distance the camera should maintain from its targets.")]
        public float CamMinDistance = 5f;
        [Header("TargetFramingCamera - Smoothing")]
        [Tooltip("Whether to smooth the camera to its new destination when it moves or not.")]
        public bool UseSmoothing = true;
        [Tooltip("The time it takes for the camera to smooth step to the new camera destination.")]
        public float SmoothTime = 0.15f;

        private Vector3 _smoothingVelocity;

        public override void Start()
        {
            base.Start();
            if (Targets == null)
            {
                Targets = new List<GameObject>(10);
            }
        }

        private void Update()
        {
            if (IsCameraActive)
            {
                if (Targets != null)
                {
                    Vector3 midpoint = transform.position;
                    float distance = 0f;
                    switch (Targets.Count)
                    {
                        case 0:
                            midpoint = DefaultTarget.transform.position;
                            break;
                        case 1:
                            midpoint = Targets[0].transform.position;
                            distance = 0f;
                            break;
                        default:
                            GetTargetBounds(out Vector3 lower, out Vector3 upper);
                            midpoint = (upper + lower) / 2f;
                            distance = (upper - lower).magnitude;
                            break;
                    }

                    Vector3 cameraDestination = midpoint - transform.forward * distance * Zoom;

                    if (distance < CamMinDistance)
                    {
                        cameraDestination = midpoint - transform.forward * CamMinDistance * Zoom;
                    }

                    if (UseSmoothing)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, cameraDestination, ref _smoothingVelocity, SmoothTime, Mathf.Infinity, DeltaTime);
                    }
                    else
                    {
                        transform.position = cameraDestination;
                    }

                    // Snap when close enough to prevent unwanted slerp behavior
                    if ((cameraDestination - transform.position).magnitude <= 0.05f)
                        transform.position = cameraDestination;
                }
            }
        }

        private void GetTargetBounds(out Vector3 lower, out Vector3 upper)
        {
            float minX = 0f, minY = 0f, minZ = 0f;
            float maxX = 0f, maxY = 0f, maxZ = 0f;
            foreach (GameObject target in Targets)
            {
                Vector3 targetPos = target.transform.position;
                if (targetPos.x < minX) minX = targetPos.x;
                if (targetPos.y < minY) minY = targetPos.y;
                if (targetPos.z < minZ) minZ = targetPos.z;
                if (targetPos.x > maxX) maxX = targetPos.x;
                if (targetPos.y > maxY) maxY = targetPos.y;
                if (targetPos.z > maxZ) maxZ = targetPos.z;
            }

            lower = new Vector3(minX, minY, minZ);
            upper = new Vector3(maxX, maxY, maxZ);
        }
    }
}