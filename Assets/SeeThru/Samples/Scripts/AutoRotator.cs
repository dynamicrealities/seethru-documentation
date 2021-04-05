using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [Range(1f,360f)]
    public float Speed = 45f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Speed * Time.deltaTime);
    }
}
