using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeeThru.Samples
{
    public class TopdownPlayerController : MonoBehaviour
    {
        public float Speed = 5f;
        // Update is called once per frame
        void Update()
        {
            Vector3 input = Vector3.zero;
            Vector3 adjustedInput = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) input += transform.forward;
            if (Input.GetKey(KeyCode.S)) input += -transform.forward;
            if (Input.GetKey(KeyCode.D)) input += transform.right;
            if (Input.GetKey(KeyCode.A)) input += -transform.right;
            input = input.normalized;
            adjustedInput = transform.position + new Vector3(input.x, 0, input.z) * Speed * Time.deltaTime;
            RaycastHit[] hits;
            if ((hits = Physics.SphereCastAll(new Ray(transform.position, input), 0.5f, 1f)).Length > 0)
            {
                bool containsWall = new List<RaycastHit>(hits).Exists(hit => hit.collider.gameObject.tag == "TopdownWall");
                if (!containsWall)
                {
                    transform.position = adjustedInput;
                }
            }
            else
            {
                transform.position = adjustedInput;
            }
        }
    }

}