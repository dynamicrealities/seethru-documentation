using SeeThru.Cameras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCameraManager : MonoBehaviour
{
    public OrbitingCamera OrbCamera;
    public List<Transform> CelestialBodies;
    private int _index = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _index += 1;
            _index = Mathf.Clamp(_index, 0, CelestialBodies.Count - 1);
            if (CelestialBodies[_index] != OrbCamera.Target)
            {
                UpdatePlanetTarget();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _index -= 1;
            _index = Mathf.Clamp(_index, 0, CelestialBodies.Count - 1);
            if (CelestialBodies[_index] != OrbCamera.Target)
            {
                UpdatePlanetTarget();
            }
        }
    }

    private void UpdatePlanetTarget()
    {
        OrbitingCameraArgs args = CelestialBodies[_index].gameObject.GetComponent<OrbitingCameraArgs>();
        OrbCamera.Target = CelestialBodies[_index];
        OrbCamera.TargetDistance = args.Distance;
        OrbCamera.MinTargetDistance = args.MinDistance;
        OrbCamera.MaxTargetDistance = args.MaxDistance;
    }
}
