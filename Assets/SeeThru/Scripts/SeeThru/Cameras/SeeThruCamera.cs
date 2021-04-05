// SeeThru © Dynamic Realities - https://twitter.com/DynRealities
// Documentation: https://github.com/dynamicrealities/seethru-documentation/wiki

using UnityEngine;

namespace SeeThru.Cameras
{
    [RequireComponent(typeof(Camera))]
    public abstract class SeeThruCamera : MonoBehaviour
    {
        [Header("General")]
        [Tooltip("Whether this camera is currently in use or not.")]
        public bool IsCameraActive = true;
        [Tooltip("Whether to use Scaled or Unscaled Time for movement.")]
        public bool UseUnscaledTime = false;
        [Tooltip("Whether the cursor is locked to the screen, confined or neither.")]
        public CursorLockMode cursorLockMode = CursorLockMode.None;
        [Tooltip("Whether the cursor is visible or not.")]
        public bool IsCursorVisible = true;

        public Camera MainCamera { get; set; }
        public float DeltaTime => UseUnscaledTime == true ? Time.unscaledDeltaTime : Time.deltaTime;

        public virtual void Awake()
        {
            MainCamera = GetComponent<Camera>();
        }

        public virtual void Start()
        {
            Cursor.lockState = cursorLockMode;
            Cursor.visible = IsCursorVisible;
        }
    }
}