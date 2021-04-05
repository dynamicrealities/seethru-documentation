using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Samples.Source
{
    public class SidewaysPlayerController : MonoBehaviour
    {
        public KeyCode Up = KeyCode.W;
        public KeyCode Jump = KeyCode.Space;
        public KeyCode Left = KeyCode.A;
        public KeyCode Right = KeyCode.D;
        [Min(0f)]
        public float GravityModifier = 1f;
        [Min(0f)]
        public float Speed = 5f;

        private Vector3 _velocity = Vector3.zero;
        private Rigidbody _rBody;

        private void Start()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            Vector3 input = Vector3.zero;
            if (Input.GetKey(Up) || Input.GetKey(Jump)) input += (transform.up * 3);
            if (Input.GetKey(Left)) input += -transform.right;
            if (Input.GetKey(Right)) input += transform.right;
            _velocity += (input * Speed * Time.deltaTime);
        }

        public void FixedUpdate()
        {
            //_velocity += GravityModifier * Physics.gravity * Time.deltaTime;
            _velocity *= _rBody.angularDrag;
            transform.position += _velocity;
        }
    }
}
