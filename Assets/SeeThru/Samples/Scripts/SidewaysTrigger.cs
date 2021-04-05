using SeeThru.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeeThru.Samples
{
    public class SidewaysTrigger : MonoBehaviour
    {
        private TargetFramingCamera _camera;

        private void Start()
        {
            _camera = FindObjectOfType<TargetFramingCamera>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player" && !_camera.Targets.Contains(other.gameObject))
            {
                _camera.Targets.Add(other.gameObject);
            }
        }
    }
}
