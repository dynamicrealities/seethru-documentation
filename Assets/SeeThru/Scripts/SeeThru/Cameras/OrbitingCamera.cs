// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using UnityEngine;

namespace SeeThru.Cameras
{
    [AddComponentMenu("SeeThru/Orbiting Camera")]
    public class OrbitingCamera : SeeThruCamera
    {
        [Header("OrbitingCamera - Target")]
        [Tooltip("The target of the Spring Arm.")]
        public Transform Target;

        [Header("OrbitingCamera - Controls")]
        [Tooltip("The Horizontal rotation axis.")]
        public string HorizontalAxis = "Mouse Y";
        [Tooltip("The Vertical rotation axis.")]
        public string VerticalAxis = "Mouse X";
        [Min(0f), Tooltip("The Horizontal Axis Sensitivity")]
        public float HorizontalSensitivity = 1f;
        [Min(0f), Tooltip("The Vertical Axis Sensitivity")]
        public float VerticalSensitivity = 1f;

        [Header("OrbitingCamera - Distance")]
        [Min(0f), Tooltip("How far away from the target in focus is the camera.")]
        public float TargetDistance = 5f;
        [Min(0f), Tooltip("The closest the camera can be to the target.")]
        public float MinTargetDistance = 1.25f;
        [Min(0f), Tooltip("The farthest the camera can be to the target.")]
        public float MaxTargetDistance = 10f;
        [Tooltip("The axis used to move the camera closer or farther away from the target.")]
        public string DistanceAxis = "Mouse ScrollWheel";
        [Min(0f), Tooltip("How far each step of adjusting the distance will add or subtract.")]
        public float DistanceStep = 1f;
        [Tooltip("If this is true, then the DistanceStep gets multiplied by the Axis value to give a relative distance change.")]
        public bool MultiplyByDistanceAxis = false;

        [Header("OrbitingCamera - Orbiting")]
        [Tooltip("Whether the camera should collide with the environment and compensate, or not.")]
        public bool CollideWithEnvironment = true;
        [Tooltip("Determines what will block the camera when it makes collision checks.")]
        public LayerMask ObstructionMask = -1;
        [Range(1f, 360f), Tooltip("How fast the camera can 'Orbit' around its target in degrees per second.")]
        public float OrbitSpeed = 90f;
        [Range(-89f, 89f), Tooltip("The minimum Vertical Angle that the camera can orbit to.")]
        public float MinVerticalAngle = -30f;
        [Range(-89f, 89f), Tooltip("The maximum Vertical Angle that the camera can orbit to.")]
        public float MaxVerticalAngle = 60f;
        [Tooltip("Whether the camera should orbit behind the target when it moves towards the camera or not.")]
        public bool StayBehindTarget = false;
        [Min(0f), Tooltip("How fast the camera should be as it adjusts to be behind the target")]
        public float StayBehindDelay = 0f;
        [Tooltip("Within how many degrees the camera can smoothly rotate to look at its target")]
        [Range(0f, 90f)]
        public float AlignSmoothRange = 45f;

        [Header("OrbitingCamera - Deviation")]
        [Tooltip("If true, then the camera doesn't have to follow the target exactly and can allow slight free movement inside of its view before having to readjust.")]
        public bool UseTargetDeviation = false;
        [Min(0f), Tooltip("How much the target can move while in view, before the camera should adjust.")]
        public float TargetDeviation = 1f;
        [Range(0f, 1f), Tooltip("How fast the camera should center the target when it deviates.")]
        public float TargetCentering = 0.5f;

        private Vector3 _lateTargetPosition;
        private Vector3 _previousTargetPosition;
        private Vector2 _orbitAngles;
        private float _lastRotationTime;
        private Vector3 _cameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = MainCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * MainCamera.fieldOfView);
                halfExtends.x = halfExtends.y * MainCamera.aspect;
                halfExtends.z = 0f;
                return halfExtends;
            }
        }

        public override void Awake()
        {
            base.Awake();
            if (Target != null)
            {
                _lateTargetPosition = Target.position;
            }
            _orbitAngles = new Vector2(45f, 0f);
            transform.rotation = Quaternion.Euler(_orbitAngles);
        }

        public override void Start()
        {
            base.Start();
            if (Target == null)
            {
                Debug.LogError($"{this.GetType().ToString()} was not given a Target.");
            }
        }

        private void LateUpdate()
        {
            if (IsCameraActive)
            {
                if (Target != null)
                {
                    float distanceDelta = Input.GetAxis(DistanceAxis);
                    if (distanceDelta > 0)
                    {
                        TargetDistance -= (MultiplyByDistanceAxis == false ? DistanceStep : Mathf.Abs(DistanceStep * distanceDelta));
                    }
                    else if(distanceDelta < 0)
                    {
                        TargetDistance += (MultiplyByDistanceAxis == false ? DistanceStep : Mathf.Abs(DistanceStep * distanceDelta));
                    }
                    TargetDistance = Mathf.Clamp(TargetDistance, MinTargetDistance, MaxTargetDistance);
                    SetCameraPosition();
                    Quaternion lookRotation;
                    if (StayBehindTarget == false ? SetRotation() : SetRotation() || OrbitBehind())
                    {
                        _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, MinVerticalAngle, MaxVerticalAngle);
                        if (_orbitAngles.y < 0f)
                        {
                            _orbitAngles.y += 360f;
                        }
                        else if (_orbitAngles.y >= 360f)
                        {
                            _orbitAngles.y -= 360f;
                        }
                        lookRotation = Quaternion.Euler(_orbitAngles);
                    }
                    else
                    {
                        lookRotation = transform.rotation;
                    }

                    Vector3 lookDirection = lookRotation * Vector3.forward;
                    Vector3 lookPosition = _lateTargetPosition - lookDirection * TargetDistance;

                    if (CollideWithEnvironment)
                    {
                        Vector3 rectOffset = lookDirection * MainCamera.nearClipPlane;
                        Vector3 rectPosition = lookPosition + rectOffset;
                        Vector3 castFrom = Target.position;
                        Vector3 castLine = rectPosition - castFrom;

                        float castDistance = castLine.magnitude;
                        Vector3 castDirection = castLine / castDistance;

                        if (Physics.BoxCast(castFrom, _cameraHalfExtends, castDirection, out RaycastHit hit, lookRotation, castDistance, ObstructionMask))
                        {
                            rectPosition = castFrom + castDirection * Mathf.Clamp(hit.distance, MinTargetDistance, MaxTargetDistance);
                            lookPosition = rectPosition - rectOffset;
                        }
                    }
                    transform.SetPositionAndRotation(lookPosition, lookRotation);
                }
            }
        }

        private void SetCameraPosition()
        {
            Vector3 targetPosition = Target.position;
            _previousTargetPosition = _lateTargetPosition;
            if (UseTargetDeviation)
            {
                if (TargetDeviation > 0f)
                {
                    float distance = Vector3.Distance(targetPosition, _lateTargetPosition);
                    float alpha = 1f;
                    if (distance > 0.01f && TargetCentering > 0f)
                    {
                        alpha = Mathf.Pow(1f - TargetCentering, DeltaTime);
                    }
                    if (distance > TargetDeviation)
                    {
                        alpha = Mathf.Min(alpha, TargetDeviation / distance);
                    }
                    _lateTargetPosition = Vector3.Lerp(targetPosition, _lateTargetPosition, alpha);
                }
                else
                {
                    _lateTargetPosition = targetPosition;
                }
            }
            else
            {
                _lateTargetPosition = targetPosition;
            }
        }

        /// <summary>
        /// Sets the cameras rotation based on input.
        /// </summary>
        /// <returns>True if the rotation has changed else false.</returns>
        private bool SetRotation()
        {
            Vector2 input = new Vector2(Input.GetAxis(HorizontalAxis) * HorizontalSensitivity, Input.GetAxis(VerticalAxis) * VerticalSensitivity);

            if (input.x < -Mathf.Epsilon || input.x > Mathf.Epsilon ||
                input.y < -Mathf.Epsilon || input.y > Mathf.Epsilon)
            {
                _orbitAngles += OrbitSpeed * DeltaTime * input;
                _lastRotationTime = DeltaTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Will make the camera orbit behind the target if it comes towards the camera.
        /// </summary>
        /// <returns>True if the rotation has changed else false.</returns>
        private bool OrbitBehind()
        {
            if (DeltaTime - _lastRotationTime < StayBehindDelay)
            {
                return false;
            }
            Vector2 movement = new Vector2(_lateTargetPosition.x - _previousTargetPosition.x, _lateTargetPosition.z - _previousTargetPosition.z);
            if (movement.sqrMagnitude < Mathf.Epsilon)
            {
                return false;
            }

            float angle = GetAngleFromDirection(movement / Mathf.Sqrt(movement.sqrMagnitude));
            float delta = Mathf.Abs(Mathf.DeltaAngle(_orbitAngles.y, angle));
            float rotationDifference = OrbitSpeed * Mathf.Min(DeltaTime, movement.sqrMagnitude);
            if (delta < AlignSmoothRange)
            {
                rotationDifference *= delta / AlignSmoothRange;
            }
            else if (180f - delta < AlignSmoothRange)
            {
                rotationDifference *= (180f - delta) / AlignSmoothRange;
            }

            _orbitAngles.y = Mathf.MoveTowardsAngle(_orbitAngles.y, angle, rotationDifference);
            return true;
        }

        private float GetAngleFromDirection(Vector2 direction)
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360f - angle : angle;
        }

        private void OnValidate()
        {
            if (MaxTargetDistance <= MinTargetDistance)
            {
                MaxTargetDistance = MinTargetDistance + 0.05f;
            }

            if (MinTargetDistance < 0)
            {
                MinTargetDistance = 0f;
            }

            TargetDistance = Mathf.Clamp(TargetDistance, MinTargetDistance, MaxTargetDistance);

            if (MaxVerticalAngle < MinVerticalAngle)
            {
                MaxVerticalAngle = MinVerticalAngle;
            }
        }
    }

}