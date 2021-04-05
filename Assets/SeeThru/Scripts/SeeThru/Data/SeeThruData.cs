// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using System;
using UnityEditor;
using UnityEngine;

namespace SeeThru.Data
{
    [Serializable]
    public struct FreeFlyingCameraButtonSetup
    {
        public KeyCode MoveForward;
        public KeyCode MoveBackwards;
        public KeyCode StrafeLeft;
        public KeyCode StrafeRight;
        public KeyCode MoveUp;
        public KeyCode MoveDown;
        public KeyCode SpeedUp;
        public KeyCode Slowdown;
        public KeyCode DoubleSpeed;
        public KeyCode HalfSpeed;

        public void GetDirectionFromActiveKeys(Transform caller, out Vector3 movementVector)
        {
            Vector3 input = Vector3.zero;
            if (Input.GetKey(MoveForward)) input += caller.forward;
            if (Input.GetKey(MoveBackwards)) input += -caller.forward;
            if (Input.GetKey(StrafeLeft)) input += -caller.right;
            if (Input.GetKey(StrafeRight)) input += caller.right;
            if (Input.GetKey(MoveUp)) input += Vector3.up;
            if (Input.GetKey(MoveDown)) input += Vector3.down;
            if (Input.GetKey(DoubleSpeed))
            {
                movementVector = input.normalized * 2;
            }
            else if (Input.GetKey(HalfSpeed))
            {
                movementVector = input.normalized * 0.5f;
            }
            else
            {
                movementVector = input.normalized;
            }
        }
    }

    [Serializable]
    public struct OrbitCameraButtonSetup
    {
        public KeyCode OrbitUp;
        public KeyCode OrbitDown;
        public KeyCode OrbitLeft;
        public KeyCode OrbitRight;
        public KeyCode ZoomIn;
        public KeyCode ZoomOut;

        public void GetDirectionFromActiveKeys(Transform caller, out Vector2 movementVector)
        {
            Vector3 input = Vector3.zero;
            if (Input.GetKey(OrbitUp)) input += caller.right;
            if (Input.GetKey(OrbitDown)) input += -caller.right;
            if (Input.GetKey(OrbitLeft)) input += caller.up;
            if (Input.GetKey(OrbitRight)) input += -caller.up;
            movementVector = input.normalized;
        }
    }

    [Serializable]
    public struct RTSCameraButtonSetup
    {
        public KeyCode MoveUp;
        public KeyCode MoveDown;
        public KeyCode StrafeLeft;
        public KeyCode StrafeRight;
        public KeyCode ZoomIn;
        public KeyCode ZoomOut;

        public void GetDirectionFromActiveKeys(RTSCameraAxis AxisSetup, Transform caller, out Vector3 movementVector)
        {
            Vector3 input = Vector3.zero;
            if (Input.GetKey(MoveUp)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, caller, caller.forward);
            if (Input.GetKey(MoveDown)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, caller, -caller.forward);
            if (Input.GetKey(StrafeLeft)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, caller, -caller.right);
            if (Input.GetKey(StrafeRight)) input += RTSCameraMappings.GetTransformedDirection(AxisSetup, caller, caller.right);
            movementVector = input;
        }
    }

    [Serializable]
    public struct RTSCameraMouseBoundary
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
    }


    public static class RTSCameraMappings
    {
        public static Vector3 GetTransformedDirection(RTSCameraAxis axisSetup, Transform caller, Vector3 direction)
        {
            Vector3 result = Vector3.zero;
            switch (axisSetup)
            {
                case RTSCameraAxis.YZ:
                    if (direction == caller.right) result = Vector3.Cross(Vector3.left, caller.forward); // right
                    else if (direction == -caller.right) result = -Vector3.Cross(Vector3.left, caller.forward); // left
                    else if (direction == -caller.forward) result = Vector3.Cross(Vector3.left, caller.right); // down
                    else if (direction == caller.forward) result = -Vector3.Cross(Vector3.left, caller.right); // up
                    break;
                case RTSCameraAxis.XZ:
                    if (direction == caller.right) result = Vector3.Cross(Vector3.up, caller.forward); // right
                    else if (direction == -caller.right) result = -Vector3.Cross(Vector3.up, caller.forward); // left
                    else if (direction == -caller.forward) result = Vector3.Cross(Vector3.up, caller.right); // down
                    else if (direction == caller.forward) result = -Vector3.Cross(Vector3.up, caller.right); // up
                    break;
                case RTSCameraAxis.XY:
                    if (direction == caller.right) result = Vector3.Cross(Vector3.up, caller.forward); // right
                    else if (direction == -caller.right) result = -Vector3.Cross(Vector3.up, caller.forward); // left
                    else if (direction == -caller.forward) result = Vector3.Cross(Vector3.back, caller.right); // down
                    else if (direction == caller.forward) result = -Vector3.Cross(Vector3.back, caller.right); // up
                    break;
            }
            return result;
        }
    }
}
