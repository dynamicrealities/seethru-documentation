// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Data;
using UnityEngine;

namespace SeeThru.Cameras
{
    [AddComponentMenu("SeeThru/Free Flying Camera")]
    public class FreeFlyingCamera : SeeThruCamera
    {
        [Header("FreeFlyingCamera - Controls")]
        [Tooltip("Whether to use Axis for camera movement or not.")]
        public bool UseAxisForMovement = false;
        [Tooltip("The horizontal movement axis name.")]
        public string HorizontalMovementAxis = "Horizontal";
        [Tooltip("The vertical movement axis name.")]
        public string VerticalMovementAxis = "Vertical";
        [Tooltip("What keys to use for moving this camera around in 3D space.")]
        public FreeFlyingCameraButtonSetup buttonSetup = new FreeFlyingCameraButtonSetup
        {
            MoveForward = KeyCode.W,
            MoveBackwards = KeyCode.S,
            StrafeLeft = KeyCode.A,
            StrafeRight = KeyCode.D,
            MoveUp = KeyCode.E,
            MoveDown = KeyCode.Q,
            SpeedUp = KeyCode.KeypadPlus,
            Slowdown = KeyCode.KeypadMinus,
            DoubleSpeed = KeyCode.LeftShift,
            HalfSpeed = KeyCode.LeftControl
        };

        [Header("FreeFlyingCamera - Movement")]
        [Tooltip("Whether to confine the camera within an area or not.")]
        public bool UseCage = false;
        [Tooltip("The box collider that defines the confines of the cage.")]
        public BoxCollider Cage = null;

        [Header("FreeFlyingCamera - Rotation")]
        [Tooltip("Whether the rotation should be inverted or not.")]
        public bool InvertedRotation = false;
        [Tooltip("The horizontal rotation axis name.")]
        public string HorizontalRotationAxis = "Mouse X";
        [Tooltip("The vertical rotation axis name.")]
        public string VerticalRotationAxis = "Mouse Y";

        [Header("FreeFlyingCamera - Sensitivity")]
        [Tooltip("The horizontal sensitivity")]
        public float HorizontalSensitivity = 1f;
        [Tooltip("The vertical sensitivity")]
        public float VerticalSensitivity = 1f;

        [Header("FreeFlyingCamera - Speed")]
        [Tooltip("How fast the camera moves.")]
        public float MovementSpeed = 5f;
        [Tooltip("Whether to smooth the camera to 0 when input seizes or not.")]
        public bool UseSmoothing = true;
        [Tooltip("The time it takes for the camera to smooth step to 0 when input seizes.")]
        public float SmoothTime = 0.35f;
        [Tooltip("If this is set to true, then the provided axis is read. If false then the button setup is used.")]
        public bool UseAxisForSpeed = true;
        [Tooltip("The name of the axis used to read whether to increase or decrease speed. Only used if UseAxisForSpeed is true.")]
        public string SpeedStepAxis = "Mouse ScrollWheel";
        [Tooltip("This value is added/subtracted to/from the camera speed when changing camera speed.")]
        public float SpeedStep = 10f;

        private Vector3 _camAngles;
        private Vector3 _smoothingVelocity;

        public override void Start()
        {
            base.Start();
            if (UseCage == true)
            {
                if (Cage != null)
                {
                    Cage.isTrigger = true;
                }
                else
                {
                    Debug.LogError("'UseCage' is set to true, but there was no BoxCollider provided as the 'Cage'. UseCage will be set to false.");
                    UseCage = false;
                }
            }
        }

        private void Update()
        {
            if (IsCameraActive == true)
            {
                if (UseAxisForMovement == true)
                {
                    float horizontal = Input.GetAxis(HorizontalMovementAxis);
                    float vertical = Input.GetAxis(VerticalMovementAxis);
                    Vector3 direction = new Vector3(horizontal, 0, vertical);
                    if (vertical > 0) direction += transform.forward;
                    if (vertical < 0) direction += -transform.forward;
                    if (horizontal < 0) direction += -transform.right;
                    if (horizontal > 0) direction += transform.right;
                    SetCameraPosition(direction.normalized);
                }
                else
                {
                    buttonSetup.GetDirectionFromActiveKeys(gameObject.transform, out Vector3 direction);
                    SetCameraPosition(direction);
                }
                SetCameraMovementSpeed();
                SetCameraRotation();
            }
        }

        private void SetCameraPosition(Vector3 direction)
        {
            Vector3 velocity = Vector3.zero;
            if (UseSmoothing)
            {
                // apparently turning smoothing on makes the movement about 100 times slower so we compensate.
                velocity = transform.position + direction * (Mathf.Abs(MovementSpeed) * 100) * DeltaTime;
                velocity = Vector3.SmoothDamp(transform.position, velocity, ref _smoothingVelocity, SmoothTime);
            }
            else
            {
                velocity = transform.position + direction * Mathf.Abs(MovementSpeed) * DeltaTime;
            }
            if (UseCage == true)
            {
                if (Cage.bounds.Contains(velocity))
                {
                    transform.position = velocity;
                }
            }
            else
            {
                transform.position = velocity;
            }
        }

        private void SetCameraMovementSpeed()
        {
            if (UseAxisForSpeed == true)
            {
                float scrollDelta = Input.GetAxis(SpeedStepAxis);
                if (scrollDelta > 0)
                {
                    MovementSpeed += SpeedStep;
                }
                else if (scrollDelta < 0)
                {
                    MovementSpeed -= SpeedStep;
                }
            }
            else
            {
                if (Input.GetKeyDown(buttonSetup.SpeedUp))
                {
                    MovementSpeed += SpeedStep;
                }
                else if (Input.GetKeyDown(buttonSetup.Slowdown))
                {
                    MovementSpeed -= SpeedStep;
                }
            }

            MovementSpeed = Mathf.Max(0f, MovementSpeed);
        }

        private void SetCameraRotation()
        {
            float rotationX = Input.GetAxis(HorizontalRotationAxis) * HorizontalSensitivity;
            float rotationY = Input.GetAxis(VerticalRotationAxis) * VerticalSensitivity;
            if (InvertedRotation == true)
            {
                rotationX *= -1f;
                rotationY *= -1f;
            }

            if (rotationY > 0)
            {
                _camAngles = new Vector3(Mathf.MoveTowards(_camAngles.x, -90f, rotationY), _camAngles.y + rotationX, 0);
            }
            else
            {
                _camAngles = new Vector3(Mathf.MoveTowards(_camAngles.x, 90f, -rotationY), _camAngles.y + rotationX, 0);
            }
            transform.rotation = Quaternion.identity * Quaternion.Euler(_camAngles);
        }

        //public override void OnValidate()
        //{
        //    base.OnValidate();
        //}
    }
}
