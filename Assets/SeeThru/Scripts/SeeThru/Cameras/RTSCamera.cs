// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using SeeThru.Data;
using System;
using System.Collections;
using UnityEngine;
using static SeeThru.Data.RTSCameraButtonSetup;

namespace SeeThru.Cameras
{
    [AddComponentMenu("SeeThru/RTS Camera")]
    public class RTSCamera : SeeThruCamera
    {
        [Header("RTSCamera - Axis")]
        [Tooltip("Which two axis to use for moving the camera horizontally/vertically and rotate.")]
        public RTSCameraAxis AxisSetup = RTSCameraAxis.XZ;

        [Header("RTSCamera - Controls")]
        [Tooltip("Whether to use the mouse for moving the camera or buttons.")]
        public bool UseMouseForMovement = false;
        [Tooltip("The offsets from each side of the screen that defines the areas the mouse can be in to move the camera.")]
        public RTSCameraMouseBoundary MouseBoundaries = new RTSCameraMouseBoundary()
        {
            Left = 64f,
            Right = 64f,
            Top = 128f,
            Bottom = 128f,
        };
        public string ZoomAxis = "Mouse ScrollWheel";
        [Tooltip("What buttons to use to move the camera.")]
        public RTSCameraButtonSetup ButtonSetup = new RTSCameraButtonSetup
        {
            MoveUp = KeyCode.W,
            MoveDown = KeyCode.S,
            StrafeLeft = KeyCode.A,
            StrafeRight = KeyCode.D,
            ZoomIn = KeyCode.KeypadPlus,
            ZoomOut = KeyCode.KeypadMinus
        };
        [Min(0f), Tooltip("How many units the camera zooms in/out per step.")]
        public float ZoomStep = 2f;

        [Header("RTSCamera - Movement")]
        [Min(0f), Tooltip("How fast the camera moves.")]
        public float Speed = 5f;
        [Tooltip("Whether to smooth the camera to 0 when input seizes or not.")]
        public bool UseSmoothing = true;
        [Tooltip("The time it takes for the camera to smooth step to 0 when input seizes.")]
        public float SmoothTime = 0.35f;

        [Header("RTSCamera - Rotation")]
        [Tooltip("Whether the camera can rotate around its up axis or not in increments.")]
        public bool CanRotate = false;
        [Range(0f, 90f), Tooltip("How many degrees the camera should rotate per step.")]
        public float RotationStep = 45f;
        public KeyCode RotateClockwise = KeyCode.Q;
        public KeyCode RotateCounterClockwise = KeyCode.E;
        [Min(0f), Tooltip("How long it should take the camera to lerp to its new rotation.")]
        public float RotationLerpTime = 0.25f;

        [Header("RTSCamera - Debug Options")]
        [Tooltip("Whether to draw the computed local directions for the camera. 4 colored lines should be visible for the camera to have been rotated correctly to fit the chosen AxisSetup.")]
        public bool ShowDirectionLines = true;
        public Color ForwardColor = Color.red;
        public Color BackwardsColor = Color.green;
        public Color LeftColor = Color.blue;
        public Color RightColor = Color.yellow;
        [Tooltip("Whether to draw the mouse boundary rectangle or not.")]
        public bool ShowMouseBoundary = true;

        private Vector3 _smoothingVelocity;
        private bool _isRotating;
        private Quaternion _startRot;

        public override void Start()
        {
            base.Start();
            transform.rotation = _startRot;
        }

        private void Update()
        {
            if (IsCameraActive)
            {
                Vector3 direction = Vector3.zero;
                if (UseMouseForMovement == false)
                {
                    ButtonSetup.GetDirectionFromActiveKeys(AxisSetup, gameObject.transform, out direction);
                    //direction = RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, direction);
                }
                else
                {
                    GetMouseDirection(out direction);
                }
                SetZoom();
                SetCameraPosition(direction.normalized);
                if (CanRotate)
                {
                    SetCameraRotation();
                }
            }
        }

        public void GetMouseDirection(out Vector3 direction)
        {
            Vector3 input = Vector3.zero;
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x < MouseBoundaries.Left) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, -transform.right);
            if (mousePos.x > (Screen.width - MouseBoundaries.Right)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, transform.right);
            if (mousePos.y > (Screen.height - MouseBoundaries.Top)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, transform.forward);
            if (mousePos.y < MouseBoundaries.Bottom) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, -transform.forward);
            direction = input;
        }

        private void SetZoom()
        {
            Vector3 zoomFactor = Vector3.zero;
            if (UseMouseForMovement)
            {
                float zoomDelta = Input.GetAxis(ZoomAxis);
                if (zoomDelta > 0) zoomFactor = transform.forward * ZoomStep;
                else if (zoomDelta < 0) zoomFactor = -transform.forward * ZoomStep;
            }
            else
            {
                if (Input.GetKeyDown(ButtonSetup.ZoomIn)) zoomFactor = transform.forward * ZoomStep;
                else if (Input.GetKeyDown(ButtonSetup.ZoomOut)) zoomFactor = -transform.forward * ZoomStep;
            }
            transform.position += zoomFactor;
        }

        private void SetCameraPosition(Vector3 direction)
        {
            Vector3 velocity = Vector3.zero;
            if (UseSmoothing)
            {
                // apparently turning smoothing on makes the movement about 100 times slower so we compensate.
                velocity = transform.position + direction * (Speed * 100) * DeltaTime;
                velocity = Vector3.SmoothDamp(transform.position, velocity, ref _smoothingVelocity, SmoothTime);
            }
            else
            {
                velocity = transform.position + direction * Speed * DeltaTime;
            }
            transform.position = velocity;
        }

        private void SetCameraRotation()
        {
            if (_isRotating == false)
            {
                if (Input.GetKeyDown(RotateClockwise))
                {
                    _isRotating = true;
                    StartCoroutine(DoRotation(-RotationStep));
                }
                else if (Input.GetKeyDown(RotateCounterClockwise))
                {
                    _isRotating = true;
                    StartCoroutine(DoRotation(RotationStep));
                }
            }
        }

        private IEnumerator DoRotation(float rotationValue)
        {
            float elapsed = 0f;
            Quaternion startRot = transform.rotation;
            Quaternion targetRot = GetAdjustedRotation(rotationValue) * startRot;
            while (elapsed < RotationLerpTime)
            {
                elapsed += DeltaTime;
                float alpha = elapsed / RotationLerpTime;
                transform.rotation = Quaternion.Lerp(startRot, targetRot, alpha);
                yield return null;
            }
            transform.rotation = targetRot;
            _isRotating = false;
            yield return null;
        }

        private Quaternion GetAdjustedRotation(float rotationValue)
        {
            switch (AxisSetup)
            {
                case RTSCameraAxis.YZ:
                    return Quaternion.AngleAxis(rotationValue, Vector3.right);
                case RTSCameraAxis.XZ:
                    return Quaternion.AngleAxis(rotationValue, Vector3.up);
                case RTSCameraAxis.XY:
                    return Quaternion.AngleAxis(rotationValue, Vector3.forward);
            }
            return Quaternion.identity;
        }

        private void OnValidate()
        {
            _startRot = transform.rotation;
        }

        private void OnDrawGizmos()
        {
            if (ShowDirectionLines)
            {
                float lineLength = 0.5f;
                Camera camCache = (MainCamera != null ? MainCamera : GetComponent<Camera>());
                Gizmos.color = ForwardColor;
                // forward
                Gizmos.DrawLine(
                    transform.position,
                    transform.position + (RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, transform.forward).normalized * lineLength));
                // backwards
                Gizmos.color = BackwardsColor;
                Gizmos.DrawLine(
                    transform.position,
                    transform.position + (RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, -transform.forward).normalized * lineLength));
                // left
                Gizmos.color = LeftColor;
                Gizmos.DrawLine(
                    transform.position,
                    transform.position + (RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, -transform.right).normalized * lineLength));
                // right
                Gizmos.color = RightColor;
                Gizmos.DrawLine(
                    transform.position,
                    transform.position + (RTSCameraMappings.GetTransformedDirection(AxisSetup, gameObject.transform, transform.right).normalized * lineLength));
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (UseMouseForMovement && ShowMouseBoundary)
            {
                // Apparently MainCamera is null here so we compensate.
                Camera camCache = (MainCamera != null ? MainCamera : GetComponent<Camera>());
                float nearClipPlane = Camera.main.nearClipPlane * 2;
                // due to the way unity renders these lines a very small offset
                // is needed to avoid lines from being invisible at certain Screen Offsets.
                float microOffset = 0.13f;
                float hLeftOffset = MouseBoundaries.Left + microOffset;
                float hRightOffset = MouseBoundaries.Right + microOffset;
                float vTopOffset = MouseBoundaries.Top - microOffset;
                float vBottomOffset = MouseBoundaries.Bottom - microOffset;

                /* E       G
                 * * * * * * *
                F*-|-------|-*H
                 * |       | *
                 * |       | *
                B*-|-------|-*D
                 * * * * * * *
                   A       C */

                Vector3 A = camCache.ScreenToWorldPoint(new Vector3(hLeftOffset, 0, nearClipPlane));
                Vector3 B = camCache.ScreenToWorldPoint(new Vector3(0, vBottomOffset, nearClipPlane));
                Vector3 C = camCache.ScreenToWorldPoint(new Vector3(Screen.width - hRightOffset, 0, nearClipPlane));
                Vector3 D = camCache.ScreenToWorldPoint(new Vector3(Screen.width, vBottomOffset, nearClipPlane));
                Vector3 E = camCache.ScreenToWorldPoint(new Vector3(hLeftOffset, Screen.height, nearClipPlane));
                Vector3 F = camCache.ScreenToWorldPoint(new Vector3(0, Screen.height - vTopOffset, nearClipPlane));
                Vector3 G = camCache.ScreenToWorldPoint(new Vector3(Screen.width - hRightOffset, Screen.height, nearClipPlane));
                Vector3 H = camCache.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height - vTopOffset, nearClipPlane));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(A, E); Gizmos.DrawLine(B, D);
                Gizmos.DrawLine(C, G); Gizmos.DrawLine(F, H);
            }
        }
    }
}