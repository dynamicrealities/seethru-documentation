using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingCameraArgs : MonoBehaviour
{
    [Min(0.2f)]
    public float Distance = 0.2f;
    [Min(0.2f)]
    public float MinDistance = 0.2f;
    [Min(1f)]
    public float MaxDistance = 1;
}
